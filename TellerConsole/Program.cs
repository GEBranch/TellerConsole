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
var container = _functions.RegisterDependencies(builder);

Deposit? deposit = null;
Withdraw? withdraw = null;
IMapper mapper;
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

        Console.WriteLine("_____________________________");
        Console.WriteLine("Processing new transaction...");
        Console.WriteLine("_____________________________");

        do
        {
            var success = _functions.GetAccountNumber(transaction);
            if (transaction.QuitProgram)
            {
                runProcess = false;
                break;
            }
            if (!success)
            {
                Log.Error($"Unable to get account number {transaction.AccountNumber}");
                Console.WriteLine($"Unable to get account number {transaction.AccountNumber}");
                continue;
            }

            success = _functions.GetAccountType(transaction);
            if (transaction.QuitProgram)
            {
                runProcess = false;
                break;
            }
            if (!success) {
                Log.Error($"Unable to get account type for account Type {transaction.AccountType}");
                continue;
            }

            try
            {
                success = _functions.GetMemberByAccountNumberAndType(transaction);
            }
            catch (InvalidDataException ex)
            {
                Log.Error(ex.Message);
                Console.WriteLine(ex.Message);
                continue;
            }

            success = _functions.GetTransactionType(transaction);
            if (transaction.QuitProgram)
            {
                runProcess = false;
                break;
            }
            if (!success)
            {
                Log.Error($"Unable to get transaction type for transaction Type {transaction.TransactionType}");
                continue;
            }

            success = _functions.GetTransactionAmount(transaction);
            if (transaction.QuitProgram)
            {
                runProcess = false;
                break;
            }
            if (!success)
            {
                Log.Error($"Unable to get transaction amount for the entered value: {transaction.AmountToProcess}");
                Console.WriteLine($"Unable to get transaction amount for the entered value: {transaction.AmountToProcess}");
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
