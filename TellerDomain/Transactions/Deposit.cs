using AutoMapper;
using Serilog;
using TellerDB;

namespace TellerDomain
{
    public class Deposit : Transaction
    {
        IMapper _mapper;

        public Deposit(IMapper mapper)
        {
            _mapper = mapper;
        }

        public override Transaction ProcessTransaction(Transaction transaction)
        {
            if (transaction.AccountDTO == null)
            {
                Log.Information($"Account: {transaction.AccountNumber} - No active account found for deposit.");
                throw new InvalidOperationException($"Account: {transaction.MemberDTO?.AccountNumber} - No active account found for deposit.");
            }

            var account = _mapper.Map<Account>(transaction.AccountDTO);
            account.Balance += transaction.AmountToProcess;
            account = DbFunctions.UpdateAccount(account);
            transaction.AccountDTO = _mapper.Map<AccountDTO>(account);

            return transaction;
        }
    }
}
 