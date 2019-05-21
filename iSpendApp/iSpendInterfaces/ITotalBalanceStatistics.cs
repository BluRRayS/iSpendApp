using System;
using System.Collections.Generic;
using System.Text;

namespace iSpendInterfaces
{
    public interface ITotalBalanceStatistics
    {
        IEnumerable<decimal> Balances { get; set; }
        IEnumerable<int> MonthNumber { get; set; }
    }
}
