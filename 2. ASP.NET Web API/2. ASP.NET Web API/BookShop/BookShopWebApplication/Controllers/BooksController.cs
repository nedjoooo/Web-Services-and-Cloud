using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BookShopSystem.Data;
using BookShopSystem.Models;
using BookShopWebApplication.Models.ViewModels;
using BookShopWebApplication.Models.BindingModels;

namespace BookShopWebApplication.Controllers
{
    public class BooksController : ApiController
    {
        private BookShopContext db = new BookShopContext();

        // GET: api/Books
        public IEnumerable<BooksViewModel> GetBooks()
        {
            List<BooksViewModel> books = new List<BooksViewModel>();
            var dbBooks = db.Books.ToList();

            foreach (var book in dbBooks)
            {
                books.Add(new BooksViewModel(book));
            }

            return books;
        }

        // GET: api/Books/5
        [ResponseType(typeof(BooksViewModel))]
        public IHttpActionResult GetBook(int id)
        {
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            BooksViewModel booksViewModel = new BooksViewModel(book);

            return Ok(booksViewModel);
        }

        // GET: api/books?search={word}
        public IHttpActionResult GetBooksByGivenWord(string word)
        {
            var booksByGivenWord = db.Books
                .Where(b => b.Title.Contains(word))
                .OrderBy(b => b.Title)
                .Take(10)
                .Select(b => new
                {
                    b.Title,
                    b.Id
                });

            if(!booksByGivenWord.Any())
            {
                return this.NotFound();
            }

            return this.Ok(booksByGivenWord);
        }

        // PUT: api/Books/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBook(int id, Book changedBook)
        {
            var dbBook = db.Books.FirstOrDefault(b => b.Id == id);
            if(dbBook == null)
            {
                return this.NotFound();
            }

            dbBook.Title = changedBook.Title == String.Empty ? dbBook.Title : changedBook.Title;
            dbBook.Description = changedBook.Description == String.Empty ? dbBook.Description : changedBook.Description;
            dbBook.Price = changedBook.Price == 0 ? dbBook.Price : changedBook.Price;
            dbBook.Copies = changedBook.Copies == 0 ? dbBook.Copies : changedBook.Copies;
            dbBook.EditionType = changedBook.EditionType == null ? dbBook.EditionType : changedBook.EditionType;
            dbBook.AgeRestriction = changedBook.AgeRestriction == null ? dbBook.AgeRestriction : changedBook.AgeRestriction;
            dbBook.RelesaeDate = changedBook.RelesaeDate != null ? changedBook.RelesaeDate : dbBook.RelesaeDate;
            dbBook.AuthorId = changedBook.AuthorId == 0 ? dbBook.AuthorId : changedBook.AuthorId;

            db.SaveChanges();
            string output = String.Format("Book with id {0} changed successfully!", id);

            return this.Ok(output);
        }

        // DELETE: api/Books/{id}
        [HttpDelete]
        public IHttpActionResult DeleteBook(int id)
        {
            Book book = db.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            db.Books.Remove(book);
            db.SaveChanges();

            return Ok(book);
        }

        // POST: api/Books
        [HttpPost]
        public IHttpActionResult PostBook(AddBookBindingModel bookModel)
        {
            if(bookModel == null)
            {
                this.ModelState.AddModelError("bookModel", "Book model can not be empty!");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }           

            var book = new Book()
            {
                Title = bookModel.Title,
                Description = bookModel.Description ?? null,
                Price = bookModel.Price,
                Copies = bookModel.Copies,
                EditionType = bookModel.EditionType,
                AgeRestriction = bookModel.AgeRestriction,
                RelesaeDate = bookModel.RelesaeDate,
                Author = db.Authors.FirstOrDefault(a => a.Id == bookModel.AuthorId)
            };

            var categoryNames = bookModel.Categories.Split(' ');
            foreach (var categoryName in categoryNames)
            {
                var category = db.Categories.FirstOrDefault(c => c.Name == categoryName);
                book.Categories.Add(category);
            }

            db.Books.Add(book);
            db.SaveChanges();

            return this.Ok("Book created");
        }
    }
}