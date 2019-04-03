using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iSpendWebApp.Models.Bill;
using iSpendWebApp.Models.Savings;

namespace iSpendWebApp.Models
{
    public class LandingPageViewModel
    {
        public LandingPageViewModel(IEnumerable<BillViewModel> bills, IEnumerable<SavingsViewModel> savings)
        {
            Bills = new List<BillViewModel>();
            Savings = new List<SavingsViewModel>();
            Bills.AddRange(bills);
            Savings.AddRange(savings);
        }

        public List<BillViewModel> Bills { get;  set; }
        public List<SavingsViewModel> Savings { get; set; }
    }
}
