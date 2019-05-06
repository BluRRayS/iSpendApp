using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;
using iSpendInterfaces;

namespace iSpendWebApp.Models
{
    public class RabobankTransactions:ITransactionsFile
    {
        [Name("Naam tegenpartij")]
        public string Name { get; set; }
        [Name("Bedrag")]
        public decimal Amount { get; set; }
        [Name("Datum")]
        public DateTime TimeOfTransaction { get; set; }
        //[Name("omschrijving-1")]
        //public string Description { get; set; }
    }
}
