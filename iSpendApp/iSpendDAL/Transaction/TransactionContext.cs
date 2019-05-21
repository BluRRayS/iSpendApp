using iSpendDAL.ContextInterfaces;
using iSpendDAL.Dto;
using iSpendInterfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace iSpendDAL.Transaction
{
    public class TransactionContext : ITransactionContext
    {
        private readonly DatabaseConnection _connection;

        public TransactionContext(DatabaseConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<ITransaction> GetBillTransactions(int id)
        {
            var billTransactions = new List<TransactionDto>();
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT Id,Name,Amount,AccountId,Category,TimeOfTransaction,IconId FROM dbo.[Transaction] WHERE AccountId=@Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        billTransactions.Add(new TransactionDto(reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(2), reader.GetString(4), reader.GetInt32(6), reader.GetDateTime(5), reader.GetInt32(3)));
                    }
                }

                connection.Close();
            }
            return billTransactions;        
        }

        public void CreateTransaction(ITransaction transaction)
        {
            using (var connection = _connection.SqlConnection)
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO dbo.[Transaction] (Name,Amount,TimeOfTransaction,AccountId,Category,IconId) VALUES(@Name,@Amount,@Time,@AccountId,@Category,@IconId)", connection);
                command.Parameters.AddWithValue("@Name", transaction.TransactionName);
                command.Parameters.AddWithValue("@Amount", transaction.TransactionAmount);
                command.Parameters.AddWithValue("@Time", transaction.TimeOfTransaction);
                command.Parameters.AddWithValue("@AccountId", transaction.AccountId);
                command.Parameters.AddWithValue("@Category", transaction.Category);
                command.Parameters.AddWithValue("@IconId", transaction.IconId);
                command.ExecuteNonQuery();
                connection.Close();
            }       
        }

        public void DeleteTransaction(int id, int billId)
        {
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("DELETE FROM dbo.[Transaction] WHERE Id= @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            GetTotalBalance(billId);
        }

        public void UpdateTransaction(int id, ITransaction transaction)
        {
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("UPDATE dbo.[Transaction] SET Name =@Name,Amount=@Amount,Category=@Category,IconId=@Icon WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Name", transaction.TransactionName);
                command.Parameters.AddWithValue("@Amount", transaction.TransactionAmount);
                command.Parameters.AddWithValue("@Category", transaction.Category);
                command.Parameters.AddWithValue("@Icon", transaction.IconId);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            GetTotalBalance(transaction.AccountId);
        }

        public ITransaction GetTransactionById(int id, int billId)
        {
            var transaction = new TransactionDto();
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT Name,Amount,Category,IconId,TimeOfTransaction FROM dbo.[Transaction] WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        transaction.AccountId = billId;
                        transaction.TransactionId = id;
                        transaction.TransactionName = reader.GetString(0);
                        transaction.TransactionAmount = reader.GetDecimal(1);
                        transaction.Category = reader.GetString(2);
                        transaction.IconId = reader.GetInt32(3);
                        transaction.TimeOfTransaction = reader.GetDateTime(4);
                    }
                }
                connection.Close();
            }
            return transaction;
        }

        public IEnumerable<ICategory> GetCategories()
        {
            var categories = new List<CategoryDto>();
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT Id,Name,IconId FROM dbo.[Categories]", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new CategoryDto(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));
                    }
                }
                connection.Close();
            }
            return categories;
        }

        public IEnumerable<ITransaction> GetAccountScheduledTransactions(int id)
        {
            var transactions = new List<TransactionDto>();
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT Id,Name,Amount,Category,IconId,TimeOfTransaction,AccountId FROM dbo.[ScheduledTransactions] WHERE AccountId = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        transactions.Add(new TransactionDto(reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(2), reader.GetString(3), reader.GetInt32(4), reader.GetDateTime(5), reader.GetInt32(6)));
                    }
                }
                connection.Close();
            }
            return transactions;
        }

        public IEnumerable<ITransaction> GetAllScheduledTransactions()
        {
            var transactions = new List<TransactionDto>();
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT Id,Name,Amount,Category,IconId,TimeOfTransaction,AccountId FROM dbo.[ScheduledTransactions]", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        transactions.Add(new TransactionDto(reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(2), reader.GetString(3), reader.GetInt32(4), reader.GetDateTime(5), reader.GetInt32(6)));
                    }
                }
                connection.Close();
            }
            return transactions;
        }

        public void RemoveScheduledTransaction(int id)
        {
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("DELETE FROM dbo.[ScheduledTransactions] WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void AddScheduledTransaction(ITransaction transaction)
        {
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("INSERT INTO dbo.[ScheduledTransactions] (Name,Amount,TimeOfTransaction,AccountId,Category,IconId) VALUES(@Name,@Amount,@Time,@AccountId,@Category,@IconId)", connection);
                command.Parameters.AddWithValue("@Name", transaction.TransactionName);
                command.Parameters.AddWithValue("@Amount", transaction.TransactionAmount);
                command.Parameters.AddWithValue("@Time", transaction.TimeOfTransaction);
                command.Parameters.AddWithValue("@AccountId", transaction.AccountId);
                command.Parameters.AddWithValue("@Category", transaction.Category);
                command.Parameters.AddWithValue("@IconId", transaction.IconId);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void EditScheduledTransaction(ITransaction transaction)
        {
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("UPDATE dbo.[ScheduledTransactions] SET Name =@Name,Amount=@Amount,TimeOfTransaction=@Time,AccountId=@AccountId,Category=@Category,IconId=@Icon WHERE Id = @Id ", connection);
                command.Parameters.AddWithValue("@Name", transaction.TransactionName);
                command.Parameters.AddWithValue("@Amount", transaction.TransactionAmount);
                command.Parameters.AddWithValue("@Time", transaction.TimeOfTransaction);
                command.Parameters.AddWithValue("@AccountId", transaction.AccountId);
                command.Parameters.AddWithValue("@Category", transaction.Category);
                command.Parameters.AddWithValue("@Icon", transaction.IconId);
                command.Parameters.AddWithValue("@Id", transaction.TransactionId);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void GetTotalBalance(int billId)
        {
            decimal sum = 0;
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT SUM(Cast(Amount as decimal(18,2))) FROM dbo.[Transaction] WHERE AccountId = @Id", connection);
                command.Parameters.AddWithValue("@Id", billId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    try
                    {
                        if (reader.Read())
                        {
                            sum = reader.GetDecimal(0);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        _connection.SqlConnection.Close();
                        UpdateBillBalance(billId, sum);
                    }

                }
                connection.Close();
            }
            UpdateBillBalance(billId, sum);
        }

        public void UpdateBillBalance(int billId, decimal amount)
        {
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("UPDATE dbo.Account SET Balance = @Amount WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", billId);
                command.Parameters.AddWithValue("@Amount", amount);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public ITransaction GetScheduledTransactionById(int id)
        {
            var transaction = new TransactionDto();
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT * FROM dbo.[ScheduledTransactions] WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        transaction.TransactionId = id;
                        transaction.AccountId = reader.GetInt32(1);
                        transaction.TimeOfTransaction = reader.GetDateTime(2);
                        transaction.TransactionAmount = reader.GetDecimal(3);
                        transaction.Category = reader.GetString(4);
                        transaction.IconId = reader.GetInt32(5);
                        transaction.TransactionName = reader.GetString(6);
                    }
                }
                connection.Close();

            }
            return transaction;
        }

        public void ImportTransactions(IEnumerable<ITransaction> transactions)
        {
            foreach (var item in transactions)
            {
                CreateTransaction(item);
            }
        }

        public ITotalBalanceStatistics GetTotalBalanceStatistics(int userId)
        {
            var balances = new List<decimal>();
            var monthNumbers = new List<int>();
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("dbo.GetUsersTotalBalanceStatistics", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@UserId", userId);
                connection.Open();
                command.ExecuteNonQuery();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                       balances.Add(reader.GetDecimal(0));
                       monthNumbers.Add(reader.GetInt32(1));
                    }                
                }
                connection.Close();               
            }
            var stats = new TotalBalanceStatisticsDto(balances,monthNumbers);
            return stats;
        }
    }
}
