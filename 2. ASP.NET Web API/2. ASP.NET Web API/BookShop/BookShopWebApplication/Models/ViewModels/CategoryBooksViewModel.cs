using BookShopSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookShopWebApplication.Models.ViewModels
{
    public class CategoryBooksViewModel
    {
        public CategoryBooksViewModel(Category category)
        {
            this.Name = category.Name;
        }
        public string Name { get; set; }
    }
}
