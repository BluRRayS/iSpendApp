using System;
using System.Collections.Generic;
using System.Text;
using iSpendDAL.ContextInterfaces;
using iSpendInterfaces;

namespace iSpendUnitTests.MockContexts
{
    internal class AccountContextMock:IAccountContext
    {
        public void AddUser(IUser account)
        {
            throw new NotImplementedException();
        }

        public bool CheckIfUserNameIsTaken(string username)
        {
            throw new NotImplementedException();
        }

        public IUser GetAccountByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public void AddAccount(IAccount newAccount, int userId)
        {
            throw new NotImplementedException();
        }

        public void RemoveAccount(int billId)
        {
            throw new NotImplementedException();
        }

        public void UpdateAccount(int id, string name, int iconId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAccount> GetAccountsByUsername(string username)
        {
            throw new NotImplementedException();
        }

        IAccount IAccountContext.GetAccountById(int billId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ITransaction> GetAccountTransactions(int billId)
        {
            throw new NotImplementedException();
        }

        public decimal GetTotalBalance(int billId)
        {
            throw new NotImplementedException();
        }

        public void AddReservation(IReservation reservation)
        {
            throw new NotImplementedException();
        }

        public void UpdateAccountBalance(int billId, decimal amount)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IUser> GetAccountUsers(int billId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IReservation> GetReservations(int accountId)
        {
            throw new NotImplementedException();
        }

        public IUser GetAccountById(int userId)
        {
            throw new NotImplementedException();
        }

        public bool CheckCredentials(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void UpdateUserDetails(IUser account)
        {
            throw new NotImplementedException();
        }
    }
}
