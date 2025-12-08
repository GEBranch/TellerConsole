using TellerConsole.Database;
using TellerConsole.Modules;

namespace TellerConsole
{

    public class Deposit : Transactions.Transaction
    {
        public override void ProcessTransaction(Member member, decimal transactionAmount)
        {
            if (transactionAmount <= 0)
            {
                throw new ArgumentException("Deposit amount must be greater than zero.");
            }

            member.Balance += transactionAmount;
            member = MemberDB.UpdateMember(member);
        }
    }
}
