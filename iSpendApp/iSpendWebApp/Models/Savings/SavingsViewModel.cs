using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using iSpendInterfaces;
using iSpendInterfaces.Helpers;

namespace iSpendWebApp.Models.Savings
{
    public class SavingsViewModel:ISaving
    {
        public SavingsViewModel()
        {
            
        }

        public SavingsViewModel(int userId, int savingId, string savingName, decimal savingCurrentAmount, decimal savingsGoalAmount, SavingState state, int iconId, DateTime goalDate)
        {
            UserId = userId;
            SavingId = savingId;
            SavingName = savingName;
            SavingCurrentAmount = savingCurrentAmount;
            SavingsGoalAmount = savingsGoalAmount;
            State = state;
            IconId = iconId;
            GoalDate = goalDate;
        }


        public int UserId { get; set; }
        public int SavingId { get; set; }
        [Required]
        public string SavingName { get; set; }
        [Required]
        public decimal SavingCurrentAmount { get; set; }
        [Required]
        public decimal SavingsGoalAmount { get; set; }
        public SavingState State { get; set; }
        [Required]
        public int IconId { get; set; }
        [Required]
        public DateTime GoalDate { get; set; }
    }
}
