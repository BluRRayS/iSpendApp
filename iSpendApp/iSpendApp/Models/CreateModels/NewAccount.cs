using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iSpendApp.Models
{
    public class NewAccount
    {
        [Required]
        public string AccountName { get; set; }

        [Required]
        [DataType(dataType: DataType.Currency)]
        public double Balance { get; set; }
    }
}
