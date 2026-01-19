namespace TellerDomain
{
    public class AddressDTO
    {
        public  int Id { get; set; }
        public string Street1 { get; set; }
        public string? Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public int? ZipPlusFour { get; set; }
        public int MemberId { get; set; }
        public bool IsActive { get; set; }
    }
}
