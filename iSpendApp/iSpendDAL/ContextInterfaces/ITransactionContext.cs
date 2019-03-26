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
        void DeleteTransaction(int id);
        void UpdateTransaction(int id, ITransaction transaction);
        ITransaction GetTransactionById(int id);

    }
}
