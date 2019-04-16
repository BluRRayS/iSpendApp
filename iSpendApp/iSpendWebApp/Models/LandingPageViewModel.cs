using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iSpendWebApp.Models.Account;
using iSpendWebApp.Models.Savings;

namespace iSpendWebApp.Models
{
    public class LandingPageViewModel
    {
        public LandingPageViewModel(IEnumerable<AccountViewModel> accounts, IEnumerable<SavingsViewModel> savings)
        {
            Accounts = new List<AccountViewModel>();
            Savings = new List<SavingsViewModel>();
            Accounts.AddRange(accounts);
            Savings.AddRange(savings);
        }

        public List<AccountViewModel> Accounts { get;  set; }
        public List<SavingsViewModel> Savings { get; set; }
    }
}
