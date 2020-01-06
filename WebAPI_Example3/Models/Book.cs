using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_Example3.Models
{
    [Table("book")]
    public class Book
    {
        [Key]
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Pages { get; set; }
        public decimal Price { get; set; }
        public DateTime DatePublished { get; set; }
        public bool Available { get; set; }
    }
}
