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
    public class AuthorsController : ApiController
    {
        private BookShopContext db = new BookShopContext();

        // GET: api/Authors
        public IEnumerable<AuthorViewModel> GetAuthors()
        {
            List<Author> authors = db.Authors.ToList();
            List<AuthorViewModel> viewModelAuthors = new List<AuthorViewModel>();

            foreach (var author in authors)
            {
                viewModelAuthors.Add(new AuthorViewModel(author));
            }

            return viewModelAuthors;
        }

        // GET: api/Authors/5
        [ResponseType(typeof(AuthorViewModel))]
        public IHttpActionResult GetAuthor(int id)
        {
            Author author = db.Authors.Find(id);
            
            if (author == null)
            {
                return NotFound();
            }

            AuthorViewModel viewModelAuthor = new AuthorViewModel(author);

            return Ok(viewModelAuthor);
        }

        // POST: api/Authors
        [HttpPost]
        public IHttpActionResult PostAuthor(AddAuthorBindingModel authorModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = new Author()
            {
                FirstName = authorModel.FirstName,
                LastName = authorModel.LastName
            };

            db.Authors.Add(author);
            db.SaveChanges();

            return this.Ok("Author created");
        }

        // GET: /api/Authors/{id}/Books
        [HttpGet]
        [Route("api/authors/{id}/books")]
        [ResponseType(typeof(AuthorBooksViewModel))]
        public IEnumerable<AuthorBooksViewModel> GetAuthorBooks(int id)
        {
            List<AuthorBooksViewModel> viewModelAuthorBooks = new List<AuthorBooksViewModel>();
            var authorBooks = db.Books
                .Where(b => b.AuthorId == id)
                .ToList();

            foreach (var authorBook in authorBooks)
            {
                viewModelAuthorBooks.Add(new AuthorBooksViewModel(authorBook));
            }

            return viewModelAuthorBooks;
        }
    }
}