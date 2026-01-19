using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Extensions.Logging;
using TellerDB;

namespace TellerDomain
{

    public static class Functions
    {
        private static Mapper _mapper;
        private static readonly SerilogLoggerFactory _factory = new SerilogLoggerFactory();

        public static IMapper SetupMappings()
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





        public static void SetupDb()
        {
            using BankContext context = TellerDB.DbFunctions.SetupDb();
        }

        public static IMapper GetMapper()
        {
            if (_mapper == null)
            {
                _mapper = SetupMappings() as Mapper;
            }
            return _mapper;
        }

        public static List<MemberDTO> GetAllMembers()
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

        public static Transaction GetMemberByAccountNumber(Transaction transaction)
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

        public static Transaction GetMemberByAccountNumberAndType(Transaction transaction)
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
                Console.WriteLine($"Account number {transaction.AccountNumber} with account type {transaction.AccountType} was not found.");
                throw new Exception($"Account number { transaction.AccountNumber } with account type { transaction.AccountType} was not found.");
            }

            transaction.MemberDTO = _mapper.Map<MemberDTO>(member);
            transaction.AccountDTO = _mapper.Map<AccountDTO>(account);

            return transaction;
        }
    }
}
