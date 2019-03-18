using System;
using System.Collections.Generic;
using System.Text;
using iSpendDAL.ContextInterfaces;
using iSpendInterfaces;

namespace iSpendUnitTests.MockContexts
{
    class AccountContextMock:IAccountContext
    {
        public void AddUser(IAccount account)
        {
            throw new NotImplementedException();
        }

        public bool CheckIfUserNameIsTaken(string username)
        {
            throw new NotImplementedException();
        }

        public IAccount GetAccountByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public IAccount GetAccountById(int userId)
        {
            throw new NotImplementedException();
        }

        public bool CheckCredentials(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
