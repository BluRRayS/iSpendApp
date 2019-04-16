using System;
using System.Collections.Generic;
using System.Text;
using iSpendInterfaces;

namespace iSpendDAL.ContextInterfaces
{
    public interface IAccountContext
    {
        void AddAccount(IAccount newAccount,int userId);

        void RemoveAccount(int billId);

        void UpdateAccount(int id, string name, int iconId);

        IEnumerable<IAccount> GetAccountsByUsername(string username);

        IAccount GetAccountById(int billId);

        IEnumerable<ITransaction> GetAccountTransactions(int billId);

        decimal GetTotalBalance(int billId);

        void AddReservation(IReservation reservation);

        void UpdateAccountBalance(int billId, decimal amount);

        IEnumerable<IUser> GetAccountUsers(int billId);
        IEnumerable<IReservation> GetReservations(int accountId);
    }
}
