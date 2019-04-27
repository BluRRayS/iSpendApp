using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace iSpendDAL
{
    public class DatabaseConnection
    {
        public DatabaseConnection(string connectionString)
        {
            SqlConnection = new SqlConnection(connectionString);
        }

        public void CheckConnection()
        {
            if (SqlConnection.State == ConnectionState.Open)
            {
                SqlConnection.Close();
            }
        }

        internal SqlConnection SqlConnection { get; }
    }
}
