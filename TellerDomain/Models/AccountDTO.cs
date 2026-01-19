

namespace TellerDomain
{
    public class AccountDTO
    {
        public int AccountId { get; set; }
        public int AccountNumber { get; set; }
        public AccountType AccountType { get; set; }
        public decimal Balance { get; set; }
        public MemberDTO MemberDTO { get; set; }
        public int MemberId { get; set; }
        public bool IsActive { get; set; }
    }
}
