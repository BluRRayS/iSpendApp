using System;
using iSpendDAL;
using iSpendDAL.ContextInterfaces;
using iSpendInterfaces;
using iSpendLogic.Helpers;
using iSpendLogic.Models;

namespace iSpendLogic
{
    public class AccountLogic
    {
        private AccountRepository Repository { get; }

        public AccountLogic(IAccountContext context)
        {
            Repository = new AccountRepository(context);
        }

        public void AddUser(string username, string password, string email)
        {
            var encryption = new Encrypt();
            password = encryption.Hash(password);
            var newUser = new Account {Username = username, Password = password, Email = email};
            Repository.AddUser(newUser);
        }

        public bool Login(string username, string password)
        {
            var encryption = new Encrypt();
            password = encryption.Hash(password);
            return Repository.CheckCredentials(username, password);
        }

        public bool IsUsernameTaken(string username)
        {
           return Repository.CheckIfUsernameIsTaken(username);
        }

        public IAccount GetAccountByUsername(string username)
        {
            var account = Repository.FindByUsername(username);
            return account;
        }

        public void UpdateUserDetails(IAccount account)
        {
            var encryption = new Encrypt();
            account.Password = encryption.Hash(account.Password);
            Repository.UpdateUserDetails(account);
        }
    }
}
