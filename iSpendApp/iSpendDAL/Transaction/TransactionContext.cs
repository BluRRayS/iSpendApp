using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.InteropServices.ComTypes;
using iSpendDAL.ContextInterfaces;
using iSpendDAL.Dto;
using iSpendInterfaces;

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
            _connection.SqlConnection.Open();
            var billTransactions = new List<TransactionDto>();
            var command = new SqlCommand("SELECT Id,Name,Amount,AccountId,Category,TimeOfTransaction,IconId FROM dbo.[Transaction] WHERE AccountId=@Id",_connection.SqlConnection);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    billTransactions.Add(new TransactionDto(reader.GetInt32(0),reader.GetString(1),reader.GetDecimal(2),reader.GetString(4),reader.GetInt32(6),reader.GetDateTime(5)));
                }
            }
            _connection.SqlConnection.Close();
            return billTransactions;
        }

        public void CreateTransaction(ITransaction transaction)
        {
            _connection.SqlConnection.Open();
            var command = new SqlCommand("INSERT INTO dbo.[Transaction] (Name,Amount,TimeOfTransaction,AccountId,Category,IconId) VALUES(@Name,@Amount,@Time,@AccountId,@Category,@IconId)", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Name",transaction.TransactionName);
            command.Parameters.AddWithValue("@Amount", transaction.TransactionAmount);
            command.Parameters.AddWithValue("@Time",DateTime.Now);
            command.Parameters.AddWithValue("@AccountId",transaction.BillId);
            command.Parameters.AddWithValue("@Category",transaction.Category);
            command.Parameters.AddWithValue("@IconId", transaction.IconId);
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
        }

        public void DeleteTransaction(int id, int billId)
        {
            _connection.SqlConnection.Open();
            var command = new SqlCommand("DELETE FROM dbo.[Transaction] WHERE Id= @Id", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();           
            _connection.SqlConnection.Close();
            GetTotalBalance(billId);
        }

        public void UpdateTransaction(int id,int billId ,ITransaction transaction)
        {
            throw new NotImplementedException();
        }

        public ITransaction GetTransactionById(int id)
        {
            throw new System.NotImplementedException();
        }



        private void GetTotalBalance(int billId)
        {
            decimal sum = 0;
            _connection.SqlConnection.Open();
            var command = new SqlCommand("SELECT SUM(Cast(Amount as decimal(18,2))) FROM dbo.[Transaction] WHERE AccountId = @Id", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Id", billId);
            command.ExecuteNonQuery();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    sum = reader.GetDecimal(0);
                }
            }
            _connection.SqlConnection.Close();
            UpdateBillBalance(billId, sum);
        }

        private void UpdateBillBalance(int billId, decimal amount)
        {
            _connection.SqlConnection.Open();
            var command = new SqlCommand("UPDATE dbo.Account SET Balance = @Amount WHERE Id = @Id", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Id", billId);
            command.Parameters.AddWithValue("@Amount", amount);
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
        }
    }
}
