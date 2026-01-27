namespace TellerDomain
{
    public class MemberDTO
    {
        public int MemberId { get; set; }
        public int AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AddressDTO Address { get; set; }
        public List<AccountDTO> Accounts { get; set; } = new List<AccountDTO>();
        public bool IsActive { get; set; }
    }
}
