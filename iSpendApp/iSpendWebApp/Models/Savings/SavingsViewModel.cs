using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using iSpendInterfaces;
using iSpendInterfaces.Helpers;

namespace iSpendWebApp.Models.Savings
{
    public class SavingsViewModel:ISaving
    {
        public int SavingId { get; set; }
        [Required]
        [DisplayName("Name")]
        public string SavingName { get; set; }
        [Required]
        [DisplayName("Start Amount")]
        public decimal SavingCurrentAmount { get; set; }
        [Required]
        [DisplayName("Goal")]
        public decimal SavingsGoalAmount { get; set; }
        public SavingState State { get; set; }
        public int IconId { get; set; }
    }
}
