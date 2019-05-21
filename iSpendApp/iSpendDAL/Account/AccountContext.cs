using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using iSpendDAL.ContextInterfaces;
using iSpendDAL.Dto;
using iSpendInterfaces;

namespace iSpendDAL.Account
{
    public class AccountContext:IAccountContext
    {

        private readonly DatabaseConnection _connection;

        public AccountContext(DatabaseConnection connection)
        {
            _connection = connection;
        }

        public void AddAccount(IAccount newAccount,int userId)
        {
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("INSERT INTO dbo.Account (Name,Balance,DateOfCreation,IconId) VALUES(@Name,@Balance,@DateOfCreation,@IconId)", connection);
                command.Parameters.AddWithValue("@Name", newAccount.AccountName);
                command.Parameters.AddWithValue("@Balance", newAccount.AccountBalance);
                command.Parameters.AddWithValue("@DateOfCreation", DateTime.Now);
                command.Parameters.AddWithValue("@IconId", newAccount.IconId);
                var command2 = new SqlCommand("INSERT INTO dbo.User_Account (UserId,AccountId) VALUES(@UserId,(SELECT Max(Id) From Account)) ", connection);
                command2.Parameters.AddWithValue("@UserId", userId);
                connection.Open();
                command.ExecuteNonQuery();
                command2.ExecuteNonQuery();
                connection.Close();
            }
            AddStartTransaction(Convert.ToDecimal(newAccount.AccountBalance));
        }

        public void RemoveAccount(int accountId)
        {
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("DELETE FROM dbo.Account WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", accountId);
                connection.Open();
                command.ExecuteNonQuery();            
                connection.Close();
            }
        }

        public void UpdateAccount(int id, string name, int iconId)
        {
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("UPDATE dbo.Account SET dbo.Account.Name = @Name , dbo.Account.IconId = @IconId WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@IconId", iconId);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public IEnumerable<IAccount> GetAccountsByUsername(string username)
        {
            var userBills = new List<IAccount>();
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT * FROM [Account]  INNER JOIN dbo.User_Account ON dbo.Account.Id = dbo.User_Account.AccountId  AND dbo.User_Account.UserId = (SELECT Id FROM dbo.[User] WHERE UserName = @Username)", connection);
                command.Parameters.AddWithValue("@Username", username);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userBills.Add(new AccountDto(reader.GetInt32(0), reader.GetString(1), Convert.ToDouble(reader.GetDecimal(2)), reader.GetDateTime(3), reader.GetInt32(4)));
                    }
                }
                connection.Close();
            }
            return userBills;
        }

        public IAccount GetAccountById(int billId)
        {
            var account = new AccountDto();
            using (var connection = _connection.SqlConnection ) 
            {
                var command = new SqlCommand("SELECT * FROM dbo.Account WHERE Id = @BillId", connection);
                command.Parameters.AddWithValue("@BillId", billId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        account.AccountId = reader.GetInt32(0);
                        account.AccountName = reader.GetString(1);
                        account.AccountBalance = Convert.ToDouble(reader.GetDecimal(2));
                        account.IconId = reader.GetInt32(4);
                    }
                }
                connection.Close();
            }
            return account;
        }

        public IEnumerable<ITransaction> GetAccountTransactions(int billId)
        {
            throw new NotImplementedException();
        }

        public decimal GetTotalBalance(int billId)
        {
            decimal sum = 0;
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT SUM(Cast(Amount as decimal(18,2))) FROM dbo.[Transaction] WHERE AccountId = @Id", connection);
                command.Parameters.AddWithValue("@Id", billId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        sum = reader.GetDecimal(0);
                    }
                }
                connection.Close();
            }
            UpdateAccountBalance(billId,sum);
            return sum;
        }

        public void AddReservation(IReservation reservation)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IReservation> GetReservations(int accountId)
        {
            var reservations = new List<IReservation>();
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT ReservationId,AccountId,SavingsId,Amount,[Date] FROM dbo.Reservations WHERE AccountId= @Id", connection);
                command.Parameters.AddWithValue("@Id", accountId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reservations.Add(new ReservationDto(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetDecimal(3), reader.GetDateTime(4)));
                    }
                }
                connection.Close();
            }
            return reservations;
        }

        public void UpdateAccountBalance(int billId, decimal amount)
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

        public IEnumerable<IUser> GetAccountUsers(int billId)
        {
            var users = new List<UserDto>();
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT dbo.[User].Id, UserName FROM dbo.[User] INNER JOIN dbo.[User_Account] ON dbo.[User].Id = dbo.[User_Account].UserId WHERE dbo.User_Account.AccountId=@Id", connection);
                command.Parameters.AddWithValue("@Id", billId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new UserDto(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
                connection.Close();
            }
            return users;
        }

        public void AddStartTransaction(decimal amount)
        {
            using (var connection = _connection.SqlConnection)
            {
                var command =
                    new SqlCommand(
                        "INSERT INTO dbo.[Transaction] (Name,Amount,TimeOfTransaction,AccountId,Category,IconId) VALUES(@Name,@Amount,@Time,(SELECT MAX(Id) FROM dbo.[Account]),@Category,@IconId)",
                        connection);
                command.Parameters.AddWithValue("@Name", "Starting Funds");
                command.Parameters.AddWithValue("@Amount", amount);
                command.Parameters.AddWithValue("@Time", DateTime.Now);
                command.Parameters.AddWithValue("@Category", "Start");
                command.Parameters.AddWithValue("@IconId", 0);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }

}
