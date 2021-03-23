using System;

namespace APIEntity
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public DateTime PublishDate { get; set; }
        public Decimal Price { get; set; }
    }
}
