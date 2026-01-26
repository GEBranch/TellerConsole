using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Extensions.Logging;
using TellerDB;

namespace TellerDomain
{

    public  class Functions
    {
        private IMapper? _mapper = null;
        private readonly SerilogLoggerFactory _factory = new SerilogLoggerFactory();

        public IMapper SetupMappings()
        {
            var services = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Member, MemberDTO>();
                cfg.CreateMap<MemberDTO, Member>();
                cfg.CreateMap<Account, AccountDTO>();
                cfg.CreateMap<AccountDTO, Account>();
                cfg.CreateMap<Address, AddressDTO>();
                cfg.CreateMap<AddressDTO, Address>();
            }, _factory);

            return services.CreateMapper();
        }

        public void SetupDb()
        {
            using BankContext context = TellerDB.DbFunctions.SetupDb();
        }

        public IMapper GetMapper()
        {
            if (_mapper == null)
            {
                var mapperConfiguration = new MapperConfiguration(cfg =>
                 cfg.LicenseKey = @"eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxODAwOTIxNjAwIiwiaWF0IjoiMTc2OTQ1NzUyOSIsImFjY291bnRfaWQiOiIwMTliZmJlMWZkNTg3ZTE5OTRlN2FmMjA1MTQ0NDZlNCIsImN1c3RvbWVyX2lkIjoiY3RtXzAxa2Z4eTZjZHY4NmE4dmhyZzB4MGFwY2owIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.2I9Rb3N28c3VobiA5e7rTb7qInH6p60I_Nt2iF3yr3JapxMjx5S80kUmLfZHS7y-gempfQp0dzHSlV8dOt3bOdb5E1289QKfeTFdviQulh8LIKGSwmfUTZ0KVQL2v0yb7l2106mtQJWhr95A24L5_UYN3vlMfJhTBt3a5cxVpQYqNRxGHDxtWopB2p-EP2v9UwFpxE_UGMubvylbo5fwmtNgDLFI2qJFm5tqiAx_kCmukOTpv1yhyfpKR_WclAjRN3fX5J67tM8_c-HA0Bci9KTPhrOAeAkZ3UmVbn99q5OPYmnhPkOnaZf-r-4gRzudZUZenXMYE6ivp-PNVD0RMA"
                    , _factory);
                _mapper = SetupMappings() as Mapper;
            }
            return _mapper;
        }

        public List<MemberDTO> GetAllMembers()
        {
            using BankContext context = new BankContext();
            List<Member> members = context.Members.Where(m => m.IsActive).ToList();
            List<MemberDTO> memberDTOs = new List<MemberDTO>();
            foreach (var member in members)
            {
                MemberDTO memberDTO = _mapper.Map<MemberDTO>(members);
                memberDTOs.Add(memberDTO);
            }
            return memberDTOs;
        }

        public Transaction GetMemberByAccountNumber(Transaction transaction)
        {
            using BankContext context = new BankContext();
            var member = context.Members
                .FirstOrDefault(m => m.IsActive && m.AccountNumber == transaction.AccountNumber);
            if (member == null)
            {
                Log.Error($"Member {transaction.AccountNumber} was not found");
                return new Transaction();
            }

            transaction.MemberDTO = _mapper.Map<MemberDTO>(member);

            return transaction;
        }

        public Transaction GetMemberByAccountNumberAndType(Transaction transaction)
        {
            using BankContext context = new BankContext();

            TellerDB.AccountType acctType = (TellerDB.AccountType)transaction.AccountType;
            var member = context.Members
                .Include(a => a.Accounts)
                .Include(a => a.Address)
                .FirstOrDefault(m => m.IsActive && m.AccountNumber == transaction.AccountNumber);
            var account = member?.Accounts?.FirstOrDefault(a => a.IsActive && a.AccountType == acctType);
            if (account == null)
            {
                throw new InvalidDataException($"Account number { transaction.AccountNumber } with account type { transaction.AccountType} was not found.");
            }

            transaction.MemberDTO = _mapper.Map<MemberDTO>(member);
            transaction.AccountDTO = _mapper.Map<AccountDTO>(account);

            return transaction;
        }
    }
}
