namespace TellerConsole
{
    public static class Extensions
    {
        public static bool IsInRange(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        public static string IsValidTransactionType(this int transactionType)
        {
            switch (transactionType)
            {
                case (int)TransactionType.Deposit:
                case (int)TransactionType.Withdrawal:
                    return string.Empty;
                default:
                    return $"{transactionType} is not a valid Transaction Type";
            }
        }

        public static string IsValidAccountType(this int accountType)
        {
            switch (accountType)
            {
                case (int)AccountType.Checking:
                case (int)AccountType.Savings:
                    return string.Empty;
                default:
                    return $"{accountType} is not a valid Account Type";
            }
        }
    }


}
