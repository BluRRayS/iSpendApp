using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSpendApp.Models
{
    public class User
    {
        private string _userName;
        private List<Account> _accounts;

        public User(string userName, List<Account> accounts)
        {
            this._userName = userName;
            this._accounts = accounts;
        }
    }
}
