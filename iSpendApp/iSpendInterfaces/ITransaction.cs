using System;
using System.Collections.Generic;
using System.Text;

namespace iSpendInterfaces
{
    public interface ITransaction
    {
        int TransactionId { get; set; }
        int BillId { get; set; }
        string TransactionName { get; set; }
        decimal TransactionAmount { get; set; }
        string Category { get; set; }
        int IconId { get; set; }
        DateTime TimeOfTransaction { get; set; }
    }
}
