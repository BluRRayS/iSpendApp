using System;
using System.Collections.Generic;
using System.Text;
using iSpendInterfaces;

namespace iSpendLogic.Models
{
    internal class Transaction:ITransaction
    {
        public Transaction()
        {

        }
        public Transaction(int id, string name, decimal amount, string category, int iconId, DateTime timeOfTransaction, int accountId)
        {
            TransactionId = id;
            TransactionName = name;
            TransactionAmount = amount;
            Category = category;
            IconId = iconId;
            TimeOfTransaction = timeOfTransaction;
            AccountId = accountId;
        }

        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public string TransactionName { get; set; }
        public decimal TransactionAmount { get; set; }
        public string Category { get; set; }
        public int IconId { get; set; }
        public DateTime TimeOfTransaction { get; set; }
    }
}
