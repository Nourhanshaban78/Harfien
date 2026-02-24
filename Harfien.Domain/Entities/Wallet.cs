using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Domain.Entities
{
    public class Wallet: BaseEntity
    {
        
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
       

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<WalletTransaction> Transactions { get; set; }
    }
}
