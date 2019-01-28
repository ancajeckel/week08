using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using LibraryAppModels;

namespace SummaryPublisherApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //create and open db connection
            SqlConnection myConnection = DbManager.ConnectToLocalDb("Library2");

            //get number of publishers from db
            Console.WriteLine($"Number of publishers in db: {Publisher.GetCountPublishersFromDb(myConnection)}");

            //get top 10 publishers
            Console.WriteLine("Top 10 Publishers:");
            Publisher.PrintTopNPublishers(myConnection);

            //number of books for each publisher
            Console.WriteLine("Number of books for each publisher:");
            Publisher.PrintNoBooksPerPublisher(myConnection);

            //the total price for books for a publisher
            Console.WriteLine("The total price for books per publisher:");
            Publisher.PrintTotalBooksPricePerPublisher(myConnection);

            //close connection
            DbManager.CloseConnection(myConnection);
            Console.ReadKey();
        }
    }
}
