using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using iSpendInterfaces.Helpers;

namespace iSpendInterfaces
{
    public interface ISaving
    {
        int UserId { get; set; }
        int SavingId { get; set; }
        string SavingName { get; set; }
        decimal SavingCurrentAmount { get; set; }
        decimal SavingsGoalAmount { get; set; }
        SavingState State { get; set; }
        int IconId { get; set; }
        DateTime GoalDate { get; set; }
    }

}
