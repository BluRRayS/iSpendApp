using System.Data;
using System.Data.SqlClient;

namespace iSpendDAL
{
    public class DatabaseConnection
    {
        private readonly string _connectionString;
        public DatabaseConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        internal SqlConnection SqlConnection => new SqlConnection(_connectionString);
    }
}
