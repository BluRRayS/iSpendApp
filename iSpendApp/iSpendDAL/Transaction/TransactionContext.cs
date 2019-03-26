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
            command.Parameters.AddWithValue("@AccountId",transaction.AccountId);
            command.Parameters.AddWithValue("@Category",transaction.Category);
            command.Parameters.AddWithValue("@IconId", transaction.IconId);
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
        }

        public void DeleteTransaction(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateTransaction(int id, ITransaction transaction)
        {
            throw new NotImplementedException();
        }

        public ITransaction GetTransactionById(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
