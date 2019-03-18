using System;
using System.Collections.Generic;
using System.Text;
using iSpendDAL.ContextInterfaces;
using iSpendInterfaces;

namespace iSpendDAL
{
    public class AccountRepository
    {
        private readonly IAccountContext _context;

        public AccountRepository(IAccountContext context)
        {
            _context = context;
        }

        public void AddUser(IAccount newUser)
        {
            _context.AddUser(newUser);
        }

        public IAccount FindById(IAccount user)
        {
            return _context.GetAccountById(user.UserId);
        }

        public IAccount FindByUsername(IAccount user)
        {
            return _context.GetAccountByUsername(user.Username);
        }

        public bool CheckCredentials(string username, string password)
        {
            return _context.CheckCredentials(username, password);
        }

        public bool CheckIfUsernameIsTaken(string username)
        {
            return _context.CheckIfUserNameIsTaken(username);
        }

    }
}
