using System.Collections;
using TellerDomain;

namespace TellerConsole.UnitTests
{
    [TestClass]
    public class FunctionsTests
    {
        [TestMethod]
        public void ValidAccountTypeShouldReturnAsValid()
        {
            Functions functions = new Functions();

            Transaction transaction = new Transaction { AccountType = AccountType.Checking };
            var result = functions.ValidateAccountType(transaction, "1");

            Assert.IsFalse(transaction.QuitProgram);
            Assert.IsTrue(result);
            Assert.AreEqual(AccountType.Checking, transaction.AccountType);
        }

        [TestMethod]
        public void InValidAccountTypeShouldReturnAsInvalid()
        {
            Functions functions = new Functions();

            Transaction transaction = new Transaction { AccountType = AccountType.Checking };
            var result = functions.ValidateAccountType(transaction, "5");

            Assert.IsFalse(transaction.QuitProgram);
            Assert.IsFalse(result);
            Assert.AreEqual(AccountType.Undefined, transaction.AccountType);
        }

        [TestMethod]
        public void AccountTypeIfQEnteredQShouldReturnQuitProgramAsTrue()
        {
            Functions functions = new Functions();

            Transaction transaction = new Transaction { AccountType = AccountType.Checking };
            var result = functions.ValidateAccountType(transaction, "Q");

            Assert.IsTrue(transaction.QuitProgram);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void InvalidAmountEnteredShouldReturnZeroAmount()
        {
            Functions functions = new Functions();

            Transaction transaction = new Transaction { AccountType = AccountType.Checking };
            var result = functions.ValidateTransactionAmount(transaction, "w");

            Assert.IsFalse(transaction.QuitProgram);
            Assert.IsFalse(result);
            Assert.AreEqual(0, transaction.AmountToProcess);
        }
    }
}
