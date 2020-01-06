using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using WebAPI_Example2.Models;

namespace WebAPI_Example2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private MySqlDatabase MySqlDb { get; set; }
        public BooksController(MySqlDatabase mySqlDatabase)
        {
            MySqlDb = mySqlDatabase;
        }


        // GET: /books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> Get()
        {
            List<Book> books = new List<Book>();

            MySqlCommand cmd = MySqlDb.Connection.CreateCommand();
            cmd.CommandText = @"SELECT ISBN, Title, Author, Pages, Price, DatePublished, Available FROM book";

            using (DbDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    Book book = new Book
                    {
                        ISBN = reader.GetString(0),
                        Title = reader.GetString(1),
                        Author = reader.GetString(2),
                        Pages = reader.GetInt32(3),
                        Price = reader.GetDecimal(4),
                        DatePublished = reader.GetDateTime(5),
                        Available = reader.GetBoolean(6)
                    };
                    books.Add(book);
                }
            }

            return Ok(books);
        }

        // GET: /books/978-8804668237
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            MySqlCommand cmd = MySqlDb.Connection.CreateCommand();
            cmd.CommandText = @"SELECT ISBN, Title, Author, Pages, Price, DatePublished, Available FROM book WHERE ISBN=@isbn";
            cmd.Parameters.AddWithValue("@isbn", id);

            using (DbDataReader reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    Book book = new Book
                    {
                        ISBN = reader.GetString(0),
                        Title = reader.GetString(1),
                        Author = reader.GetString(2),
                        Pages = reader.GetInt32(3),
                        Price = reader.GetDecimal(4),
                        DatePublished = reader.GetDateTime(5),
                        Available = reader.GetBoolean(6)
                    };
                    return Ok(book);
                }
            }
            return NotFound("");
        }

        // POST: /books
        // BODY
        //{
        //    "isbn": "978-8804668237",
        //    "title": "1984",
        //    "author": "George Orwell",
        //    "pages": 333,
        //    "price": 11.90,
        //    "datePublished": "2016-06-21T00:00:00Z",
        //    "available": true
        //}
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Book newBook)
        {

            ActionResult<Book> checkBook = await Get(newBook.ISBN);
            if (checkBook.Result == null)
            {
                return NotFound("");
            }
            else
            {
                MySqlCommand cmd = MySqlDb.Connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO book (ISBN, Title, Author, Pages, Price, DatePublished, Available) VALUES (@isbn, @title, @author, @pages, @price, @datepublished, @available);";
                cmd.Parameters.AddWithValue("@isbn", newBook.ISBN);
                cmd.Parameters.AddWithValue("@title", newBook.Title);
                cmd.Parameters.AddWithValue("@author", newBook.Author);
                cmd.Parameters.AddWithValue("@pages", newBook.Pages);
                cmd.Parameters.AddWithValue("@price", newBook.Price);
                cmd.Parameters.AddWithValue("@datepublished", newBook.DatePublished);
                cmd.Parameters.AddWithValue("@available", newBook.Available);

                int result = await cmd.ExecuteNonQueryAsync();

                if (result == 1) { return Created(newBook.ISBN, newBook); }
                else
                {
                    return NotFound("");
                }
            }
        }

        // PUT: /books/978-8804668237
        // BODY
        //{
        //    "title": "1984 (2)",
        //    "author": "George Orwell (2)",
        //    "pages": 3332,
        //    "price": 112.90,
        //    "datePublished": "2020-06-21T00:00:00Z",
        //    "available": false
        //}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Book updatedBook)
        {
            ActionResult<Book> checkBook = await Get(id);
            if (checkBook.Result != null)
            {
                return NotFound("");
            }
            else
            {
                MySqlCommand cmd = MySqlDb.Connection.CreateCommand();
                cmd.CommandText = @"UPDATE book SET Title = @title, Author = @author, Pages = @pages, Price = @price, DatePublished = @datepublished, Available = @available WHERE ISBN = @isbn;";
                cmd.Parameters.AddWithValue("@isbn", id);

                cmd.Parameters.AddWithValue("@title", updatedBook.Title);
                cmd.Parameters.AddWithValue("@author", updatedBook.Author);
                cmd.Parameters.AddWithValue("@pages", updatedBook.Pages);
                cmd.Parameters.AddWithValue("@price", updatedBook.Price);
                cmd.Parameters.AddWithValue("@datepublished", updatedBook.DatePublished);
                cmd.Parameters.AddWithValue("@available", updatedBook.Available);

                int result = await cmd.ExecuteNonQueryAsync();

                if (result == 1) {
                    return NoContent(); // 204 
                }
                else
                {
                    return NotFound("");
                }               
            }    
        }

        // DELETE: /books/978-8804668237
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            MySqlCommand cmd = MySqlDb.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM book WHERE ISBN=@isbn";
            cmd.Parameters.AddWithValue("@isbn", id);
            int result = await cmd.ExecuteNonQueryAsync();
            if (result == 1) { return Ok(); }
            else
            {
                return NotFound("");
            }
        }
    }
}
