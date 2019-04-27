using iSpendDAL.ContextInterfaces;
using iSpendDAL.Dto;
using iSpendInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace iSpendDAL.Transaction
{
    public class TransactionContext: ITransactionContext
    {
        private readonly DatabaseConnection _connection;

        public TransactionContext(DatabaseConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<ITransaction> GetBillTransactions(int id)
        {
            _connection.CheckConnection();
            _connection.SqlConnection.Open();
            var billTransactions = new List<TransactionDto>();
            var command = new SqlCommand("SELECT Id,Name,Amount,AccountId,Category,TimeOfTransaction,IconId FROM dbo.[Transaction] WHERE AccountId=@Id",_connection.SqlConnection);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    billTransactions.Add(new TransactionDto(reader.GetInt32(0),reader.GetString(1),reader.GetDecimal(2),reader.GetString(4),reader.GetInt32(6),reader.GetDateTime(5),reader.GetInt32(3)));
                }
            }
            _connection.SqlConnection.Close();
            return billTransactions;
        }

        public void CreateTransaction(ITransaction transaction)
        {
            _connection.CheckConnection();
            _connection.SqlConnection.Open();
            var command = new SqlCommand("INSERT INTO dbo.[Transaction] (Name,Amount,TimeOfTransaction,AccountId,Category,IconId) VALUES(@Name,@Amount,@Time,@AccountId,@Category,@IconId)", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Name",transaction.TransactionName);
            command.Parameters.AddWithValue("@Amount", transaction.TransactionAmount);
            command.Parameters.AddWithValue("@Time",DateTime.Now);
            command.Parameters.AddWithValue("@AccountId",transaction.AccountId);
            command.Parameters.AddWithValue("@Category",transaction.Category);
            command.Parameters.AddWithValue("@IconId", transaction.IconId);
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
        }

        public void DeleteTransaction(int id, int billId)
        {
            _connection.CheckConnection();
            _connection.SqlConnection.Open();
            var command = new SqlCommand("DELETE FROM dbo.[Transaction] WHERE Id= @Id", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();           
            _connection.SqlConnection.Close();
            GetTotalBalance(billId);
        }

        public void UpdateTransaction(int id, ITransaction transaction)
        {
            _connection.CheckConnection();
            _connection.SqlConnection.Open();
            var command = new SqlCommand("UPDATE dbo.[Transaction] SET Name =@Name,Amount=@Amount,Category=@Category,IconId=@Icon WHERE Id = @Id", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Name", transaction.TransactionName);
            command.Parameters.AddWithValue("@Amount", transaction.TransactionAmount);
            command.Parameters.AddWithValue("@Category", transaction.Category);
            command.Parameters.AddWithValue("@Icon", transaction.IconId);
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
            GetTotalBalance(transaction.AccountId);
        }

        public ITransaction GetTransactionById(int id, int billId)
        {
            _connection.CheckConnection();
            var transaction = new TransactionDto();
           _connection.SqlConnection.Open();
           var command = new SqlCommand("SELECT Name,Amount,Category,IconId,TimeOfTransaction FROM dbo.[Transaction] WHERE Id = @Id", _connection.SqlConnection);
           command.Parameters.AddWithValue("@Id", id);
           command.ExecuteNonQuery();
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

           _connection.SqlConnection.Close();
           
           return transaction;
        }

        public IEnumerable<ICategory> GetCategories()
        {
            _connection.CheckConnection();
            var categories = new List<CategoryDto>();
            _connection.SqlConnection.Open();
            var command = new SqlCommand("SELECT Id,Name,IconId FROM dbo.[Categories]",_connection.SqlConnection);
            command.ExecuteNonQuery();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    categories.Add(new CategoryDto(reader.GetInt32(0), reader.GetString(1),reader.GetInt32(2)));
                }
            }
            _connection.SqlConnection.Close();
            return categories;
        }

        public IEnumerable<ITransaction> GetAccountScheduledTransactions(int id)
        {
            _connection.CheckConnection();
            var transactions = new List<TransactionDto>();
            var command = new SqlCommand("SELECT Id,Name,Amount,Category,IconId,TimeOfTransaction,AccountId FROM dbo.[ScheduledTransactions] WHERE AccountId = @Id", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Id", id);
            _connection.SqlConnection.Open();
            command.ExecuteNonQuery();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    transactions.Add(new TransactionDto(reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(2), reader.GetString(3), reader.GetInt32(4), reader.GetDateTime(5), reader.GetInt32(6)));
                }
            }
            _connection.SqlConnection.Close();
            return transactions;
        }

        public IEnumerable<ITransaction> GetAllScheduledTransactions()
        {
            _connection.CheckConnection();
            var transactions = new List<TransactionDto>();
            var command = new SqlCommand("SELECT Id,Name,Amount,Category,IconId,TimeOfTransaction,AccountId FROM dbo.[ScheduledTransactions]", _connection.SqlConnection);
            _connection.SqlConnection.Open();
            command.ExecuteNonQuery();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    transactions.Add(new TransactionDto(reader.GetInt32(0),reader.GetString(1),reader.GetDecimal(2),reader.GetString(3),reader.GetInt32(4),reader.GetDateTime(5),reader.GetInt32(6)));
                }
            }
            _connection.SqlConnection.Close();
            return transactions;
        }

        public void RemoveScheduledTransaction(int id)
        {
            _connection.CheckConnection();
            var command = new SqlCommand("DELETE FROM dbo.[ScheduledTransactions] WHERE Id = @Id",_connection.SqlConnection);
            command.Parameters.AddWithValue("@Id", id);
            _connection.SqlConnection.Open();
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
        }

        public void AddScheduledTransaction(ITransaction transaction)
        {
            _connection.CheckConnection();
            var command = new SqlCommand("INSERT INTO dbo.[ScheduledTransactions] (Name,Amount,TimeOfTransaction,AccountId,Category,IconId) VALUES(@Name,@Amount,@Time,@AccountId,@Category,@IconId)", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Name", transaction.TransactionName);
            command.Parameters.AddWithValue("@Amount", transaction.TransactionAmount);
            command.Parameters.AddWithValue("@Time", transaction.TimeOfTransaction);
            command.Parameters.AddWithValue("@AccountId", transaction.AccountId);
            command.Parameters.AddWithValue("@Category", transaction.Category);
            command.Parameters.AddWithValue("@IconId", transaction.IconId);
            _connection.SqlConnection.Open();            
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
        }

        public void EditScheduledTransaction(ITransaction transaction)
        {
            _connection.CheckConnection();
            var command = new SqlCommand("UPDATE dbo.[ScheduledTransactions] SET Name =@Name,Amount=@Amount,TimeOfTransaction=@Time,AccountId=@AccountId,Category=@Category,IconId=@Icon WHERE Id = @Id ", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Name", transaction.TransactionName);
            command.Parameters.AddWithValue("@Amount", transaction.TransactionAmount);
            command.Parameters.AddWithValue("@Time", transaction.TimeOfTransaction);
            command.Parameters.AddWithValue("@AccountId", transaction.AccountId);
            command.Parameters.AddWithValue("@Category", transaction.Category);
            command.Parameters.AddWithValue("@Icon", transaction.IconId);
            command.Parameters.AddWithValue("@Id", transaction.TransactionId);
            _connection.SqlConnection.Open();
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();

        }


        public void GetTotalBalance(int billId)
        {
            _connection.CheckConnection();
            decimal sum = 0;
            _connection.SqlConnection.Open();
            var command = new SqlCommand("SELECT SUM(Cast(Amount as decimal(18,2))) FROM dbo.[Transaction] WHERE AccountId = @Id", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Id", billId);
            command.ExecuteNonQuery();
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
                    _connection.SqlConnection.Close();
                    UpdateBillBalance(billId, sum);
                }
                
            }
            _connection.SqlConnection.Close();
            UpdateBillBalance(billId, sum);
        }

        public void UpdateBillBalance(int billId, decimal amount)
        {
            _connection.CheckConnection();
            _connection.SqlConnection.Open();
            var command = new SqlCommand("UPDATE dbo.Account SET Balance = @Amount WHERE Id = @Id", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Id", billId);
            command.Parameters.AddWithValue("@Amount", amount);
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
        }

        public ITransaction GetScheduledTransactionById(int id)
        {
            var transaction = new TransactionDto();
            var command = new SqlCommand("SELECT * FROM dbo.[ScheduledTransactions] WHERE Id = @Id",_connection.SqlConnection);
            command.Parameters.AddWithValue("@Id", id);
            _connection.CheckConnection();
            _connection.SqlConnection.Open();
            command.ExecuteNonQuery();
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
            _connection.SqlConnection.Close();
            return transaction;
        }
    }
}
