using System.Collections.Generic;
using iSpendDAL.ContextInterfaces;
using iSpendInterfaces;

namespace iSpendDAL.Account
{
    public class AccountRepository
    {
        private readonly IAccountContext _context;

        public AccountRepository(IAccountContext context)
        {
            _context = context;
        }

        public void AddAccount(IAccount newBill, int userId)
        {
            _context.AddAccount(newBill, userId);
        }

        public IEnumerable<IAccount> GetUserBills(string username)
        {
           return _context.GetAccountsByUsername(username);
        }

        public IAccount GetBillById(int id)
        {
            return _context.GetAccountById(id);
        }

        public void UpdateAccount(int id,string name, int iconId)
        {
            _context.UpdateAccount(id,name,iconId);
        }

        public void RemoveBill(int id)
        {
            _context.RemoveAccount(id);
        }

        public void RefreshBillBalance(int id)
        {
            _context.GetTotalBalance(id);
        }

        public IEnumerable<IUser> GetBillUsers(int billId)
        {
            return _context.GetAccountUsers(billId);
        }

        public IEnumerable<IReservation> GetReservations(int accountId)
        {
            return _context.GetReservations(accountId);
        }
    }
}
