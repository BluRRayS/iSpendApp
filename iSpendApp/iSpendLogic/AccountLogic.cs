using System;
using System.Collections.Generic;
using System.Text;
using iSpendDAL;
using iSpendDAL.Account;
using iSpendDAL.ContextInterfaces;
using iSpendInterfaces;

namespace iSpendLogic
{
    public class AccountLogic
    {
        private AccountRepository Repository { get; }

        public AccountLogic(IAccountContext context)
        {
            Repository = new AccountRepository(context);
        }

        public void AddAccount(IAccount newAccount, int userId)
        {
            Repository.AddAccount(newAccount, userId);
        }

        public IEnumerable<IAccount> GetUserAccounts(string username)
        {
            var accounts = new List<IAccount>();
            foreach (var account in Repository.GetUserBills(username))
            {
                account.Reservations = Repository.GetReservations(account.AccountId);
                accounts.Add(account);
            }
            return accounts;
        }

        public IAccount GetAccountById(int id)
        {
            var account = Repository.GetBillById(id);
            account.Reservations = Repository.GetReservations(id);
            return account;
        }

        public void UpdateAccount(int id, string name, int iconId)
        {
            Repository.UpdateAccount(id, name, iconId);
        }

        public void RemoveAccount(int id)
        {
            Repository.RemoveBill(id);
        }

        public void RefreshAccountBalance(int id)
        {
            Repository.RefreshBillBalance(id);
        }

        public IEnumerable<IUser> GetAccountUsers(int billId)
        {
            return Repository.GetBillUsers(billId);
        }

        public IEnumerable<IReservation> GetAccountReservations(int accountId)
        {
            return Repository.GetReservations(accountId);
        }

        public IEnumerable<IAccount> GetAccountsByUsername(string username)
        {
            return Repository.GetUserBills(username);
        }
    }
}
