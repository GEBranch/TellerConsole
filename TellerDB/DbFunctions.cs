using Microsoft.EntityFrameworkCore;

namespace TellerDB
{
    public static class DbFunctions
    {
        public static BankContext SetupDb()
        {
            using BankContext context = new BankContext();
            context.Database.Migrate();

            return context;
        }

        public static List<Member> GetAllMembers()
        {
            using var context = new BankContext();
            List<Member> members = context.Members.ToList();
            return members;
        }

        public static Member GetMemberByAccountId(int accountNumber)
        {
            using var context = new BankContext();
            Member? member = context.Members
                .FirstOrDefault(m => m.AccountNumber == accountNumber);
            if (member == null)
            {
                throw new Exception($"Member with Account Number '{accountNumber}' was not found.");
            }
            return member;
        }

        public static Member UpdateMember(Member member)
        {
            using var context = new BankContext();
            context.Members.Update(member);
            context.SaveChanges();
            return member;
        }

        public static Account UpdateAccount(Account account)
        {
            using var context = new BankContext();
            context.Accounts.Update(account);
            context.SaveChanges();
            return account;
        }
    }
}
