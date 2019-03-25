using System;
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

        public void AddBill(IBill newBill, int userId)
        {
            _context.AddBill(newBill, userId);
        }

        public IEnumerable<IBill> GetUserBills(string username)
        {
           return _context.GetBillsByUsername(username);
        }

        public IBill GetBillById(int id)
        {
            return _context.GetBillById(id);
        }

        public void UpdateBill(int id,string name, int iconId)
        {
            _context.UpdateBill(id,name,iconId);
        }

        public void RemoveBill(int id)
        {
            _context.RemoveBill(id);
        }
    }
}
