using System;
using System.Collections.Generic;
using System.Text;
using iSpendInterfaces;

namespace iSpendDAL.ContextInterfaces
{
    public interface IBillContext
    {
        void AddBill(IBill newBill,int userId);

        void RemoveBill(IBill billToRemove);

        void UpdateBill(IBill billToUpdate);

        IEnumerable<IBill> GetBillsByUsername(string username);

        IBill GetBillById(int billId);

        IEnumerable<ITransaction> GetBillTransactions(int billId);
    }
}
