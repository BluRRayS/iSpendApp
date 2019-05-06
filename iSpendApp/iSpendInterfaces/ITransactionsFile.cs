using System;
using System.Collections.Generic;
using System.Text;

namespace iSpendInterfaces
{
    public interface ITransactionsFile
    {
        string Name { get; set; }
        decimal Amount { get; set; }
        DateTime TimeOfTransaction { get; set; }
        //string Description { get; set; }
    }
}
