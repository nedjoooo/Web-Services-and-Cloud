using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Restaurants.Services.Models
{
    public class MealBindingModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        [Required]
        public int TypeId { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}