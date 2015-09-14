using System.ComponentModel.DataAnnotations;

namespace Restaurants.Services.Models
{
    public class RestourantBindingModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int TownId { get; set; }
    }
}