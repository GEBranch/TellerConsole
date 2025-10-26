namespace TellerConsole
{
    internal class Withdraw : Transaction
    {
        public override void ProcessTransaction(Member member, decimal transactionAmount)
        {
            decimal origBalance = member.Balance;
            if (transactionAmount <= 0)
            {
                throw new ArgumentException("Withdrawal amount must be greater than zero.");
            }

            member.Balance -= transactionAmount;

            if (member.Balance < 0)
            {
                member.Balance = origBalance;
                throw new InvalidDataException($"Insufficient Funds - Current Balance: {member.Balance}");
            }

            member = MemberDB.UpdateMember(member);
        }
    }
}
