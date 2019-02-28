using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace iSpendApp.Models
{
    public class AccountDAL
    {
        private string connectionString ="Server=mssql.fhict.local;Database=dbi412182;User Id=dbi412182;Password=!LeGo2001;";
        private List<Account> _accounts;
        private List<Transaction> _transactions;
        private string _accountName;
        private double _accountBalance;

        public AccountDAL()
        {
            _accounts = new List<Account>();
        }

        public void AddAccount(NewAccount account)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("INSERT INTO [Account](Name,Balance,DateOfCreation) VALUES(@Name,@Balance,@DateOfCreation)",conn);
                command.Parameters.AddWithValue("@Name", account.AccountName);
                command.Parameters.AddWithValue("@Balance", account.Balance);
                command.Parameters.AddWithValue("@DateOfCreation", DateTime.Now);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void RemoveAccount(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("DELETE FROM Account WHERE Id = @Id", conn);
                command.Parameters.AddWithValue("Id", id);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        public IReadOnlyList<Account> GetAllAccounts()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT Id,Name,Balance FROM Account Order By Name", conn);
                command.ExecuteNonQuery();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _accounts.Add(new Account(reader.GetInt32(0), reader.GetString(1), reader.GetDouble(2)));
                    }
                }
                conn.Close();
            }
            return _accounts as IReadOnlyList<Account>;
        }

        public Account GetAccount(int accountId)
        {
            _accounts.Clear();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT Name,Balance FROM Account", conn);
                command.ExecuteNonQuery();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _accounts.Add(new Account(accountId, reader.GetString(0), reader.GetDouble(1)));
                    }
                }
                conn.Close();
            }
            return _accounts[0];
        }

        public IReadOnlyList<Transaction> GetTransactions(int accountId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Transaction WHERE Id = @Id",conn);
                command.Parameters.AddWithValue("@Id",accountId);
                command.ExecuteNonQuery();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _transactions.Add(new Transaction(reader.GetInt32(0),reader.GetName(1),reader.GetDouble(2)));
                    }
                }
                conn.Close();
            }
            return _transactions as IReadOnlyList<Transaction>;
        }
    }
}
