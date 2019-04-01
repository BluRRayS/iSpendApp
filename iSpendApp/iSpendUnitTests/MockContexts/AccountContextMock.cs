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
