using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iSpendDAL.ContextInterfaces;
using iSpendDAL.Transaction;
using iSpendInterfaces;

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
            var transactions = GetAllScheduledTransactions();
            transactions = transactions.Where(transaction => transaction.TimeOfTransaction.Day == executingTime.Day);
            foreach (var transaction in transactions)
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

        public void RemoveScheduledTransaction(int id)
        {
            Repository.RemoveScheduledTransaction(id);
        }

        public void AddScheduledTransaction(ITransaction transaction)
        {
            Repository.AddScheduledTransaction(transaction);
        }
    }
}
