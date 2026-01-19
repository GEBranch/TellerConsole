using AutoMapper;
using Serilog;
using TellerDB;

namespace TellerDomain
{
    public class Deposit : Transaction
    {
        IMapper _mapper;

        public Deposit()
        {
            _mapper = Functions.GetMapper();
        }

        public override Transaction ProcessTransaction(Transaction transaction)
        {
            if (transaction.AccountDTO == null)
            {
                Log.Information($"Account: {transaction.AccountNumber} - No active account found for deposit.");
                throw new InvalidOperationException($"Account: {transaction.MemberDTO?.AccountNumber} - No active account found for deposit.");
            }
            Account account;
            account = _mapper.Map<Account>(transaction.AccountDTO);
            account.Balance += transaction.AmountToProcess;
            account = DbFunctions.UpdateAccount(account);
            transaction.AccountDTO = _mapper.Map<AccountDTO>(account);

            return transaction;
        }
    }
}
 