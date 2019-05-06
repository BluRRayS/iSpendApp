using System;
using System.Collections.Generic;
using System.Text;
using iSpendInterfaces;

namespace iSpendDAL.ContextInterfaces
{
    public interface ITransactionContext
    {
        IEnumerable<ITransaction> GetBillTransactions(int id);
        void CreateTransaction(ITransaction transaction);
        void DeleteTransaction(int id, int accountId);
        void UpdateTransaction(int id, ITransaction transaction);
        ITransaction GetTransactionById(int id,int accountId);
        IEnumerable<ICategory> GetCategories();
        IEnumerable<ITransaction> GetAccountScheduledTransactions(int id);
        IEnumerable<ITransaction> GetAllScheduledTransactions();
        void RemoveScheduledTransaction(int id);
        void AddScheduledTransaction(ITransaction transaction);
        void EditScheduledTransaction(ITransaction transaction);
        void GetTotalBalance(int billId);
        void UpdateBillBalance(int billId, decimal amount);
        ITransaction GetScheduledTransactionById(int id);
        void ImportTransactions(IEnumerable<ITransaction> transactions);
    }
}
