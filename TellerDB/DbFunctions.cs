using Microsoft.EntityFrameworkCore;

namespace TellerDB
{
    public class DbFunctions
    {
        public BankContext _context;

        public DbFunctions()
        {
            _context = new BankContext();
        }

        public BankContext SetupDb()
        {
            using BankContext context = _context;
            context.Database.Migrate();

            return context;
        }

        public List<Member> GetAllMembers()
        {
            List<Member> members = [.. _context.Members];
            return members;
        }

        public Member GetMemberByAccountId(int accountNumber)
        {
            Member? member = _context.Members
                .FirstOrDefault(m => m.AccountNumber == accountNumber);
            if (member == null)
            {
                throw new ArgumentNullException($"Member with Account Number '{accountNumber}' was not found.");
            }
            return member;
        }

        public Member UpdateMember(Member member)
        {
            _context.Members.Update(member);
            _context.SaveChanges();
            return member;
        }

        public Account UpdateAccount(Account account)
        {
            _context.Accounts.Update(account);
            _context.SaveChanges();
            return account;
        }
    }
}
