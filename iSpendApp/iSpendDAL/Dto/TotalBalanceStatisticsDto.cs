using System;
using System.Collections.Generic;
using System.Text;
using iSpendInterfaces;

namespace iSpendDAL.Dto
{
    internal class TotalBalanceStatisticsDto:ITotalBalanceStatistics
    {
        public TotalBalanceStatisticsDto(IEnumerable<decimal> balances, IEnumerable<int> monthNumber)
        {
            Balances = balances;
            MonthNumber = monthNumber;
        }

        public IEnumerable<decimal> Balances { get; set; }
        public IEnumerable<int> MonthNumber { get; set; }
    }
}
