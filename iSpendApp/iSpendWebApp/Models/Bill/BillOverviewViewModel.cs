using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iSpendInterfaces;

namespace iSpendWebApp.Models.Bill
{
    public class BillOverviewViewModel
    {
        public List<IBill> Bills { get; set; }
    }
}
