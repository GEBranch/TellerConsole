using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TellerDomain;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("TellerConsoleLog.txt", rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddAutoMapper(cfg => { }, typeof(Program).Assembly);

Functions.GetMapper();
Functions.SetupDb();
             
bool runProcess = true;
while (runProcess == true)
{
    try
    {
        decimal transactionAmount = 0m;
        Transaction transaction = new Transaction();

        runProcess = true;

        Console.WriteLine("_____________________________");
        Console.WriteLine("Processing new transaction...");
        Console.WriteLine("_____________________________");

        do
        {
            Console.Write("Enter the Account Number or Q for quit: ");
            var acctNumber = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(acctNumber))
            {
                runProcess = false;
                continue;
            }
            if (acctNumber.ToUpper() == "Q")
            {
                runProcess = false;
                continue;
            }
            transaction.AccountNumber = Convert.ToInt32(acctNumber);

            Console.Write("Enter the Account Type: (Checking = 1, Savings = 2): ");
            transaction.AccountType = (AccountType)Int32.Parse(Console.ReadLine());
            if (transaction.AccountType == AccountType.Undefined)
            {
                runProcess = false;
                continue;
            }
            if (!transaction.AccountType.IsValidAccountType())
            {
                Console.WriteLine("Invalid account type. Please enter 1 for Checking or 2 for Savings.");
                continue;
            }

            try
            {
                transaction = Functions.GetMemberByAccountNumberAndType(transaction);
            }
            catch (Exception ex)
            {
                            
                Log.Error(ex, ex.Message);
                continue;
            }
                        
            if (transaction.AccountDTO == null || transaction.AccountDTO?.AccountType == AccountType.Undefined)
            {
                Log.Error($"Account number {transaction.AccountNumber} with account type {transaction.AccountType} was not found.");
                continue;
            }

            transaction.OriginalAccountBalance = transaction.AccountDTO.Balance;
            transaction.AccountType = transaction.AccountType;


            Console.Write("Enter the Transaction Type (Deposit = 1, Withdrawal = 2): ");
            transaction.TransactionType = (TransactionType)Int32.Parse(Console.ReadLine());

            if (!transaction.TransactionType.IsValidTransactionType())
            {
                Console.WriteLine($"{transaction.TransactionType} is not a valid transaction type");
                continue;
            }

            Console.Write("Enter the Amount: ");
            var tranAmount = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(tranAmount))
            {
                runProcess = false;
                continue;
            }
            transaction.AmountToProcess = Convert.ToDecimal(tranAmount);
            if (transaction.AmountToProcess <= 0)
            {
                Log.Information($"Account: {transaction.MemberDTO?.AccountNumber} - Invalid deposit amount: <= zero: {transaction.AmountToProcess}");
                Console.WriteLine("Deposit amount must be greater than zero.");
                continue;
            }

            Transaction actionObject;
            try
            {
                switch (transaction.TransactionType)
                {
                    case TransactionType.Deposit:
                        actionObject = new Deposit();
                        break;
                    case TransactionType.Withdrawal:
                        actionObject = new Withdraw();
                        break;
                    default:
                        Console.WriteLine($"Invalid transaction type - {transaction.TransactionType}.");
                        continue;
                }

                transaction = actionObject.ProcessTransaction(transaction);
                            
            } catch (InvalidDataException ex)
            {
                Log.Error(ex.Message);
                Console.WriteLine(ex.Message);
                continue;
            }
 
            Console.WriteLine($"The Beginning balance was {transaction.OriginalAccountBalance} and the ending balance is {transaction.AccountDTO.Balance}.");
        }
        while (runProcess);

        Log.CloseAndFlush();
    }

    catch (Exception ex)
    {
        Console.WriteLine($"{ex.Message}");
        continue;
    }

}
