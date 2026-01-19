using TellerDB;

namespace TellerDomain
{
    public class Transaction
    {
        public AccountType AccountType { get; set; }
        public TransactionType TransactionType { get; set; }
        public MemberDTO? MemberDTO { get; set; } = new MemberDTO();
        public AccountDTO? AccountDTO { get; set; } = new AccountDTO();
        public decimal AmountToProcess { get; set; }
        public decimal OriginalAccountBalance { get; set; }
        public int AccountNumber { get; set; }
        public int AccountId { get; set; }
        public virtual Transaction ProcessTransaction(Transaction transaction) 
        {
            throw new NotImplementedException();
        }
    }
}
