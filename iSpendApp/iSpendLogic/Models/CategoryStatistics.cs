using System;
using System.Collections.Generic;
using System.Text;
using iSpendInterfaces;

namespace iSpendLogic.Models
{
    internal class CategoryStatistics :ICategoryStatistics
    {
        public IEnumerable<string> Categories { get; set; }
        public IEnumerable<decimal> AvgCostPerCategory { get; set; }
    }
}
