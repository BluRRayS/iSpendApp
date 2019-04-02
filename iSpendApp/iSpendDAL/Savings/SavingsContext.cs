using System;
using System.Collections.Generic;
using System.Text;
using iSpendDAL.ContextInterfaces;

namespace iSpendDAL.Savings
{
    public class SavingsContext:ISavingsContext
    {
        private readonly DatabaseConnection _connection;
        public SavingsContext(DatabaseConnection connection)
        {
            _connection = connection;
        }
    }
}
