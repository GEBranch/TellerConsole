using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TellerDB
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        [MaxLength(50)]
        public int AccountNumber { get; set; }

        public AccountType AccountType { get; set; }

        [DataType(DataType.Currency)]
        [Precision(18, 2)]
        public decimal Balance { get; set; }

        public Member Member { get; set; }
        [ForeignKey("AccountNumber")]
        public int MemberId { get; set; }

        public bool IsActive { get; set; }
    }
}
