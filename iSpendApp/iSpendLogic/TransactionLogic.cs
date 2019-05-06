using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using iSpendDAL.ContextInterfaces;
using iSpendDAL.Transaction;
using iSpendInterfaces;
using iSpendLogic.Models;

namespace iSpendLogic
{
    public class TransactionLogic
    {
        private TransactionRepository Repository { get; }

        public TransactionLogic(ITransactionContext context)
        {
            Repository = new TransactionRepository(context);
        }

        public IEnumerable<ITransaction> GetBillTransactions(int id)
        {
            var transactions = Repository.GetBillTransactions(id);
            return transactions.OrderByDescending(trans => trans.TimeOfTransaction);
        }

        public void CreateTransaction(ITransaction transaction)
        {
            transaction.TimeOfTransaction = DateTime.Now;
            Repository.CreateTransaction(transaction);
        }

        public void DeleteTransaction(int id, int billId)
        {
            Repository.DeleteTransaction(id, billId);
        }

        public void UpdateTransaction(int id, ITransaction transaction)
        {
            Repository.UpdateTransaction(id,transaction);
        }

        public ITransaction GetTransactionById(int id,int billId)
        {
            return Repository.GetTransactionById(id,billId);
        }

        public IEnumerable<ICategory> GetCategories()
        {
            return Repository.GetCategories();
        }

        public void ExecuteScheduledTransactions(DateTime executingTime)
        {
            //Todo: Check interval maybe each week instead of 1 time each month
            //Todo: Check if month days higher than month max days works!
            var transactions = GetAllScheduledTransactions();
            var days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            var transactionsToExecute = transactions = transactions.Where(transaction => transaction.TimeOfTransaction.Day == executingTime.Day);
            if (transactions.Any(t => t.TimeOfTransaction.Day > days && executingTime.Day == days))
            {
                transactionsToExecute =transactions = transactions.Where(t => t.TimeOfTransaction.Day > days);
            }

            foreach (var transaction in transactionsToExecute)
            {
                Repository.CreateTransaction(transaction);
            }
        }

        public IEnumerable<ITransaction> GetAllScheduledTransactions()
        {
            return Repository.GetAllScheduledTransactions();
        }

        public IEnumerable<ITransaction> GetAccountScheduledTransactions(int id)
        {
            return Repository.GetAccountScheduledTransactions(id);
        }

        public void EditScheduledTransaction(ITransaction transaction)
        {
            Repository.EditScheduledTransaction(transaction);
        }

        public void RemoveScheduledTransaction(int id)
        {
            Repository.RemoveScheduledTransaction(id);
        }

        public void AddScheduledTransaction(ITransaction transaction)
        {
            Repository.AddScheduledTransaction(transaction);
        }

        public ITransaction GetScheduledTransactionById(int id)
        {
            return Repository.GetScheduledTransactionById(id);
        }

        public void ImportTransactions(IEnumerable<ITransactionsFile> uploads,int accountId)
        {
            var transactions = uploads.Select(item => new Models.Transaction(0, item.Name, item.Amount, "Upload", 0, item.TimeOfTransaction, accountId)).ToList();
            Repository.ImportTransactions(transactions);
        }
    }
}
