using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_Example3.Models;

namespace WebAPI_Example3.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        private readonly BookContext _context;
        public BooksController(BookContext context)
        {
            _context = context;
        }

        // GET: /books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> Get()
        {
            List<Book> allBooks = await _context.Books.ToListAsync();
            return Ok(allBooks);
        }

        // GET: /books/978-8804668237
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            Book book = await _context.Books.FindAsync(id);
            if (book == null) { return NotFound(""); }
            return Ok(book);
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
            Book book = await _context.Books.FindAsync(newBook.ISBN);
            if (book != null) { return NotFound(""); }
            _context.Books.Add(newBook);
            try
            {
                await _context.SaveChangesAsync();
                return Created(newBook.ISBN, newBook);
            }
            catch
            {
                return NotFound("");
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
            updatedBook.ISBN = id;
            _context.Entry(updatedBook).State = EntityState.Modified;
            _context.Entry(updatedBook).Property(x => x.Price).IsModified = false;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            { return NotFound(""); }

            return NoContent(); //204
        }

        // DELETE: /books/978-8804668237
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Book book = await _context.Books.FindAsync(id);
            if (book == null) { return NotFound(""); }
            else
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return Ok();
            }
        }
    }
}
