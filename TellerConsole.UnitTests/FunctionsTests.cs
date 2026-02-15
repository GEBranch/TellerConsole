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
            Functions functions = new();

            Transaction transaction = new() { AccountType = AccountType.Checking };
            var result = functions.ValidateAccountType(transaction, "1");

            Assert.IsFalse(transaction.QuitProgram);
            Assert.IsTrue(result);
            Assert.AreEqual(AccountType.Checking, transaction.AccountType);
        }

        [TestMethod]
        public void InValidAccountTypeShouldReturnAsInvalid()
        {
            Functions functions = new();

            Transaction transaction = new() { AccountType = AccountType.Checking };
            var result = functions.ValidateAccountType(transaction, "5");

            Assert.IsFalse(transaction.QuitProgram);
            Assert.IsFalse(result);
            Assert.AreEqual(AccountType.Undefined, transaction.AccountType);
        }

        [TestMethod]
        public void AccountTypeIfQEnteredQShouldReturnQuitProgramAsTrue()
        {
            Functions functions = new();

            Transaction transaction = new() { AccountType = AccountType.Checking };
            var result = functions.ValidateAccountType(transaction, "Q");

            Assert.IsTrue(transaction.QuitProgram);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void InvalidAmountEnteredShouldReturnZeroAmount()
        {
            Functions functions = new();

            Transaction transaction = new() { AccountType = AccountType.Checking };
            var result = functions.ValidateTransactionAmount(transaction, "w");

            Assert.IsFalse(transaction.QuitProgram);
            Assert.IsFalse(result);
            Assert.AreEqual(0, transaction.AmountToProcess);
        }

        [TestMethod]
        public void ValidAmountEnteredShouldReturnSetAmountToProcess()
        {
            Functions functions = new();

            Transaction transaction = new() { AccountType = AccountType.Checking };
            var result = functions.ValidateTransactionAmount(transaction, "50");

            Assert.IsFalse(transaction.QuitProgram);
            Assert.IsFalse(result);
            Assert.AreEqual(50, transaction.AmountToProcess);
        }


        [TestMethod]
        public void ValidAmountEnteredShouldReturnNewBalance()
        {
            Functions functions = new();

            AccountDTO accountDTO = new() { Balance = 200 };
            Transaction transaction = new() { AccountType = AccountType.Checking, AccountDTO = accountDTO };
            var result = functions.ValidateTransactionAmount(transaction, "50");

            Assert.IsFalse(transaction.QuitProgram);
            Assert.IsFalse(result);
            Assert.AreEqual(250, transaction.AccountDTO.Balance);
        }
    }
}
