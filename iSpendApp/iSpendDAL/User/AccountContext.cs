using System;
using System.Data.SqlClient;
using iSpendDAL.ContextInterfaces;
using iSpendDAL.Dto;
using iSpendInterfaces;

namespace iSpendDAL.User
{
    public class UserContext : IUserContext
    {
        private readonly DatabaseConnection _connection;

        public UserContext(DatabaseConnection connection)
        {
            _connection = connection;
        }

        public void AddUser(IUser account)
        {
            _connection.SqlConnection.Open();
            var command = new SqlCommand("INSERT INTO [User](username,password,email,dateOfCreation) VALUES(@Username,@Password,@Email,@Time)",_connection.SqlConnection);
            command.Parameters.AddWithValue("@Username",account.Username);
            command.Parameters.AddWithValue("@Password",account.Password);
            command.Parameters.AddWithValue("@Email",account.Email);
            command.Parameters.AddWithValue("@Time",DateTimeOffset.Now);
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
        }

        public bool CheckIfUserNameIsTaken(string username)
        {
            var taken =false;
            _connection.SqlConnection.Open();
            var command = new SqlCommand("SELECT username FROM [User] WHERE username=@Username", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Username", username);
            command.ExecuteNonQuery();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read() == true)
                {
                    taken = true;
                }
            }
            _connection.SqlConnection.Close();
            return taken;
        }

        public IUser GetAccountByUsername(string username)
        {
            var Account = new UserDto();
            _connection.SqlConnection.Open();
            var command = new SqlCommand("SELECT Id,username,password,email,dateOfCreation  FROM [User] WHERE username=@Username", _connection.SqlConnection);
            command.Parameters.AddWithValue("@Username",username);
            command.ExecuteNonQuery();
            using (var reader = command.ExecuteReader())
            {
                    if (reader.Read() == true)
                    {
                        Account.UserId = reader.GetInt32(0);
                        Account.Username = reader.GetString(1);
                        Account.Password = reader.GetString(2);
                        Account.Email = reader.GetString(3);
                        Account.DateOfCreation = reader.GetDateTime(4);
                    }
            }
            _connection.SqlConnection.Close();
            return Account;
        }

        public IUser GetAccountById(int userId)
        {
            var account = new UserDto();
            _connection.SqlConnection.Open();
            var command = new SqlCommand("SELECT Id,username,password,email,dateOfCreation  FROM dbo.User WHERE Id=" + userId + "", _connection.SqlConnection);
            command.ExecuteNonQuery();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read() == true)
                {
                    account.UserId = reader.GetInt32(0);
                    account.Username = reader.GetString(1);
                    account.Password = reader.GetString(2);
                    account.Email = reader.GetString(3);
                    account.DateOfCreation = reader.GetDateTime(4);
                }
            }
            _connection.SqlConnection.Close();
            return account;
        }

        public bool CheckCredentials(string username, string password)
        {
            var correct = false;
            _connection.SqlConnection.Open();
            var command = new SqlCommand("SELECT username,password FROM [User] WHERE username=@Username AND password=@Password",_connection.SqlConnection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password",password);
            command.ExecuteNonQuery();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read() == true)
                {
                    correct = true;
                }
            }
            _connection.SqlConnection.Close();
            return correct;
        }


        public void UpdateUserDetails(IUser account)
        {
            _connection.SqlConnection.Open();
            var command = new SqlCommand("UPDATE [User] SET username= @Username, password=@Password, email=@Email  WHERE Id=@UserId ", _connection.SqlConnection);
            command.Parameters.AddWithValue("@UserId", account.UserId);
            command.Parameters.AddWithValue("@Username", account.Username);
            command.Parameters.AddWithValue("@Password", account.Password);
            command.Parameters.AddWithValue("@Email", account.Email);
            command.ExecuteNonQuery();
            _connection.SqlConnection.Close();
        }
    }
}
