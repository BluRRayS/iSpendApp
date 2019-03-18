using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iSpendWebApp.Models
{
    public class TransactionsViewModel
    {
        public int TransactionId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double Amount { get; set; }

        [Required]
        //public List<Category> CategoryList { get; set; }

        public int AccountId { get; set; }

    }
}
