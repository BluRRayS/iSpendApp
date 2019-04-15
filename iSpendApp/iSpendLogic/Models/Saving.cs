using System;
using System.Collections.Generic;
using System.Text;
using iSpendInterfaces;
using iSpendInterfaces.Helpers;

namespace iSpendLogic.Models
{
    internal class Saving :ISaving
    {
        public int UserId { get; set; }
        public int SavingId { get; set; }
        public string SavingName { get; set; }
        public decimal SavingCurrentAmount { get; set; }
        public decimal SavingsGoalAmount { get; set; }
        public SavingState State { get; set; }
        public int IconId { get; set; }
        public DateTime GoalDate { get; set; }
    }
}
