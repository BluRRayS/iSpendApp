using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace iSpendWebApp.Models
{
    public sealed class RabobankTransactionsMap : ClassMap<RabobankTransactions>
    {
        public RabobankTransactionsMap()
        {
            AutoMap();
            Map(m => m.Name).Name("Tegenrekening IBAN/BBAN");
        }
    }
}
