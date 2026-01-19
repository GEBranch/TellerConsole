
using TellerDomain;

namespace TellerConsole
{
    public static class Extensions
    {
        public static bool IsValidTransactionType(this TransactionType transactionType)
        {
            switch (transactionType)
            {
                case TransactionType.Deposit:
                case TransactionType.Withdrawal:
                    return true;
                default:
                    return false;
            }
        }

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
    }


}
