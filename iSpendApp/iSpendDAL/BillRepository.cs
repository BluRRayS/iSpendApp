﻿using System;
using System.Collections.Generic;
using System.Text;
using iSpendDAL.ContextInterfaces;
using iSpendInterfaces;

namespace iSpendDAL
{
    public class BillRepository
    {
        private readonly IBillContext _context;

        public BillRepository(IBillContext context)
        {
            _context = context;
        }

        public void AddBill(IBill newBill)
        {
            _context.AddBill(newBill);
        }

        public IEnumerable<IBill> GetUserBills(string username)
        {
           return _context.GetBillsByUsername(username);
        }
    }
}
