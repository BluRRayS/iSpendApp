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

        public void AddBill(IBill newBill, int userId)
        {
            Repository.AddBill(newBill, userId);
        }

        public IEnumerable<IBill> GetUserBills(string username)
        {
            var accounts = new List<IBill>();
            foreach (var account in Repository.GetUserBills(username))
            {
                account.Reservations = Repository.GetReservations(account.BillId);
                accounts.Add(account);
            }
            return accounts;
        }

        public IBill GetBillById(int id)
        {
            var account = Repository.GetBillById(id);
            account.Reservations = Repository.GetReservations(id);
            return account;
        }

        public void UpdateBill(int id, string name, int iconId)
        {
            Repository.UpdateBill(id, name, iconId);
        }

        public void RemoveBill(int id)
        {
            Repository.RemoveBill(id);
        }

        public void RefreshBillBalance(int id)
        {
            Repository.RefreshBillBalance(id);
        }

        public IEnumerable<IUser> GetBillUsers(int billId)
        {
            return Repository.GetBillUsers(billId);
        }

        public IEnumerable<IReservation> GetAccountReservations(int accountId)
        {
            return Repository.GetReservations(accountId);
        }

        public IEnumerable<IBill> GetBillsByUsername(string username)
        {
            return Repository.GetUserBills(username);
        }
    }
}
