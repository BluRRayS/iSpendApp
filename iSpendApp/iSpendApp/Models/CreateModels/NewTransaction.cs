using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iSpendApp.Models.CreateModels
{
    public class NewTransaction
    {
        [Required]
        public string Name { get; private set; }


        [Required]
        [DataType(DataType.Currency)]
        public double Amount { get; private set; }

    }
}
