using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace LibraryAppModels
{
    public class Book
    {
        public int BookId { get; set; }

        public string Title { get; set; }

        public Publisher PublisherId { get; set; }

        public int Year { get; set; }

        public decimal Price { get; set; }

        public Book(SqlConnection ctorConnection, string title, Publisher publisherId, int year, decimal price)
        {
            this.Title = title;
            this.PublisherId = publisherId;
            this.Year = year;
            this.Price = price;

            this.BookId = InsertNewBookDb(ctorConnection);
        }

        public Book(SqlConnection ctorConnection, int bookId)
        {
            this.BookId = bookId;
        }

        public int InsertNewBookDb(SqlConnection parConnection)
        {
            //set parameters
            SqlParameter titleParameter = DbManager.CreateNewParameterByNameAndValue("BookTitle", this.Title);
            SqlParameter publisherParameter = DbManager.CreateNewParameterByNameAndValue("BookPublisher", this.PublisherId.PublisherId);
            SqlParameter yearParameter = DbManager.CreateNewParameterByNameAndValue("BookYear", this.Year);
            SqlParameter priceParameter = DbManager.CreateNewParameterByNameAndValue("BookPrice", this.Price);

            //configure insert command
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.Connection = parConnection;
            string insertText = @"
                insert into Book (Title, PublisherId, [Year], Price) 
                  values (@BookTitle, @BookPublisher, @BookYear, @BookPrice);
                select cast(SCOPE_IDENTITY() as int)";
            insertCommand.CommandText = insertText;
            insertCommand.Parameters.Add(titleParameter);
            insertCommand.Parameters.Add(publisherParameter);
            insertCommand.Parameters.Add(yearParameter);
            insertCommand.Parameters.Add(priceParameter);

            //execute command
            return Convert.ToInt32(insertCommand.ExecuteScalar());
        }

        public int UpdateBookDb(SqlConnection parConnection, string newTitle, int newYear, decimal newPrice)
        {
            SqlParameter bookidParameter = DbManager.CreateNewParameterByNameAndValue("BookId", this.BookId);
            SqlParameter titleParameter = DbManager.CreateNewParameterByNameAndValue("BookTitle", newTitle);
            SqlParameter yearParameter = DbManager.CreateNewParameterByNameAndValue("BookYear", newYear);
            SqlParameter priceParameter = DbManager.CreateNewParameterByNameAndValue("BookPrice", newPrice);

            //configure update command
            SqlCommand updateCommand = new SqlCommand();
            updateCommand.Connection = parConnection;
            string updateText = @"update book set Title = @BookTitle, [Year] = @BookYear, Price = @BookPrice where BookId = @BookId";
            updateCommand.CommandText = updateText;
            updateCommand.Parameters.Add(bookidParameter);
            updateCommand.Parameters.Add(titleParameter);
            updateCommand.Parameters.Add(yearParameter);
            updateCommand.Parameters.Add(priceParameter);

            //execute command and return number of rows affected
            return Convert.ToInt32(updateCommand.ExecuteNonQuery());
        }

        public int DeleteBookDb(SqlConnection parConnection)
        {
            //set parameters
            SqlParameter bookidParameter = DbManager.CreateNewParameterByNameAndValue("BookId", this.BookId);

            //configure insert command
            SqlCommand deleteCommand = new SqlCommand();
            deleteCommand.Connection = parConnection;
            string deleteText = @"delete from book where BookId = @BookId";
            deleteCommand.CommandText = deleteText;
            deleteCommand.Parameters.Add(bookidParameter);

            //execute command
            return Convert.ToInt32(deleteCommand.ExecuteNonQuery());
        }

        public void SelectBookDb(SqlConnection parConnection)
        {
            //set parameters
            SqlParameter bookidParameter = DbManager.CreateNewParameterByNameAndValue("BookId", this.BookId);

            //select command
            SqlCommand selectCommand = new SqlCommand(@"
                select BookId, Title, PublisherId, [Year], Price 
                  from Book where BookId = @BookId");
            selectCommand.Connection = parConnection;
            selectCommand.Parameters.Add(bookidParameter);

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
    }
}
