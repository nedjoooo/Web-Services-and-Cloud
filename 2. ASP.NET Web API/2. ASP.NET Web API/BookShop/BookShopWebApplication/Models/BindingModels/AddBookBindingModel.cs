using BookShopSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookShopWebApplication.Models.BindingModels
{
    public class AddBookBindingModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public EditionType? EditionType { get; set; }

        public decimal Price { get; set; }

        public int Copies { get; set; }

        public DateTime? RelesaeDate { get; set; }

        public AgeRestriction? AgeRestriction { get; set; }

        public int AuthorId { get; set; }

        public string Categories { get; set; }
    }
}