
namespace TellerDomain
{
    public class Transaction
    {
        public AccountType AccountType { get; set; } = AccountType.Undefined;
        public TransactionType TransactionType { get; set; } = TransactionType.Undefined;
        public MemberDTO? MemberDTO { get; set; } = new MemberDTO();
        public AccountDTO? AccountDTO { get; set; } = new AccountDTO();
        public decimal AmountToProcess { get; set; } = 0;
        public decimal OriginalAccountBalance { get; set; } = 0;
        public int AccountNumber { get; set; } = 0;
        public int AccountId { get; set; } = 0;
        public bool QuitProgram { get; set; } = false;
        public virtual Transaction ProcessTransaction(Transaction transaction) 
        {
            throw new NotImplementedException();
        }
    }
}
