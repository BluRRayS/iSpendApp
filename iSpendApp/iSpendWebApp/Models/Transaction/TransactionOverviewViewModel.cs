using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iSpendInterfaces;

namespace iSpendWebApp.Models.Transaction
{
    public class TransactionOverviewViewModel
    {
        public TransactionOverviewViewModel(IEnumerable<TransactionsViewModel> transactions, IEnumerable<TransactionsViewModel> scheduledTransactions)
        {
            Transactions = transactions.ToList();
            ScheduledTransactions = scheduledTransactions.ToList();
        }

        public IEnumerable<TransactionsViewModel> Transactions { get; }
        public IEnumerable<TransactionsViewModel> ScheduledTransactions { get; }
    }
}
