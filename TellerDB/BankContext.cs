using Microsoft.EntityFrameworkCore;
namespace TellerDB
{
    public class BankContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Address> Addresses { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(local);Database=TellerDB;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=true");
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasData(
                    new Account { AccountId = 1, AccountNumber = 1, MemberId = 1, AccountType = AccountType.Checking, Balance = 500.00m, IsActive = true },
                    new Account { AccountId = 2, AccountNumber = 1, MemberId = 1, AccountType = AccountType.Savings, Balance = 1000.00M, IsActive = true },
                    new Account { AccountId = 3, AccountNumber = 2, MemberId = 2, AccountType = AccountType.Checking, Balance = 5000.00M, IsActive = true },
                    new Account { AccountId = 4, AccountNumber = 3, MemberId = 3, AccountType = AccountType.Checking, Balance = 1500.00M, IsActive = true }
                );

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Member)
                .WithMany(m => m.Accounts);

            modelBuilder.Entity<Address>()
                .HasData(
                    new Address { AddressId = 1, Street1 = "123 Main St", City = "Indianapolis", State = "IN", ZipCode = 46201, AccountNumber = 1, IsActive = true },
                    new Address { AddressId = 2, Street1 = "456 Elm St", City = "New York", State = "IN", ZipCode = 46038, AccountNumber = 2, IsActive = true },
                    new Address { AddressId = 3, Street1 = "789 Oak St", City = "Los Angeles", State = "IN", ZipCode = 29572, AccountNumber = 3, IsActive = true }
                );

            modelBuilder.Entity<Member>()
                .HasData(
                    new Member { MemberId = 1, AccountNumber = 1, FirstName = "John", LastName = "Doe", IsActive = true, AddressId = 1 },
                    new Member { MemberId = 2, AccountNumber = 2, FirstName = "Jane", LastName = "Smith", IsActive = true, AddressId = 2 },
                    new Member { MemberId = 3, AccountNumber = 3, FirstName = "Super", LastName = "Man", IsActive = true, AddressId = 3 }
                );
            modelBuilder.Entity<Member>()
                .HasOne(m => m.Address);

            modelBuilder.Entity<Member>()
                .HasMany(m => m.Accounts);
        }
    }
}
