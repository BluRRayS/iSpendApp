using iSpendDAL.ContextInterfaces;
using iSpendInterfaces;

namespace iSpendDAL.Account
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

        public IAccount FindById(int id)
        {
            return _context.GetAccountById(id);
        }

        public IAccount FindByUsername(string username)
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

        public void UpdateUserDetails(IAccount account)
        {
            _context.UpdateUserDetails(account);
        }
    }
}
