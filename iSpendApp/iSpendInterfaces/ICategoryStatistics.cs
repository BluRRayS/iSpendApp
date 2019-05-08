using System;
using System.Collections.Generic;
using System.Text;

namespace iSpendInterfaces
{
    public interface ICategoryStatistics
    {
        IEnumerable<string> Categories { get; set; }
        IEnumerable<decimal> AvgCostPerCategory { get; set; }
    }
}
