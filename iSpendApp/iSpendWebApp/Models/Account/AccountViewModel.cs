using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using iSpendInterfaces;

namespace iSpendWebApp.Models.Account
{
    public class AccountViewModel:IAccount
    {
        public AccountViewModel(IEnumerable<string>icons)
        {
            var iconNames = icons.Select(iconName => iconName.Remove(iconName.Length - 4)).ToList();
            Icons = iconNames;
        }

        public AccountViewModel()
        {
            
        }

        public AccountViewModel(int billId, string billName, double billBalance, IEnumerable<ITransaction> transactions, int iconId, IEnumerable<int> accountIds, IEnumerable<string> icons, IEnumerable<IReservation> reservations)
        {
            AccountId = billId;
            AccountName = billName;
            AccountBalance = billBalance;
            Transactions = transactions;
            IconId = iconId;
            UserIds = accountIds;
            var iconNames = icons.Select(iconName => iconName.Remove(iconName.Length - 4)).ToList();
            Icons = iconNames;
            Reservations = reservations;
        }

        public int AccountId { get; set; }
        [DisplayName("Name")]
        [Required]
        public string AccountName { get; set; }

        [DisplayName("Balance")]
        [Required]
        [DataType(DataType.Currency)]
        public double AccountBalance { get; set; }

        public IEnumerable<ITransaction> Transactions { get; set; }
        public int IconId { get; set; }
        public IEnumerable<int> UserIds { get; set; }
        public IEnumerable<IReservation> Reservations { get; set; }
        public int UserId { get; set; }
        public List<string> Icons { get; set; }
    }
}
