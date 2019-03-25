﻿using System;
using System.Collections.Generic;
using System.Text;
using iSpendDAL;
using iSpendDAL.ContextInterfaces;
using iSpendInterfaces;

namespace iSpendLogic
{
    public class BillLogic
    {
        private BillRepository Repository { get; }

        public BillLogic(IBillContext context)
        {
            Repository = new BillRepository(context);
        }

        public void AddBill(IBill newBill , int userId)
        {
            Repository.AddBill(newBill, userId);
        }

        public IEnumerable<IBill> GetUserBills(string username)
        {
           return Repository.GetUserBills(username);
        }
    }
}
