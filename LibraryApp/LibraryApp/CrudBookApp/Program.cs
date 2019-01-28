using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using LibraryAppModels;

namespace CrudBookApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //create and open db connection
            SqlConnection myConnection = DbManager.ConnectToLocalDb("Library2");

            //insert new book based on existing PublisherId
            Publisher publi3 = new Publisher(myConnection, Publisher.LoadPublisherIdByName(myConnection, "Litera"), "Litera");
            publi3.PublisherId = 3;

            Book bookNew = new Book(myConnection, "Homo Sapiens", publi3, 2017, 65);
            Console.WriteLine($"Book inserted with Id:{bookNew.BookId}");

            //update a book (read id from Console)
            Console.WriteLine("Please insert Id of book to update:");
            Book bookUpd = new Book(myConnection, Convert.ToInt32(Console.ReadLine()));
            var updRecords = bookUpd.UpdateBookDb(myConnection, "New Title", 2015, 100);
            Console.WriteLine($"{updRecords} record(s) were affected");

            //delete a book (read id from Console)
            Console.WriteLine("Please insert Id of book to delete:");
            Book bookDel = new Book(myConnection, Convert.ToInt32(Console.ReadLine()));
            Console.WriteLine($"{bookDel.DeleteBookDb(myConnection)} record(s) were deleted");

            //select a book (read id from Console)
            Console.WriteLine("Please insert Id of book to select:");
            Book bookSel = new Book(myConnection, Convert.ToInt32(Console.ReadLine()));
            bookSel.SelectBookDb(myConnection);

            //close connection
            DbManager.CloseConnection(myConnection);
            Console.ReadKey();
        }
    }
}
