using System;
using System.Collections.Generic;
using System.Text;
using iSpendInterfaces;

namespace iSpendDAL.ContextInterfaces
{
    public interface IUserContext
    {
        void AddUser(IUser account);
        bool CheckIfUserNameIsTaken(string username);
        IUser GetAccountByUsername(string username);
        IUser GetAccountById(int userId);
        bool CheckCredentials(string username, string password);
        void UpdateUserDetails(IUser account);
        IEnumerable<IUser> GetAllUsers();

    } 
}
