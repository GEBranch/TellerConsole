    using AutoMapper;
    using TellerDB;

namespace TellerDomain
{
    public class Withdraw : Transaction
    {
        IMapper _mapper;

        public Withdraw()
        {
            _mapper = Functions.GetMapper();
        }

        public override Transaction ProcessTransaction(Transaction transaction)
        { 
            var activeAccountDTO = transaction.MemberDTO.Accounts.FirstOrDefault(a => a.IsActive && a.AccountType == transaction.AccountType
                                                                                && a.MemberId == transaction.MemberDTO.MemberId);
            if (activeAccountDTO == null) {
                throw new InvalidDataException($"transaction.AccountType not found for Member {transaction.MemberDTO.AccountNumber}");
            }
            if (transaction.AmountToProcess > activeAccountDTO.Balance)
            {
                throw new InvalidDataException($"Insufficient Funds - Current Balance: {transaction.OriginalAccountBalance}");
            }

            Account account = _mapper.Map<Account>(activeAccountDTO);

            account.Balance -= transaction.AmountToProcess;

            account = DbFunctions.UpdateAccount(account);

            transaction.AccountDTO  = _mapper.Map<AccountDTO>(account);

            return transaction;
        }
    }
}
