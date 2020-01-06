using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI_Example1.Models;

namespace WebAPI_Example1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private static List<Book> Books = new List<Book>
        {
            new Book
            {
                ISBN = "978-8804668237",
                Title = "1984",
                Author = "George Orwell",
                Pages = 333,
                Price = 11.90m,
                DatePublished = new DateTime(2016, 6, 21, 0, 0, 0, DateTimeKind.Utc),
                Available = true
            },

            new Book
            {
                ISBN = "978-8804665298",
                Title = "Fahrenheit 451",
                Author = "Ray Bradbury",
                Pages = 177,
                Price = 10.20m,
                DatePublished = new DateTime(2016, 5, 10, 0, 0, 0, DateTimeKind.Utc),
                Available = true
            },

            new Book
            {
                ISBN = "978-8807900587",
                Title = "Il ritratto di Dorian Gray",
                Author = "Oscar Wilde",
                Pages = 261,
                Price = 6.80m,
                DatePublished = new DateTime(2013, 6, 5, 0, 0, 0, DateTimeKind.Utc),
                Available = false
            }
        };

        // GET: /books
        [HttpGet]
        public ActionResult<IEnumerable<Book>> AllBooks()
        {
            return Ok(Books);
        }

        // GET: /books/978-8804668237
        [HttpGet("{id}")]
        public ActionResult<Book> Get(string id)
        {
            try
            {
                return Ok(Books.Single(t => t.ISBN == id));
            }
            catch
            {
                return NotFound("");
            }
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
        public IActionResult Post([FromBody] Book newBook)
        {
            int currentIndex = Books.FindIndex(t => t.ISBN == newBook.ISBN);
            if (currentIndex >= 0)
            {
                return NotFound("");
            }
            else
            {
                Books.Add(newBook);
                return Created(newBook.ISBN, newBook);
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
        public IActionResult Put(string id, [FromBody] Book updatedBook)
        {
            int currentIndex = Books.FindIndex(t => t.ISBN == id);
            if (currentIndex >= 0)
            {
                Books[currentIndex].Title = updatedBook.Title;
                Books[currentIndex].Author = updatedBook.Author;
                Books[currentIndex].Pages = updatedBook.Pages;
                Books[currentIndex].Price = updatedBook.Price;
                Books[currentIndex].DatePublished = updatedBook.DatePublished;
                Books[currentIndex].Available = updatedBook.Available;
                return NoContent(); // 204
            }
            else
            {
                return NotFound("");
            }
        }

        // DELETE: /books/978-8804668237
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {

            int currentIndex = Books.FindIndex(t => t.ISBN == id);
            if (currentIndex >= 0)
            {
                Books.RemoveAt(currentIndex);
                return Ok();
            }
            else
            {
                return NotFound("");
            }
        }
    }
}