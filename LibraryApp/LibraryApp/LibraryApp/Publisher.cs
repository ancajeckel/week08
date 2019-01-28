using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace LibraryAppModels
{
    public class Publisher
    {
        public int PublisherId { get; set; }

        public string Name { get; set; }

        public Publisher(SqlConnection ctorConnection, string name)
        {
            this.Name = name;
            this.PublisherId = InsertNewPublisherDb(ctorConnection);
        }

        public Publisher(SqlConnection ctorConnection, int publisherId, string name)
        {
            this.PublisherId = publisherId;
            this.Name = name;
        }

        public static string ReadPublisherNameForId(SqlConnection parConnection, int parPublisher)
        {
            //set parameter
            SqlParameter publisherParameter = DbManager.CreateNewParameterByNameAndValue("PublisherId", parPublisher);

            //configure select command
            string selectStatement = "select Name from Publisher where PublisherId = @PublisherId";
            SqlCommand command = new SqlCommand(selectStatement);
            command.Connection = parConnection;
            command.Parameters.Add(publisherParameter);

            //execute command and return scalar value
            return (string)command.ExecuteScalar();
        }

        public static int LoadPublisherIdByName(SqlConnection parConnection, string parName)
        {
            //set parameter
            SqlParameter publisherParameter = DbManager.CreateNewParameterByNameAndValue("PublisherName", parName);

            //configure select command
            string selectStatement = "select PublisherId from Publisher where lower(PublisherName) = lower(@PublisherName)";
            SqlCommand command = new SqlCommand(selectStatement);
            command.Connection = parConnection;
            command.Parameters.Add(publisherParameter);

            //execute command and return scalar value
            return (int)command.ExecuteScalar();
        }

        public int InsertNewPublisherDb(SqlConnection parConnection)
        {
            //set parameter
            SqlParameter publisherParameter = DbManager.CreateNewParameterByNameAndValue("PublisherName", this.Name);

            //configure insert command
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.Connection = parConnection;
            string insertText = @"
                insert into Publisher (Name) 
                  values (@PublisherName);
                select cast(SCOPE_IDENTITY() as int)"; // insert 1rec + info about last affected record
            insertCommand.CommandText = insertText;
            insertCommand.Parameters.Add(publisherParameter);

            return Convert.ToInt32(insertCommand.ExecuteScalar());
        }

        public static int GetCountPublishersFromDb(SqlConnection parConnection)
        {
            return (int)DbManager.ExecuteSqlScalarNoParams(parConnection, "select count(*) from Publisher");
        }

        public static void PrintTopNPublishers(SqlConnection parConnection)
        {
            DbManager.ExecuteAndParseReader(parConnection, "select top 10 PublisherId, Name from Publisher");
        }

        public static void PrintNoBooksPerPublisher(SqlConnection parConnection)
        {
            DbManager.ExecuteAndParseReader(parConnection, @"select p.[Name] as PublisherName, count(b.BookId) as NoBooks
                from publisher p join book b on p.PublisherId = b.PublisherId
               group by p.[Name]
               order by 2 desc, 1");
        }

        public static void PrintTotalBooksPricePerPublisher(SqlConnection parConnection)
        {
            DbManager.ExecuteAndParseReader(parConnection, @"select p.[Name] as PublisherName, sum(b.Price) as NoBooks
                from publisher p join book b on p.PublisherId = b.PublisherId
               group by p.[Name]
               order by 2 desc, 1");
        }

        public static int DeletePublisher(SqlConnection parConnection, int parPublisherId)
        {
            try
            {
                string varStoredProcedure = "DeletePubliser";
                SqlCommand procCommand = new SqlCommand(varStoredProcedure);
                procCommand.Connection = parConnection;
                procCommand.CommandType = System.Data.CommandType.StoredProcedure;
                procCommand.Parameters.Add(DbManager.CreateNewParameterByNameAndValue("PublisherId", parPublisherId));

                return procCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
            finally
            {
                // connection.Close();
            }
        }
    }
}
