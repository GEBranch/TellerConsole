using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TellerDB
{
    public class Member
    {
        public int MemberId { get; set; }

        public int AccountNumber { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public Address Address { get; set; }
        public int AddressId { get; set; }

        public List<Account>? Accounts { get; set; } = new List<Account>();

        public bool IsActive { get; set; }

    }
}
