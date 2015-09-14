using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Restaurants.Data;
using Restaurants.Models;
using Restaurants.Services.Models;

namespace Restaurants.Services.Controllers
{
    [RoutePrefix("api/restaurants")]
    public class RestaurantsController : ApiController
    {
        private RestaurantsContext db = new RestaurantsContext();

        [HttpGet]
        public IHttpActionResult GetAllRestaurants([FromUri]int townId)
        {
            var restaurants = db.Restaurants
                .Where(r => r.TownId == townId)
                .OrderByDescending(r => r.Ratings.Average(rt => rt.Stars))
                .ThenBy(r => r.Name)
                .Select(RestourantOutputModel.GetRestourants);

            return this.Ok(restaurants);
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult CreateNewRestaurant(RestourantBindingModel restourantModel)
        {
            if (restourantModel == null)
            {
                return this.BadRequest("Invalid restaurant data.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserId = User.Identity.GetUserId();

            if (currentUserId == null)
            {
                return this.Unauthorized();
            }

            var town = db.Towns.FirstOrDefault(t => t.Id == restourantModel.TownId);

            var restaurant = new Restaurant()
            {
                Name = restourantModel.Name,
                Town = town,
                OwnerId = currentUserId
            };

            db.Restaurants.Add(restaurant);
            db.SaveChanges();

            return CreatedAtRoute(
                    "DefaultApi",
                    new
                    {
                        id = restaurant.Id
                    },
                    new RestourantOutputModel()
                    {
                        Id = restaurant.Id,
                        Name = restaurant.Name,
                        Rating = null,
                        Town = new TownOutputModel() { Id = restaurant.TownId, Name = restaurant.Town.Name}
                    });
        }

        [HttpPost]
        [Authorize]
        [Route("{id}/rate")]
        public IHttpActionResult RateRestaurant(int id, RateRestaurantModel ratingRestaurantModel)
        {
            if (ratingRestaurantModel.Stars < 1 || ratingRestaurantModel.Stars > 10)
            {
                return this.BadRequest("Rating must be in range 1 - 10");
            }

            var currentUserId = User.Identity.GetUserId();

            if (currentUserId == null)
            {
                return this.Unauthorized();
            }

            var restaurant = db.Restaurants.FirstOrDefault(r => r.Id == id);

            if (restaurant == null)
            {
                return this.NotFound();
            }

            if (currentUserId == restaurant.OwnerId)
            {
                return this.BadRequest("Restaurant owner not rates his own restaurant.");
            }

            var user = db.Users.FirstOrDefault(u => u.Id == currentUserId);

            var currentRating = db.Ratings.FirstOrDefault(r => r.RestaurantId == restaurant.Id && r.UserId == currentUserId);

            if (currentRating == null)
            {
                var rating = new Rating()
                {
                    User = user,
                    Restaurant = restaurant,
                    Stars = ratingRestaurantModel.Stars
                };

                db.Ratings.Add(rating);
            }
            else
            {
                currentRating.Stars = ratingRestaurantModel.Stars;
            }

            db.SaveChanges();

            return this.Ok();
        }

        [HttpGet]
        [Route("{id}/meals")]
        public IHttpActionResult GetRestaurantMeals(int id)
        {
            var restaurant = db.Restaurants.FirstOrDefault(r => r.Id == id);

            if (restaurant == null)
            {
                return this.NotFound();
            }

            var meals = restaurant.Meals
                .OrderBy(m => m.Type.Order)
                .ThenBy(m => m.Name)
                .Select(m => new
                {
                    Id = m.Id,
                    Name = m.Name,
                    Price = m.Price,
                    Type = m.Type != null ? m.Type.Name : null
                })
                .AsQueryable();

            return this.Ok(meals);
        }
    }
}
