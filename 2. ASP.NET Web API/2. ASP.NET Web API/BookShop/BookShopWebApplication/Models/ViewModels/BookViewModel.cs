using BookShopSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookShopWebApplication.Models.ViewModels
{
    public class BookViewModel
    {
        public BookViewModel(Book book)
        {
            Title = book.Title;
        }
        public string Title { get; set; }     
    }
}
