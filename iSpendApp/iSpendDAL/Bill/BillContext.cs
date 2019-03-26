using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using iSpendDAL.ContextInterfaces;
using iSpendDAL.Dto;
using iSpendInterfaces;

namespace iSpendDAL.Bill
{
    public class BillContext:IBillContext
    {
        private List<IBill> _userBills = new List<IBill>();
        private readonly DatabaseConnection _connection;

        public BillContext(DatabaseConnection connection)
        {
            _connection = connection;
        }

        public void AddBill(IBill newBill,int userId)//Todo: Add transaction instead of adding balance directly
        //ToDo: test SubQuery second command.
        {
            _connection.SqlConnection.Open();
            var command = new SqlCommand("INSERT INTO dbo.Account (Name,Balance,DateOfCreation) VALUES(@Name,@Balance,@DateOfCreation)",_connection.SqlConnection);
            command.Parameters.AddWithValue("@Name",newBill.BillName);
            command.Parameters.AddWithValue("@Balance", newBill.BillBalance);
            command.Parameters.AddWithValue("@DateOfCreation",DateTime.Now);
            command.ExecuteNonQuery();
            command.Parameters.Clear();
            command = new SqlCommand("INSERT INTO dbo.User_Account (UserId,AccountId) VALUES(@UserId,(SELECT Max(Id) From Account)) ", _connection.SqlConnection);
            command.Parameters.AddWithValue("@UserId",userId);
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
            AddStartTransaction(Convert.ToDecimal(newBill.BillBalance));
        }

        public void RemoveBill(int billId)
        {
            _connection.SqlConnection.Open();
            var command = new SqlCommand("DELETE FROM dbo.Account WHERE Id = @Id  DELETE FROM dbo.User_Account WHERE AccountId = @Id ", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Id", billId);
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
        }

        public void UpdateBill(int id, string name, int iconId)
        {
           _connection.SqlConnection.Open();
           var command = new SqlCommand("UPDATE dbo.Account SET dbo.Account.Name = @Name , dbo.Account.IconId = @IconId WHERE Id = @Id", _connection.SqlConnection);
           command.Parameters.AddWithValue("@Name",name);
           command.Parameters.AddWithValue("@IconId",iconId);
           command.Parameters.AddWithValue("@Id",id);
           command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
        }

        public IEnumerable<IBill> GetBillsByUsername(string username)
        {
            _userBills.Clear();
            _connection.SqlConnection.Open();
            var command = new SqlCommand("SELECT * FROM [Account] INNER JOIN dbo.User_Account ON dbo.Account.Id = dbo.User_Account.AccountId  AND dbo.User_Account.UserId = (SELECT Id FROM dbo.[User] WHERE UserName = @Username)", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Username", username);
            command.ExecuteNonQuery();
            using (var reader = command.ExecuteReader())
            {
                _userBills = new List<IBill>();
                while (reader.Read())
                {
                    _userBills.Add(new BillDto(reader.GetInt32(0), reader.GetString(1),Convert.ToDouble(reader.GetDecimal(2)),reader.GetDateTime(3)));
                }
            }
            _connection.SqlConnection.Close();
            return _userBills;
        }

        public IBill GetBillById(int billId)
        {
            var bill = new BillDto();
            _connection.SqlConnection.Open();
            var command = new SqlCommand("SELECT * FROM dbo.Account WHERE Id = @BillId",_connection.SqlConnection);
            command.Parameters.AddWithValue("@BillId", billId);
            command.ExecuteNonQuery();
            using (var reader = command.ExecuteReader())
            {                
                if (reader.Read())
                {
                    bill.BillId = reader.GetInt32(0);
                    bill.BillName = reader.GetString(1);
                    bill.BillBalance = Convert.ToDouble( reader.GetDecimal(2));
                    bill.IconId = reader.GetInt32(4);
                }
            }
            _connection.SqlConnection.Close();
            return bill;
        }

        public IEnumerable<ITransaction> GetBillTransactions(int billId)
        {
            throw new NotImplementedException();
        }

        public decimal GetTotalBalance(int billId)
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
            UpdateBillBalance(billId,sum);
            return sum;
        }

        public void UpdateBillBalance(int billId, decimal amount)
        {
            _connection.SqlConnection.Open();
            var command = new SqlCommand("UPDATE dbo.Account SET Balance = @Amount WHERE Id = @Id", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Id", billId);
            command.Parameters.AddWithValue("@Amount", amount);
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
        }

        public void AddStartTransaction(decimal amount)
        {
            _connection.SqlConnection.Open();
            var command = new SqlCommand("INSERT INTO dbo.[Transaction] (Name,Amount,TimeOfTransaction,AccountId,Category,IconId) VALUES(@Name,@Amount,@Time,(SELECT MAX(Id) FROM dbo.[Account]),@Category,@IconId)", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Name", "Starting Funds");
            command.Parameters.AddWithValue("@Amount", amount);
            command.Parameters.AddWithValue("@Time", DateTime.Now);
            command.Parameters.AddWithValue("@Category", "Start");
            command.Parameters.AddWithValue("@IconId", 0);
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
        }
    }

}
