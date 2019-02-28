using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSpendApp.Models
{
    public class Transaction
    {
        private string _name;
        private int _id;
        private double _amount;
       // private Category _category;

        public Transaction(int id, string name, double amount/* ,Category category*/)
        {
            _name = name;
            _id = id;
            _amount = amount;
            //_category = category;
        }


        public void Remove()
        {
            //Sql remove
        }
    }
}
