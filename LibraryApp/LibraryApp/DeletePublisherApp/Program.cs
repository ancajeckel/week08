using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using LibraryAppModels;

namespace DeletePublisherApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //create and open db connection
            SqlConnection myConnection = DbManager.ConnectToLocalDb("Library2");

            //read the publisher id from console
            Console.WriteLine("Please insert Id of a publisher to delete:");
            int ret = Publisher.DeletePublisher(myConnection, Convert.ToInt32(Console.ReadLine()));
            Console.WriteLine($"{ret} record(s) were deleted.");

            //close connection
            DbManager.CloseConnection(myConnection);
            Console.ReadKey();
        }
    }
}
