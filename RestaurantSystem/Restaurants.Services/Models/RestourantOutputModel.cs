using System;
using System.Linq;
using System.Linq.Expressions;
using Restaurants.Models;

namespace Restaurants.Services.Models
{
    public class RestourantOutputModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double? Rating { get; set; }

        public TownOutputModel Town { get; set; }

        public static Expression<Func<Restaurant, RestourantOutputModel>> GetRestourants
        {
            get
            {
                return b => new RestourantOutputModel()
                {
                    Id = b.Id,
                    Name = b.Name,
                    Rating = b.Ratings.Average(rt => rt.Stars),
                    Town = new TownOutputModel() { Id = b.TownId, Name = b.Town.Name}
                };
            }
        }
    }
}