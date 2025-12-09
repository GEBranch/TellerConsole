using System.ComponentModel.DataAnnotations;
using TellerConsole.Members;
using TellerConsole.Modules;

namespace TellerConsole.Transactions
{
    public class Transaction
    {
        public AccountType Type { get; set; }
        public decimal Amount { get; set; }
        public decimal AccountBalance { get; set; }
        public TransactionType TransactionType { get; set; }
        public int AccountNumber { get; set; }
        public virtual void ProcessTransaction(Member member, decimal transactionAmount) 
        {
            throw new NotImplementedException();
        }
    }
}
