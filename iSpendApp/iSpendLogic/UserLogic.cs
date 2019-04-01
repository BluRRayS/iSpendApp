using System;
using iSpendDAL;
using iSpendDAL.Account;
using iSpendDAL.ContextInterfaces;
using iSpendInterfaces;
using iSpendLogic.Helpers;
using iSpendLogic.Models;

namespace iSpendLogic
{
    public class UserLogic
    {
        private AccountRepository Repository { get; }

        public UserLogic(IAccountContext context)
        {
            Repository = new AccountRepository(context);
        }

        public void AddUser(string username, string password, string email)
        {
            var encryption = new Encrypt();
            password = encryption.Hash(password);
            var newUser = new User {Username = username, Password = password, Email = email};
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

        public IUser GetAccountByUsername(string username)
        {
            var account = Repository.FindByUsername(username);
            return account;
        }

        public void UpdateUserDetails(IUser account)
        {
            var encryption = new Encrypt();
            account.Password = encryption.Hash(account.Password);
            Repository.UpdateUserDetails(account);
        }
    }
}
