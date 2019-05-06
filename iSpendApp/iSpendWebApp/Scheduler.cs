using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using iSpendDAL.ContextInterfaces;
using iSpendLogic;

namespace iSpendWebApp
{
    public class Scheduler
    {
        private readonly TransactionLogic _transactionLogic;
        public Scheduler(ITransactionContext transactionContext)
        {
            _transactionLogic = new TransactionLogic(transactionContext);
        }

        public void Start()
        {
            while (true)
            {
                if (DateTime.Now.Hour != 0) continue;
                _transactionLogic.ExecuteScheduledTransactions(DateTime.Now);
                Thread.Sleep(3601000); //1 hour and a second just to be sure :)
            }           
        }
    }
}
