using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iSpendDAL.Account;
using iSpendDAL.ContextInterfaces;
using iSpendDAL.Transaction;
using iSpendInterfaces;
using iSpendLogic.Models;

namespace iSpendLogic
{
    public class StatisticsLogic
    {
        private readonly AccountRepository _accountRepository;
        private readonly TransactionRepository _transactionRepository;
        public StatisticsLogic(IAccountContext accountContext, ITransactionContext transactionContext)
        {
            _accountRepository = new AccountRepository(accountContext);
            _transactionRepository = new TransactionRepository(transactionContext);
        }

        public ICategoryStatistics GetUserCategoryStatistics(string username)
        {
            var stats = new CategoryStatistics();
            var accountIds = _accountRepository.GetUserBills(username);
            var transactions = new List<ITransaction>();
            foreach (var id in accountIds)
            {
                transactions.AddRange(_transactionRepository.GetBillTransactions(id.AccountId).Where(trans =>
                    (trans.Category != "Upload") && (trans.Category != "Start") && (trans.Category != "SavingPlan")).ToList());
            }
            stats.Categories = transactions.Select(trans => trans.Category).Distinct();
            var avgCostCategories = stats.Categories.Select(category => transactions.Where(trans => trans.Category == category).Average(trans => trans.TransactionAmount)).ToList();
            stats.AvgCostPerCategory = avgCostCategories;

            return stats;
        }

        public ITotalBalanceStatistics GetTotalBalanceStatistics(int userId)
        {
            return _transactionRepository.GetTotalBalanceStatistics(userId);
        }

    }
}
