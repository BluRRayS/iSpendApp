using System;
using System.Collections.Generic;
using System.Text;
using iSpendDAL;
using iSpendDAL.Bill;
using iSpendDAL.ContextInterfaces;
using iSpendInterfaces;

namespace iSpendLogic
{
    public class BillLogic
    {
        private BillRepository Repository { get; }

        public BillLogic(IBillContext context)
        {
            Repository = new BillRepository(context);
        }

        public void AddBill(IBill newBill , int userId)
        {
            Repository.AddBill(newBill, userId);
        }

        public IEnumerable<IBill> GetUserBills(string username)
        {
           return Repository.GetUserBills(username);
        }

        public IBill GetBillById(int id)
        {
            return Repository.GetBillById(id);
        }

        public void UpdateBill(int id,string name, int iconId)
        {
            Repository.UpdateBill(id,name, iconId);
        }

        public void RemoveBill(int id)
        {
            Repository.RemoveBill(id);
        }

        public void RefreshBillBalance(int id)
        {
            Repository.RefreshBillBalance(id);
        }
    }
}
