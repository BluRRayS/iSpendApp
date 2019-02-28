using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSpendApp.Models
{
    public class Account
    {
        private readonly int _id;
        public string AccountName { get; private set; }
        public double Balance { get; private set; }
        private readonly List<Transaction> _transactions;

        public Account()
        {
            _transactions = new List<Transaction>();
        }

        public Account(int id,string accountName, double balance)
        {
            _id = id;
            AccountName = accountName;
            Balance = balance;
            _transactions = new List<Transaction>();
        }

        public IReadOnlyList<Transaction> GetTransactions()
        {
            return _transactions as IReadOnlyList<Transaction>;
        }

        public int GetAccountId()
        {
            return _id;
        }



        public void AddTransaction(string name, double amount,Category category)
        {
            //
        }

        public void RemoveTransaction(int index)
        {
            //
        }

        public void RemoveAllTransactions()
        {
            //Remove all transactions from database when account gets deleted.
        }

    }
}
