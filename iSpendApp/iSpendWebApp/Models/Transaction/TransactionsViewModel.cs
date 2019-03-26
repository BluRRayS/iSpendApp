using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using iSpendInterfaces;

namespace iSpendWebApp.Models.Transaction
{
    public class TransactionsViewModel:ITransaction
    {
        public TransactionsViewModel()
        {
                AccountId = new int();
        }

        public TransactionsViewModel(int id, int accountId, string name, decimal amount, string category, int iconId, DateTime time)
        {
            TransactionId = id;
            AccountId = accountId;
            TransactionName = name;
            TransactionAmount = amount;
            Category = category;
            IconId = iconId;
            TimeOfTransaction = time;
        }

        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        [Required]
        public string TransactionName { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal TransactionAmount { get; set; }
        public string Category { get; set; }
        public int IconId { get; set; }
        public DateTime TimeOfTransaction { get; set; }
        public IEnumerable<string> Categories { get; set; }

    }
}
