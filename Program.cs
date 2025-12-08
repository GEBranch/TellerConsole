using TellerConsole;
using TellerConsole.Database;
using TellerConsole.Extensions;
using TellerConsole.Modules;
using TellerConsole.Transactions;



// Initialize the Member database with default values.
MemberDB.InitializeDB();

bool processTran = true;
while (processTran == true)
{
    try
    {
        Member member = new Member();
        string tranAmount;
        decimal transactionAmount = 0m;
        decimal beginningBalance = 0m;
        string acctType;
        int accountType;
        string tranType;
        int transactionType = 0;
        string acctNumber;
        int accountNumber;

        bool isValidTransaction = false;

        Console.WriteLine("_____________________________");
        Console.WriteLine("Processing new transaction...");
        Console.WriteLine("_____________________________");


        do
        {
            Console.Write("Enter the Account Number: ");
            acctNumber = Console.ReadLine();
            accountNumber = Convert.ToInt32(acctNumber);

            Console.Write("Enter the Account Type: (Checking = 1, Savings = 2): ");
            acctType = Console.ReadLine();
            accountType = Convert.ToInt32(acctType);
            if (!string.IsNullOrWhiteSpace(accountType.IsValidAccountType()))
            {
                Console.WriteLine(accountType.IsValidAccountType());

                isValidTransaction = false;
                continue;
            }

            member = Member.GetMemberByAccountNumberAndType(accountNumber, accountType);
            if (member.AccountNumber == 0)
            {
                isValidTransaction = false;
                continue;
            }

            beginningBalance = member.Balance;

            Console.Write("Enter the Transaction Type (Deposit = 1, Withdrawal = 2): ");
            tranType = Console.ReadLine();
            transactionType = Convert.ToInt32(tranType);
            if (!transactionType.IsInRange(1, 2))
            {
                Console.WriteLine(transactionType.IsValidTransactionType());
                isValidTransaction = false;
                continue;
            }

            Console.Write("Enter the Amount: ");
            tranAmount = Console.ReadLine();
            transactionAmount = Convert.ToInt32(tranAmount);
            if (transactionAmount == 0)
            {
                Console.WriteLine("You must enter a positive amount.");
                continue;
            }

            Transaction transaction;
            switch (transactionType)
            {
                case (int)TransactionType.Deposit:
                    transaction = new TellerConsole.Transactions.Deposit();
                    break;
                case (int)TransactionType.Withdrawal:
                    transaction = new Withdraw();
                    break;
                default:
                    Console.WriteLine($"Invalid transaction type - {transactionType}.");
                    continue;
            }

            transaction.ProcessTransaction(member, transactionAmount);

            MemberDB.UpdateMember(member);

            Console.WriteLine($"The Beginning balance was {beginningBalance} and the ending balance is {member.Balance}.");
        }
        while (string.IsNullOrWhiteSpace(accountType.IsValidAccountType()) && isValidTransaction); 
    }

    catch (Exception ex)
    {
        Console.WriteLine($"{ex.Message}");
        continue;
    }
}
















