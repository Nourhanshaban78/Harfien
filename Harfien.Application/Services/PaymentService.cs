
using Stripe;
using Harfien.Application.DTO.Payment;
using Harfien.Application.Interfaces.payment_interfaces;
using Harfien.Domain.Interface_Repository.Repositories;
using Harfien.Domain.Shared.Repositories;
using Microsoft.Extensions.Configuration;
using Harfien.Domain.Entities;
using Harfien.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Harfien.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IWalletRepository _walletRepo;
        private readonly IUserRepository _userRepo;
        private readonly ICraftsmanRepository _craftsmanRepo;
        private readonly IClientRepository _clientRepo;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService( IPaymentRepository paymentRepo,  IOrderRepository orderRepo,
            IWalletRepository walletRepo,  IUserRepository userRepo,
            IConfiguration config,  ICraftsmanRepository craftsmanRepo,
            IClientRepository clientRepo , IUnitOfWork unitOfWork)
        {
            _paymentRepo = paymentRepo;
            _orderRepo = orderRepo;
            _walletRepo = walletRepo;
            _userRepo = userRepo;
            _craftsmanRepo = craftsmanRepo;
            _clientRepo = clientRepo;
            _config = config;

            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];
        }


        public async Task<PaymentResultDto> PayOrderWithCardAsync(CreatePaymentDto dto, string clientId)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.stripeToken))
                    return new PaymentResultDto
                    {
                        Success = false,
                        Message = "Stripe token is required"
                    };

                var order = await _orderRepo.GetByIdWithDetailsAsync(dto.OrderId);
                if (order == null)
                    return new PaymentResultDto { Success = false, Message = "Order not found" };

                if (order.Amount <= 0)
                    return new PaymentResultDto
                    {
                        Success = false,
                        Message = "Invalid order amount"
                    };

                if (order.ClientId == null)
                    return new PaymentResultDto { Success = false, Message = "Order has no client associated" };

                var client = await _clientRepo.GetByIdAsync(order.ClientId);
                if (client == null)
                    return new PaymentResultDto { Success = false, Message = "Client not found" };

                if (client.UserId != clientId)
                    return new PaymentResultDto { Success = false, Message = "Unauthorized" };

                bool isPaid = order.Payment != null && order.Payment.Status == PaymentStatus.Paid;
                if (isPaid)
                    return new PaymentResultDto { Success = false, Message = "Order already paid" };

                if (order.CraftsmanId == null)
                    return new PaymentResultDto { Success = false, Message = "Order has no craftsman associated" };

                var craftsman = await _craftsmanRepo.GetByIdAsync(order.CraftsmanId);
                if (craftsman == null)
                    return new PaymentResultDto { Success = false, Message = "Craftsman not found" };


                var craftsmanWallet = await _walletRepo.GetByUserIdAsync(craftsman.UserId);
                bool isNewCraftsmanWallet = false;

                if (craftsmanWallet == null)
                {
                    craftsmanWallet = new Wallet
                    {
                        UserId = craftsman.UserId,
                        Balance = 0,
                        IsActive = true,
                        Transactions = new List<WalletTransaction>()
                    };
                    await _walletRepo.AddAsync(craftsmanWallet);
                    isNewCraftsmanWallet = true;
                }

                var admin = await _userRepo.GetAdminAsync();
                if (admin == null)
                    return new PaymentResultDto { Success = false, Message = "Admin user not found" };


                var adminWallet = await _walletRepo.GetByUserIdAsync(admin.Id);
                bool isNewAdminWallet = false;

                if (adminWallet == null)
                {
                    adminWallet = new Wallet
                    {
                        UserId = admin.Id,
                        Balance = 0,
                        IsActive = true,
                        Transactions = new List<WalletTransaction>()
                    };
                    await _walletRepo.AddAsync(adminWallet);
                    isNewAdminWallet = true;
                }

                var paymentMethodService = new PaymentMethodService();
                var paymentMethod = await paymentMethodService.CreateAsync(new PaymentMethodCreateOptions
                {
                    Type = "card",
                    Card = new PaymentMethodCardOptions
                    {
                        Token = dto.stripeToken
                    }
                });

                if (paymentMethod == null || string.IsNullOrEmpty(paymentMethod.Id))
                    return new PaymentResultDto
                    {
                        Success = false,
                        Message = "Failed to create payment method"
                    };


                var paymentIntentService = new PaymentIntentService();
                var paymentIntent = await paymentIntentService.CreateAsync(new PaymentIntentCreateOptions
                {
                    Amount = (long)(order.Amount * 100),
                    Currency = _config["StripeSettings:Currency"],
                    PaymentMethod = paymentMethod.Id,
                    PaymentMethodTypes = new List<string> { "card" },
                    Confirm = true
                });


                if (paymentIntent == null)
                    return new PaymentResultDto
                    {
                        Success = false,
                        Message = "Payment intent creation failed"
                    };

                if (paymentIntent.Status != "succeeded")
                    return new PaymentResultDto { Success = false, Message = "Stripe payment failed" };


                decimal craftsmanShare = order.Amount * 0.9m;
                decimal adminShare = order.Amount * 0.1m;


                AddTransaction(craftsmanWallet, craftsmanShare, TransactionType.Credit, order.Id, "Order Payment Share");
                AddTransaction(adminWallet, adminShare, TransactionType.Credit, order.Id, "Platform Commission");


                if (!isNewCraftsmanWallet)
                {
                    _walletRepo.Update(craftsmanWallet);
                }
                else
                {
                    await _walletRepo.SaveAsync();
                }

                if (!isNewAdminWallet)
                {
                    _walletRepo.Update(adminWallet);
                }
                else
                {
                    await _walletRepo.SaveAsync();
                }


                var payment = new Payment
                {
                    OrderId = order.Id,
                    Amount = order.Amount,
                    Status = PaymentStatus.Paid,
                    TransactionRef = paymentIntent.Id
                };

                order.Payment = payment;

                await _paymentRepo.AddAsync(payment);
                _orderRepo.Update(order);
                await _paymentRepo.SaveAsync();
                return new PaymentResultDto { Success = true, Message = "Payment successful via card" };
            }

            catch (StripeException stripeEx)
            {
                return new PaymentResultDto
                {
                    Success = false,
                    Message = stripeEx.StripeError?.Message ?? "Stripe payment failed"
                };
            }
            catch (Exception ex)
            {
                return new PaymentResultDto { Success = false, Message = $"Payment failed: {ex.Message}" };
            }
        }

        private void AddTransaction(Wallet wallet, decimal amount, TransactionType type, int orderId, string reason)
        {
            if (wallet == null)
                throw new ArgumentNullException(nameof(wallet));

            wallet.Balance += type == TransactionType.Credit ? amount : -amount;

            if (wallet.Transactions == null)
                wallet.Transactions = new List<WalletTransaction>();

            wallet.Transactions.Add(new WalletTransaction
            {
                Amount = amount,
                Type = type,
                Status = TransactionStatus.Completed,
                TransactionReason = reason,
                OrderId = orderId,
                Reference = Guid.NewGuid().ToString()
            });
        }
    }
}