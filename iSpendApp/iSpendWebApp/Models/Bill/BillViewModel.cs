using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using iSpendInterfaces;

namespace iSpendWebApp.Models
{
    public class BillViewModel :IBill
    {
        public int BillId { get; set; }
        public string BillName { get; set; }
        public decimal BillBalance { get; set; }
        public IEnumerable<ITransaction> Transactions { get; set; }
        public int IconId { get; set; }
        public IEnumerable<int> AccountIds { get; set; }
    }
}
