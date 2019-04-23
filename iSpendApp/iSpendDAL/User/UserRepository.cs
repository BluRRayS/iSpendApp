using System.Collections.Generic;
using iSpendDAL.ContextInterfaces;
using iSpendInterfaces;

namespace iSpendDAL.User
{
    public class UserRepository
    {
        private readonly IUserContext _context;

        public UserRepository(IUserContext context)
        {
            _context = context;
        }

        public void AddUser(IUser newUser)
        {
            _context.AddUser(newUser);
        }

        public IUser FindById(int id)
        {
            return _context.GetAccountById(id);
        }

        public IUser FindByUsername(string username)
        {
            return _context.GetAccountByUsername(username);
        }

        public bool CheckCredentials(string username, string password)
        {
            return _context.CheckCredentials(username, password);
        }

        public bool CheckIfUsernameIsTaken(string username)
        {
            return _context.CheckIfUserNameIsTaken(username);
        }

        public void UpdateUserDetails(IUser account)
        {
            _context.UpdateUserDetails(account);
        }

        public IEnumerable<IUser> GetAllUsers()
        {
            return _context.GetAllUsers();
        }
    }
}
