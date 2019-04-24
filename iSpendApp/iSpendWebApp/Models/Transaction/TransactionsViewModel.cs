using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using iSpendInterfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace iSpendWebApp.Models.Transaction
{
    public class TransactionsViewModel:ITransaction
    {
        public TransactionsViewModel(int billId,IEnumerable<string>icons)
        {
            AccountId = billId;
            var iconNames = icons.Select(iconName => iconName.Remove(iconName.Length - 4)).ToList();
            Icons = iconNames;
        }

        public TransactionsViewModel()
        {
            
        }

        public TransactionsViewModel(int id, int accountId, string name, decimal amount, string category, int iconId, DateTime time,IEnumerable<string> icons)
        {
            TransactionId = id;
            AccountId = accountId;
            TransactionName = name;
            TransactionAmount = amount;
            Category = category;
            IconId = iconId;
            TimeOfTransaction = time;
            var iconNames = icons.Select(iconName => iconName.Remove(iconName.Length - 4)).ToList();
            Icons = iconNames;
        }

        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string TransactionName { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Amount")]
        public decimal TransactionAmount { get; set; }
        public string Category { get; set; }
        [Display(Name = "Icon")]
        public int IconId { get; set; }
        [Display(Name = "Time Of Transaction")]
        public DateTime TimeOfTransaction { get; set; }
        public List<string> Icons { get; set; }
    }
}
