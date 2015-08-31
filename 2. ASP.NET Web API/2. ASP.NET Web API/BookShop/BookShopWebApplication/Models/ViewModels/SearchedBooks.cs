using BookShopSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookShopWebApplication.Models.ViewModels
{
    public class SearchedBooks
    {
        public SearchedBooks(Book book)
        {
            this.Title = book.Title;
            this.Id = book.Id;
        }
        public string Title { get; set; }
        public int Id { get; set; }
    }
}