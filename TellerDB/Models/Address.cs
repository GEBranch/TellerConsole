using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TellerDB
{
    public class Address
    {
        public int AddressId { get; set; }

        public int AccountNumber { get; set; }
        
        [MaxLength(100)] 
        public required string Street1 { get; set; }

        [MaxLength(100)]
        [AllowNull]
        public string? Street2 { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(2)]
        public string State { get; set; }

        public int ZipCode { get; set; }

        [AllowNull]
        public int? ZipPlusFour { get; set; }

        public bool IsActive { get; set; }
    }
}
