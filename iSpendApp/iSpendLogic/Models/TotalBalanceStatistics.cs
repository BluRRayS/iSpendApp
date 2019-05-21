using System;
using System.Collections.Generic;
using System.Text;
using iSpendInterfaces;

namespace iSpendLogic.Models
{
    internal class TotalBalanceStatistics :ITotalBalanceStatistics
    {
        public TotalBalanceStatistics(IEnumerable<decimal> balances, IEnumerable<int> timeStamps)
        {
            Balances = balances;
            MonthNumber = timeStamps;
        }

        public IEnumerable<decimal> Balances { get; set; }
        public IEnumerable<int> MonthNumber { get; set; }
    }
}
