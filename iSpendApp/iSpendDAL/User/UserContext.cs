using System;
using System.Collections.Generic;
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
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("INSERT INTO [User](username,password,email,dateOfCreation) VALUES(@Username,@Password,@Email,@Time)", connection);
                command.Parameters.AddWithValue("@Username", account.Username);
                command.Parameters.AddWithValue("@Password", account.Password);
                command.Parameters.AddWithValue("@Email", account.Email);
                command.Parameters.AddWithValue("@Time", DateTimeOffset.Now);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public bool CheckIfUserNameIsTaken(string username)
        {
            var taken =false;
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT username FROM [User] WHERE username=@Username", connection);
                command.Parameters.AddWithValue("@Username", username);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read() == true)
                    {
                        taken = true;
                    }
                }
                connection.Close();

            }
            return taken;
        }

        public IUser GetAccountByUsername(string username)
        {
            var account = new UserDto();
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT Id,username,password,email,dateOfCreation  FROM [User] WHERE username=@Username", connection);
                command.Parameters.AddWithValue("@Username", username);
                connection.Open();
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
                connection.Close();
            }
            return account;
        }

        public IUser GetAccountById(int userId)
        {
            var account = new UserDto();
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT Id,username,password,email,dateOfCreation  FROM dbo.User WHERE Id=" + userId + "", connection);
                connection.Open();
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
                connection.Close();
            }
            return account;
        }

        public bool CheckCredentials(string username, string password)
        {
            var correct = false;
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT username,password FROM [User] WHERE username=@Username AND password=@Password", connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read() == true)
                    {
                        correct = true;
                    }
                }
                connection.Close();
            }
            return correct;
        }


        public void UpdateUserDetails(IUser account)
        {
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("UPDATE [User] SET username= @Username, password=@Password, email=@Email  WHERE Id=@UserId ", connection);
                command.Parameters.AddWithValue("@UserId", account.UserId);
                command.Parameters.AddWithValue("@Username", account.Username);
                command.Parameters.AddWithValue("@Password", account.Password);
                command.Parameters.AddWithValue("@Email", account.Email);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public IEnumerable<IUser> GetAllUsers()
        {
            var users = new List<IUser>();
            using (var connection = _connection.SqlConnection)
            {
                var command = new SqlCommand("SELECT Id,UserName,Email FROM dbo.[User]", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new UserDto(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                    }

                }
                connection.Close();
            }
            return users;
        }
    }
}
