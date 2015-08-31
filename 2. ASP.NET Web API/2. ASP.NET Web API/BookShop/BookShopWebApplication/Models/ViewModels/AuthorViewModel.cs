using BookShopSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookShopWebApplication.Models.ViewModels
{
    public class AuthorViewModel
    {
        public AuthorViewModel(Author author)
        {
            Id = author.Id;
            FirstName = author.FirstName;
            LastName = author.LastName;
            Books = new List<BookViewModel>();
            foreach (var book in author.Books)
            {
                Books.Add(new BookViewModel(book));
            }
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<BookViewModel> Books { get; set; }

    }
}