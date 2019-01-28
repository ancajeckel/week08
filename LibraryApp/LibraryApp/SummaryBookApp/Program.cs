using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using LibraryAppModels;

namespace SummaryBookApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //create and open db connection
            SqlConnection myConnection = DbManager.ConnectToLocalDb("Library2");

            //get all the books that are published in 2010
            Console.WriteLine("Get all the books that are published in 2010");
            DbManager.ExecuteAndParseReader(myConnection, @"
              select BookId, Title, PublisherId, [Year], Price
                from book
               where [Year] = 2010
               order by 1");

            //get the book that is published in the max year (can use multiple commands)
            Console.WriteLine("Get the book that is published in the max year");
            DbManager.ExecuteAndParseReader(myConnection, @"
              select BookId, Title, PublisherId, [Year], Price
                from book
               where [Year] = (select max([Year]) from Book)
               order by 1");

            //get Top 10 books (Title, Year, Price)
            Console.WriteLine("Get Top 10 books (Title, Year, Price)");
            DbManager.ExecuteAndParseReader(myConnection, @"
              select top 10 Title, [Year], Price
                from book
               order by 1");

            //close connection
            DbManager.CloseConnection(myConnection);
            Console.ReadKey();
        }
    }
}
