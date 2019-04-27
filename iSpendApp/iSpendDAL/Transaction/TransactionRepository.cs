using System;
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

         public void DeleteTransaction(int id, int billId)
         {
             _context.DeleteTransaction(id, billId);
         }

         public void UpdateTransaction(int id, ITransaction transaction)
         {
             _context.UpdateTransaction(id,transaction);
         }

         public ITransaction GetTransactionById(int id,int billId)
         {
             return _context.GetTransactionById(id,billId);
         }

         public IEnumerable<ICategory> GetCategories()
         {
             return _context.GetCategories();
         }

         public IEnumerable<ITransaction> GetAccountScheduledTransactions(int id)
         {
             return _context.GetAccountScheduledTransactions(id);
         }

         public IEnumerable<ITransaction> GetAllScheduledTransactions()
         {
             return _context.GetAllScheduledTransactions();
         }

         public void RemoveScheduledTransaction(int id)
         {
             _context.RemoveScheduledTransaction(id);
         }

         public void AddScheduledTransaction(ITransaction transaction)
         {
             _context.AddScheduledTransaction(transaction);
         }

         public void EditScheduledTransaction(ITransaction transaction)
         {
             _context.EditScheduledTransaction(transaction);
         }

         public ITransaction GetScheduledTransactionById(int id)
         {
             return _context.GetScheduledTransactionById(id);
         }
    }
}
