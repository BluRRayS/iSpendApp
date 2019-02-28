using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace iSpendApp.Models
{
    public class AccountManager
    {
        private string _errorMessage;
        private List<Account> _accounts;
        private AccountDAL data;

        public AccountManager()
        {
            _accounts = new List<Account>();
            data = new AccountDAL();
        }

        public IReadOnlyList<Account> GetAccounts()
        {
            GetAllAccounts();
            return _accounts as IReadOnlyList<Account>;
        }

        public Account GetAccount(int accountId)
        {
            return data.GetAccount(accountId);
        }

        public string ErrorMessage()
        {
            return _errorMessage;
        }

        private void GetAllAccounts()
        {
            _accounts.Clear();
            try
            {
                _accounts.AddRange(data.GetAllAccounts());
            }
            catch
            {
                _errorMessage = "Error Getting data from database";
            }
        }


    }
}
