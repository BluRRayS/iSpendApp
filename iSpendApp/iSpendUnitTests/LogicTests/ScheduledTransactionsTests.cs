using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using iSpendDAL;
using iSpendDAL.Transaction;
using iSpendInterfaces;
using iSpendLogic;
using iSpendUnitTests.LogicTests.Models;
using Xunit;
using Xunit.Sdk;

namespace iSpendUnitTests
{
    public class ScheduledTransactionsTests
    {
        private readonly TransactionLogic _transactionLogic =
            new TransactionLogic(new TransactionContext(new DatabaseConnection("")));

        [Fact]
        public void ExecutesScheduledTransactionsWhichDayIsLargeThanMonthDays()
        {
            //Arrange
            const string string31 = "31-02-2001";
            var date31 = DateTime.ParseExact(string31,"dd-MM-yyyy", CultureInfo.CurrentCulture);
            const string string28 = "28-02-2001";
            var date28 = DateTime.ParseExact(string28, "dd-MM-yyyy",CultureInfo.CurrentCulture);
            var transactionOn31 = new Transaction(){AccountId = 0, Category = "",IconId = 0, TimeOfTransaction = date31, TransactionAmount = 100,TransactionId = 0,TransactionName = "31feb"};
            var transactionOn28 = new Transaction() { AccountId = 0, Category = "", IconId = 0, TimeOfTransaction = date28, TransactionAmount = 100, TransactionId = 0, TransactionName = "28feb" };
            var days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            var count = 0;
            var transactions = new List<ITransaction> {transactionOn31, transactionOn28};
            var transactionsToExecute = transactions = transactions.Where(transaction => transaction.TimeOfTransaction.Day == DateTime.Now.Day).ToList();
            //Act

            if (transactions.Any(t => t.TimeOfTransaction.Day > days && DateTime.Now.Day == days))
            {
                transactionsToExecute = transactions = transactions.Where(t => t.TimeOfTransaction.Day > days).ToList();
            }

            foreach (var transaction in transactionsToExecute)
            {
                count++;
            }

            //Assert
            Assert.Equal(2, count);
        }
    }
}
