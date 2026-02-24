using AutoMapper;
using Harfien.Application.DTO.Payment;
using Harfien.Application.Interfaces.payment_interfaces;
using Harfien.Domain.Entities;
using Harfien.Domain.Enums;
using Harfien.Domain.Shared.Repositories;
using Harfien.Domain.Shared;

public class WalletTransactionService : IWalletTransactionService
{
    private readonly IWalletRepository _walletRepo;
    private readonly IWalletTransactionRepository _transactionRepo;
    private readonly IMapper _mapper;

    public WalletTransactionService(
        IWalletRepository walletRepo,
        IWalletTransactionRepository transactionRepo,
        IMapper mapper)
    {
        _walletRepo = walletRepo;
        _transactionRepo = transactionRepo;
        _mapper = mapper;
    }


    public async Task<WalletTransactionDto> CreateAsync(string userId, CreateWalletTransactionDto dto)
    {
        if (dto.Amount <= 0)
            throw new Exception("Amount must be greater than zero");

        var wallet = await _walletRepo.GetByUserIdAsync(userId);
        if (wallet == null)
        {
            wallet = new Wallet
            {
                UserId = userId,
                Balance = 0
            };
            await _walletRepo.AddAsync(wallet);
            await _walletRepo.SaveAsync(); 
        }

        
        if (dto.Type == TransactionType.Debit)
        {
            if (wallet.Balance < dto.Amount)
                throw new Exception("Insufficient balance");

            wallet.Balance -= dto.Amount;
        }
        else
        {
            wallet.Balance += dto.Amount;
        }

        var transaction = new WalletTransaction
        {
            WalletId = wallet.Id,
            Amount = dto.Amount,
            Type = dto.Type,
            Status = TransactionStatus.Completed,
            TransactionReason = dto.Reason,
            Reference = Guid.NewGuid().ToString()
        };

        await _transactionRepo.AddAsync(transaction);
        _walletRepo.Update(wallet);

        await _transactionRepo.SaveAsync(); 

        return _mapper.Map<WalletTransactionDto>(transaction);
    }


    //public async Task<WalletTransactionDto> CreateAsync(string userId, CreateWalletTransactionDto dto)
    //{
    //    if (dto.Amount <= 0)
    //        throw new Exception("Amount must be greater than zero");

    //    var wallet = await _walletRepo.GetByUserIdAsync(userId);
    //    if (wallet == null)
    //        throw new Exception("Wallet not found");

    //    if (dto.Type == TransactionType.Debit)
    //    {
    //        if (wallet.Balance < dto.Amount)
    //            throw new Exception("Insufficient balance");

    //        wallet.Balance -= dto.Amount;
    //    }
    //    else
    //    {
    //        wallet.Balance += dto.Amount;
    //    }

    //    var transaction = new WalletTransaction
    //    {
    //        WalletId = wallet.Id,
    //        Amount = dto.Amount,
    //        Type = dto.Type,
    //        Status = TransactionStatus.Completed,
    //        TransactionReason = dto.Reason,
    //        Reference = Guid.NewGuid().ToString()
    //    };

    //    await _transactionRepo.AddAsync(transaction);
    //    _walletRepo.Update(wallet);

    //    await _transactionRepo.SaveAsync();

    //    return _mapper.Map<WalletTransactionDto>(transaction);
    //}

    public async Task<PagedResult<WalletTransactionDto>> GetMyTransactionsAsync(
        string userId, int pageNumber, int pageSize)
    {
        if (pageNumber <= 0) pageNumber = 1;
        if (pageSize <= 0 || pageSize > 50) pageSize = 10;

        var skip = (pageNumber - 1) * pageSize;

        var totalCount = await _transactionRepo.GetTransactionsCountByUserIdAsync(userId);

        var transactions = await _transactionRepo
            .GetTransactionsByUserIdAsync(userId, skip, pageSize);

        return new PagedResult<WalletTransactionDto>
        {
            Items = _mapper.Map<List<WalletTransactionDto>>(transactions),
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
