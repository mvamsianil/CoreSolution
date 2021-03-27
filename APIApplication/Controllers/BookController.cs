using APIEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIApplication.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("coreapi/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private List<Book> objBooks;
        /// <summary>
        /// 
        /// </summary>
        public BookController()
        {
            if (objBooks == null || objBooks.Count == 0)
            {
                objBooks = new List<Book>();
                objBooks.Add(new Book()
                {
                    BookId = 1,
                    Title = "Midnight Rain",
                    Genre = "Fantasy",
                    PublishDate = new DateTime(2000, 12, 16),
                    AuthorId = 1,
                    Description = "A former architect battles an evil sorceress.",
                    Price = 14.95M
                });

                objBooks.Add(new Book()
                {
                    BookId = 2,
                    Title = "Maeve Ascendant",
                    Genre = "Fantasy",
                    PublishDate = new DateTime(2000, 11, 17),
                    AuthorId = 2,
                    Description = "After the collapse of a nanotechnology society, the young" + "survivors lay the foundation for a new society.",
                    Price = 12.95M
                });

                objBooks.Add(new Book()
                {
                    BookId = 3,
                    Title = "The Sundered Grail",
                    Genre = "Fantasy",
                    PublishDate = new DateTime(2001, 09, 10),
                    AuthorId = 2,
                    Description = "The two daughters of Maeve battle for control of England.",
                    Price = 12.95M
                });

                objBooks.Add(new Book()
                {
                    BookId = 4,
                    Title = "Lover Birds",
                    Genre = "Romance",
                    PublishDate = new DateTime(2000, 09, 02),
                    AuthorId = 3,
                    Description = "When Carla meets Paul at an ornithology conference, tempers fly.",
                    Price = 7.99M
                });

                objBooks.Add(new Book()
                {
                    BookId = 5,
                    Title = "Splish Splash",
                    Genre = "Romance",
                    PublishDate = new DateTime(2000, 11, 02),
                    AuthorId = 4,
                    Description = "A deep sea diver finds true love 20,000 leagues beneath the sea.",
                    Price = 6.99M
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("ListOfBooks"), HttpGet]
        public IEnumerable<Book> Get()
        {
            return objBooks.AsEnumerable();
        }

    }
}
