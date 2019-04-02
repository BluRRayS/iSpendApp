using System;
using System.Collections.Generic;
using System.Text;
using iSpendDAL.ContextInterfaces;

namespace iSpendDAL.Savings
{
    public class SavingsRepository
    {
        private readonly ISavingsContext _context;
        public SavingsRepository(ISavingsContext context)
        {
            _context = context;
        }
    }
}
