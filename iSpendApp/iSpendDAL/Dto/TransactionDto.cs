using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using iSpendInterfaces;

namespace iSpendDAL.Dto
{
    internal class TransactionDto: ITransaction
    {
        public TransactionDto(int id,string name,decimal amount, string category, int iconId, DateTime timeOfTransaction)
        {
            TransactionId = id;
            TransactionName = name;
            TransactionAmount = amount;
            Category = category;
            IconId = iconId;
            TimeOfTransaction = timeOfTransaction;
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
