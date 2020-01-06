using System;

namespace WebAPI_Example2.Models
{
    public class Book
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Pages { get; set; }
        public decimal Price { get; set; }
        public DateTime DatePublished { get; set; }
        public bool Available { get; set; }
    }
}
