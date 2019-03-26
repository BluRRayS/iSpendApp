using System;
using System.Collections.Generic;
using System.Text;
using iSpendInterfaces;

namespace iSpendDAL.ContextInterfaces
{
    public interface IBillContext
    {
        void AddBill(IBill newBill,int userId);

        void RemoveBill(int billId);

        void UpdateBill(int id, string name, int iconId);

        IEnumerable<IBill> GetBillsByUsername(string username);

        IBill GetBillById(int billId);

        IEnumerable<ITransaction> GetBillTransactions(int billId);

        decimal GetTotalBalance(int billId);
        void UpdateBillBalance(int billId, decimal amount);

    }
}
