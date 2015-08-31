﻿using BookShopSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookShopWebApplication.Models.ViewModels
{
    public class AuthorBooksViewModel
    {
        public AuthorBooksViewModel(Book book)
        {
            this.Id = book.Id;
            this.Title = book.Title;
            this.Description = book.Description;
            this.EditionType = (EditionType)book.EditionType;
            this.Price = book.Price;
            this.Copies = book.Copies;
            this.RelesaeDate = (DateTime)book.RelesaeDate;
            this.AgeRestriction = (AgeRestriction)book.AgeRestriction;
            this.Categories = new List<CategoryBooksViewModel>();
            foreach (var category in book.Categories)
            {
                Categories.Add(new CategoryBooksViewModel(category));
            }
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public EditionType EditionType { get; set; }
        public decimal Price { get; set; }
        public int Copies { get; set; }
        public DateTime RelesaeDate { get; set; }
        public AgeRestriction AgeRestriction { get; set; }
        public ICollection<CategoryBooksViewModel> Categories { get; set; }
    }
}