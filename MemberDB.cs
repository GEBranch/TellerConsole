namespace TellerConsole
{

    // Simple database class (as a list). It will be replaced by an
    // actual database in a later iteration of this program.
    
    public static class MemberDB
    {
        static IEnumerable<Member> allAccounts = new List<Member>();

        public static IEnumerable<Member> GetAllMembers()
        {
            return allAccounts;
        }

        public static Member UpdateMember(Member member)
        {
            // Object Oriented program logic. For this example, just calling this routine will update the Member
            // as it is passed to this empty routine byref with the updated values. The return value is 
            // completely unnecessary. I just put it there to make the program just a little more readable.
            return member;
        }

        public static void InitializeDB()
        {
            if (!allAccounts.Any())
            {
                allAccounts =
                [
                    new() { AccountNumber = 1, Balance = 100.00m, Type = AccountType.Checking },
                    new() { AccountNumber = 1, Balance = 50.00m, Type = AccountType.Savings },
                    new() { AccountNumber = 2, Balance = 10.00m, Type = AccountType.Checking },
                ];
            }
        }
    }
}
