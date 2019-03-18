using System;
using System.Collections.Generic;
using System.Text;
using iSpendInterfaces;

namespace iSpendDAL.ContextInterfaces
{
    public interface IAccountContext
    {
        void AddUser(IAccount account);
        bool CheckIfUserNameIsTaken(string username);
        IAccount GetAccountByUsername(string username);
        IAccount GetAccountById(int userId);
        bool CheckCredentials(string username, string password);
    } 
}
