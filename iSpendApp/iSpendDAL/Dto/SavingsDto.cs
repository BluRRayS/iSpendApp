using System;
using System.Collections.Generic;
using System.Text;
using iSpendInterfaces;
using iSpendInterfaces.Helpers;

namespace iSpendDAL.Dto
{
    internal class SavingsDto :ISaving
    {
        public SavingsDto()
        {
            
        }

        public SavingsDto(int userId, int savingId, string savingName, decimal savingCurrentAmount, decimal savingsGoalAmount, SavingState state, int iconId, DateTime goalDate)
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
        public string SavingName { get; set; }
        public decimal SavingCurrentAmount { get; set; }
        public decimal SavingsGoalAmount { get; set; }
        public SavingState State { get; set; }
        public int IconId { get; set; }
        public DateTime GoalDate { get; set; }
    }
}
