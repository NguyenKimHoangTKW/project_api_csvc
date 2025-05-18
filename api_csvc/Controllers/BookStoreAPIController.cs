using api_csvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace api_csvc.Controllers
{
    [RoutePrefix("api/v1")]
    public class BookStoreAPIController : ApiController
    {
        csvcapiEntities1 db = new csvcapiEntities1();

        [HttpGet]
        [Route("danh-sach-books")]
        public async Task<IHttpActionResult> danh_sach_books()
        {
            var get_list_books = await db.Books.ToListAsync();
            var list_data = new List<object>();

            foreach (var book in get_list_books)
            {
                var list_genre = await db.Details_book_genre
                    .Where(x => x.id_books == book.id_books)
                    .Select(x => x.Genre.name_genre)
                    .ToListAsync();

                list_data.Add(new
                {
                    id = book.id_books,
                    bookName = book.bookName,
                    bookCover = book.bookCover,
                    rating = book.rating,
                    language = book.Language.acronym,
                    pageNo = book.pageNo,
                    author = book.Author.name_author,
                    genre = list_genre,
                    readed = book.readed,
                    lastRead = book.lastRead,
                    completion = book.completion,
                    description = book.description,
                    backgroundColor = book.backgroundColor,
                    navTintColor = book.navTintColor
                });
            }

            if (list_data.Count > 0)
            {
                return Ok(new { data = list_data, success = true });
            }
            else
            {
                return Ok(new { message = "Không tìm thấy thông tin sách", success = false });
            }
        }

        [HttpGet]
        [Route("danh-sach-categories")]
        public async Task<IHttpActionResult> danh_sach_categories()
        {
            var list_cat = await db.Categories.ToListAsync();
            var list_data = new List<object>();
            
            foreach (var items in list_cat)
            {
                var list_data_ = new List<object>();
                var list_book_by_cat = await db.books_by_categories
                    .Where(x => x.id_categories == items.id_categories)
                    .ToListAsync();
                foreach (var books in list_book_by_cat)
                {
                    var list_genre = await db.Details_book_genre
                    .Where(x => x.id_books == books.id_books)
                    .Select(x => x.Genre.name_genre)
                    .ToListAsync();
                    list_data_.Add(new
                    {

                        id = books.id_books,
                        books.Book.bookName,
                        books.Book.bookCover,
                        books.Book.rating,
                        books.Book.Language.acronym,
                        books.Book.pageNo,
                        books.Book.Author.name_author,
                        genre = list_genre,
                        books.Book.readed,
                        books.Book.completion,
                        books.Book.lastRead,
                        books.Book.description,
                        books.Book.backgroundColor,
                        books.Book.navTintColor
                    });
                    
                }
                list_data.Add(new
                {
                    id = items.id_categories,
                    categoryName = items.name_categories,
                    books = list_data_
                });
            }
           
            if (list_data.Count > 0)
            {
                return Ok(new { data = list_data, success = true });
            }
            else
            {
                return Ok(new { message = "Không tìm thấy thông tin", success = false });
            }
        }
        [HttpPost]
        [Route("detail_books")]
        public async Task<IHttpActionResult> danh_sach_book_by_categories(Book items)
        {
            if (items == null)
            {
                return Ok(new { message = "Không  tìm thấy thông tin", success = false });
            }
            var list_genre = await db.Details_book_genre
                    .Where(x => x.id_books == items.id_books)
                    .Select(x => x.Genre.name_genre)
                    .ToListAsync();
            var list_book_by_cat = await db.Books
                .Where(x => x.id_books == items.id_books)
                .Select(x => new
                {
                    id = x.id_books,
                    x.bookName,
                    x.bookCover,
                    x.rating,
                    x.Language.acronym,
                    x.pageNo,
                    x.Author.name_author,
                    genre = list_genre,
                    x.readed,
                    x.lastRead,
                    x.completion,
                    x.description,
                    x.backgroundColor,
                    x.navTintColor

                })
                .FirstOrDefaultAsync();
            return Ok(new { data = list_book_by_cat, success = true });
        }
    }
}
