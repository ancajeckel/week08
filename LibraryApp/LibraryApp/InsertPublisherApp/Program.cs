using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using LibraryAppModels;

namespace InsertPublisherApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //create and open db connection
            SqlConnection myConnection = DbManager.ConnectToLocalDb("Library2");

            //read new publisher -> scalar query
            string outPublisherName = Publisher.ReadPublisherNameForId(myConnection, 1);
            Console.WriteLine($"Publisher name for id 1 is: {outPublisherName}");

            //insert new publisher
            Console.WriteLine("Insert new publisher with name: ");
            Publisher publi1 = new Publisher(myConnection, Console.ReadLine());
            Console.WriteLine($"Publisher inserted with Id:{publi1.PublisherId}");

            //close connection
            DbManager.CloseConnection(myConnection);
            Console.ReadKey();
        }
    }
}
