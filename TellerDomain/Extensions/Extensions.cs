

using TellerDomain;

namespace TellerDomain
{
    public static class Extensions
    {
        public static bool IsValidAccountType(this AccountType accountType)
        {
            switch (accountType)
            {
                case AccountType.Checking:
                case AccountType.Savings:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsValidTransactionType(this TransactionType transactionType)
        {
            switch (transactionType)
            {
                case TransactionType.Withdrawal:
                case TransactionType.Deposit:
                    return true;
                default:
                    return false;
            }
        }
    }
}
