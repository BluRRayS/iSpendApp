using System;
using System.Collections.Generic;
using System.Text;
using iSpendInterfaces;

namespace iSpendUnitTests.LogicTests.Models
{
    public class Transaction:ITransaction
    {
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public string TransactionName { get; set; }
        public decimal TransactionAmount { get; set; }
        public string Category { get; set; }
        public int IconId { get; set; }
        public DateTime TimeOfTransaction { get; set; }
    }
}
