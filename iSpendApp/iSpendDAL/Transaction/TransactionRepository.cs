using System.Collections.Generic;
using iSpendDAL.ContextInterfaces;
using iSpendInterfaces;

namespace iSpendDAL.Transaction
{
    public class TransactionRepository
    {
        private readonly ITransactionContext _context;

        public TransactionRepository(ITransactionContext context)
        {
            _context = context;
        }

         public IEnumerable<ITransaction> GetBillTransactions(int id)
         {
            return _context.GetBillTransactions(id);
         }

         public void CreateTransaction(ITransaction transaction)
         {
             _context.CreateTransaction(transaction);
         }
    }
}
