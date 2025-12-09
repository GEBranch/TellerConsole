using Serilog;
using TellerConsole.Database;
using TellerConsole.Modules;

namespace TellerConsole.Transactions
{
    public class Deposit : Transaction
    {
        public override void ProcessTransaction(Member member, decimal transactionAmount)
        {
            if (transactionAmount <= 0)
            {
                Log.Information($"Account: {member.AccountNumber} - Deposit amount must be greater than zero.");
                throw new ArgumentException("Deposit amount must be greater than zero.");
            }

            member.Balance += transactionAmount;
            member = MemberDB.UpdateMember(member);
        }
    }
}
