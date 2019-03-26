using System;
using System.Collections.Generic;
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
           return Repository.GetBillTransactions(id);
        }

        public void CreateTransaction(ITransaction transaction)
        {
            Repository.CreateTransaction(transaction);
        }
    }
}
