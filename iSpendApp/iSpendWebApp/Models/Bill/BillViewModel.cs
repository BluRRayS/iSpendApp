using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using iSpendInterfaces;

namespace iSpendWebApp.Models.Bill
{
    public class BillViewModel:IBill
    {
        public BillViewModel(IEnumerable<string>icons)
        {
            var iconNames = icons.Select(iconName => iconName.Remove(iconName.Length - 4)).ToList();
            Icons = iconNames;
        }

        public BillViewModel()
        {
            
        }

        public BillViewModel(int billId, string billName, double billBalance, IEnumerable<ITransaction> transactions, int iconId, IEnumerable<int> accountIds, IEnumerable<string> icons)
        {
            BillId = billId;
            BillName = billName;
            BillBalance = billBalance;
            Transactions = transactions;
            IconId = iconId;
            AccountIds = accountIds;
            var iconNames = icons.Select(iconName => iconName.Remove(iconName.Length - 4)).ToList();
            Icons = iconNames;
        }

        public int BillId { get; set; }
        [DisplayName("Name")]
        [Required]
        public string BillName { get; set; }

        [DisplayName("Balance")]
        [Required]
        [DataType(DataType.Currency)]
        public double BillBalance { get; set; }

        public IEnumerable<ITransaction> Transactions { get; set; }
        public int IconId { get; set; }
        public IEnumerable<int> AccountIds { get; set; }
        public int UserId { get; set; }
        public List<string> Icons { get; set; }
    }
}
