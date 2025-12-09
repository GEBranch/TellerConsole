using TellerConsole.Database;
using TellerConsole.Extensions;
using TellerConsole.Members;

namespace TellerConsole.Modules
{
    public class Member : Account
    {
        public static Member GetMemberByAccountNumberAndType(int id, int accountType)
        {
            if (!string.IsNullOrWhiteSpace(accountType.IsValidAccountType()))
            {
                return new Member();
            }

            var member = MemberDB.GetAllMembers().FirstOrDefault(m => m.AccountNumber == id && m.Type == (AccountType)accountType)
                        ?? new Member();
            if (member.AccountNumber == 0)
            {
                Console.WriteLine($"Account not found.");
                return new Member();
            }
            return member;
        }
    }
}
