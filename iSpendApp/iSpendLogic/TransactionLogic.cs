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
    }
}
