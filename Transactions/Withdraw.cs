using Serilog;
using TellerConsole.Database;
using TellerConsole.Modules;

namespace TellerConsole.Transactions
{
    internal class Withdraw : Transaction
    {
        public override void ProcessTransaction(Member member, decimal transactionAmount)
        {
            decimal origBalance = member.Balance;
            if (transactionAmount <= 0)
            {
                Log.Information($"Account: {member.AccountNumber} - Withdrawal amount must be greater than zero.");

                throw new ArgumentException("Withdrawal amount must be greater than zero.");
            }

            member.Balance -= transactionAmount;

            if (member.Balance < 0)
            {
                member.Balance = origBalance;
                Log.Information($"Account: {member.AccountNumber} - Insufficient Funds - Current Balance: {member.Balance}.");
                throw new InvalidDataException($"Insufficient Funds - Current Balance: {member.Balance}");
            }

            member = MemberDB.UpdateMember(member);
        }
    }
}
