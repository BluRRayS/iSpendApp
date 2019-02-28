using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iSpendWebApp.Models
{
    public class AccountViewModel
    {
        public int AccountId { get; set; }

        [Required]
        [Display (Name = "Account Name")]
        public string AccountName { get; set; }

        [Required]
        [Display(Name = "=Amount")]
        public double Amount { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Icon")]
        public int IconId { get; set; }

        public List<TransactionsViewModel> Transactions { get; set; }


    }
}
