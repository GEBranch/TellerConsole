using Autofac;
using Autofac.Core;
using AutoMapper;
using Serilog;
using Serilog.Extensions.Logging;
using TellerDomain;

var _factory = new SerilogLoggerFactory();
var _functions = new Functions();

bool runProcess = true;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("TellerConsoleLog.txt", rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var builder = new ContainerBuilder();
builder.RegisterInstance(_functions.GetMapper()).As<IMapper>().SingleInstance();
builder.RegisterType<Deposit>();
builder.RegisterType<Withdraw>();

builder.RegisterAssemblyTypes(typeof(Program).Assembly)
       .Where(t => t.IsSubclassOf(typeof(Transaction)))
       .AsSelf();

var container = builder.Build();

Deposit? deposit = null;
Withdraw? withdraw = null;
IMapper? mapper;
using var scope = container.BeginLifetimeScope();
try
{
    var result = scope.TryResolve<Deposit>(out deposit);
    result = scope.TryResolve<Withdraw>(out withdraw);
    result = scope.TryResolve<IMapper>(out mapper);
} catch (DependencyResolutionException ex)
{
    Log.Error(ex, $"Dependency resolution failed: {ex.Message}");
    Console.WriteLine("Application configuration error. Please contact support.");
    runProcess = false;
}
_functions.SetupDb();
             
while (runProcess == true)
{
    try
    {
        var transaction = new Transaction();

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

            if (!transaction.AccountType.IsValidAccountType())
            {
                Log.Error($"Invalid account type {transaction.AccountType}. Please enter 1 for Checking or 2 for Savings.");
                Console.WriteLine($"Invalid account type {transaction.AccountType}. Please enter 1 for Checking or 2 for Savings.");
                runProcess = false;
                continue;
            }

            try
            {
                transaction = _functions.GetMemberByAccountNumberAndType(transaction);
            }
            catch (InvalidDataException ex)
            {
                Log.Error(ex.Message);
                Console.WriteLine(ex.Message);
                continue;
            }

            if (transaction.AccountDTO == null)
            {
                Log.Error($"Account: {transaction.MemberDTO?.AccountNumber} - No active account found for transaction.");
                Console.WriteLine($"Account: {transaction.MemberDTO?.AccountNumber} - No active account found for transaction.");
                continue;
            }

            transaction.OriginalAccountBalance = transaction.AccountDTO.Balance;

            Console.Write("Enter the Transaction Type (Deposit = 1, Withdrawal = 2): ");
            transaction.TransactionType = (TransactionType)Int32.Parse(Console.ReadLine());

            if (!transaction.TransactionType.IsValidTransactionType())
            {
                Console.WriteLine($"{transaction.TransactionType} is not a valid transaction type");
                Log.Error($"{transaction.TransactionType} is not a valid transaction type");
                continue;
            }

            Console.Write("Enter the Amount: ");
            var tranAmount = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(tranAmount))
            {
                Log.Error($"Account: {transaction.MemberDTO?.AccountNumber} - Invalid deposit amount: <= zero: {tranAmount}");
                runProcess = false;
                continue;
            }
            transaction.AmountToProcess = Convert.ToDecimal(tranAmount);
            if (transaction.AmountToProcess <= 0)
            {
                Log.Error($"Account: {transaction.MemberDTO?.AccountNumber} - Invalid deposit amount: <= zero: {transaction.AmountToProcess}");
                Console.WriteLine("Deposit amount must be greater than zero.");
                continue;
            }

            Transaction actionObject;
            try
            {
                switch (transaction.TransactionType)
                {
                    case TransactionType.Deposit:
                        actionObject = deposit;
                        break;
                    case TransactionType.Withdrawal:
                        actionObject = withdraw;
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
