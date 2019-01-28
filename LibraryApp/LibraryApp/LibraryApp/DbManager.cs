using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace LibraryAppModels
{
    public static class DbManager
    {
        public static SqlConnection ConnectToLocalDb(string localDbName)
        {
            string connectionString = "Data Source=.;Initial Catalog=" + localDbName + ";Integrated Security=True";
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;

            //open connection
            connection.Open();

            Console.WriteLine($"Connection to local db: {localDbName} has been opened.");

            return connection;
        }

        public static void CloseConnection(SqlConnection connection)
        {
            connection.Close();

            Console.WriteLine($"Connection ({connection.ConnectionString}) has been closed.");
        }

        public static object ExecuteSqlScalarNoParams(SqlConnection connection, string parSelect)
        {
            //configure select command
            string selectStatement = parSelect;
            SqlCommand command = new SqlCommand(selectStatement);
            command.Connection = connection;

            //execute command and return scalar value
            return command.ExecuteScalar();
        }

        public static void ExecuteAndParseReader(SqlConnection parConnection, string parSelect)
        {
            SqlCommand selectCommand = new SqlCommand(parSelect);
            selectCommand.Connection = parConnection;

            SqlDataReader reader = selectCommand.ExecuteReader();

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write($"{reader[i]} ");
                }
                Console.WriteLine();
            }

            reader.Close();
        }

        public static SqlParameter CreateNewParameterByNameAndValue(string parName, object parValue)
        {
            SqlParameter outParam = new SqlParameter();
            outParam.ParameterName = parName;
            outParam.Value = parValue;

            return outParam;
        }
    }
}
