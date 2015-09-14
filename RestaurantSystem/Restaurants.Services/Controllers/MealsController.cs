using System;
using System.Linq;
using System.Web.Http;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Restaurants.Data;
using Restaurants.Models;
using Restaurants.Services.Models;

namespace Restaurants.Services.Controllers
{
    [RoutePrefix("api/meals")]
    public class MealsController : ApiController
    {
        private RestaurantsContext db = new RestaurantsContext();

        [HttpPost]
        [Authorize]
        public IHttpActionResult CreateNewMeal(MealBindingModel mealModel)
        {
            if (mealModel == null)
            {
                return this.BadRequest("Invalid meal data.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mealType = db.MealTypes.FirstOrDefault(mt => mt.Id == mealModel.TypeId);
            if (mealType == null)
            {
                return this.BadRequest("Invalid meal type id.");
            }

            var restaurant = db.Restaurants.FirstOrDefault(r => r.Id == mealModel.RestaurantId);
            if (restaurant == null)
            {
                return this.BadRequest("Invalid restaurant id.");
            }

            var currentUserId = User.Identity.GetUserId();
            if (currentUserId == null)
            {
                return this.Unauthorized();
            }

            var meal = new Meal()
            {
                Name = mealModel.Name,
                Price = mealModel.Price,
                Restaurant = restaurant,
                Type = mealType
            };

            db.Meals.Add(meal);
            db.SaveChanges();

            return CreatedAtRoute(
                "DefaultApi",
                new
                {
                    id = meal.Id
                },
                new MealOutputModel()
                {
                    Id = meal.Id,
                    Name = meal.Name,
                    Price = meal.Price,
                    Type = meal.Type.Name
                });
        }

        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public IHttpActionResult EditExistingMeal(int id, EditMealBindingModel editMealModel)
        {
            if (editMealModel == null)
            {
                return this.BadRequest("Invalid meal data.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mealType = db.MealTypes.FirstOrDefault(mt => mt.Id == editMealModel.TypeId);
            if (mealType == null)
            {
                return this.BadRequest("Invalid meal type id.");
            }

            var meal = db.Meals.FirstOrDefault(m => m.Id == id);
            if (meal == null)
            {
                return this.NotFound();
            }

            var currentUserId = User.Identity.GetUserId();
            if (currentUserId == null || meal.Restaurant.OwnerId != currentUserId)
            {
                return this.Unauthorized();
            }

            meal.Name = editMealModel.Name;
            meal.Price = editMealModel.Price;
            meal.Type = mealType;
            db.SaveChanges();

            return
                this.Ok(new MealOutputModel()
                {
                    Id = meal.Id,
                    Name = meal.Name,
                    Price = meal.Price,
                    Type = meal.Type.Name
                });
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public IHttpActionResult DeleteMealById(int id)
        {
            var meal = db.Meals.FirstOrDefault(m => m.Id == id);
            if (meal == null)
            {
                return this.NotFound();
            }

            var currentUserId = User.Identity.GetUserId();
            if (currentUserId == null || meal.Restaurant.OwnerId != currentUserId)
            {
                return this.Unauthorized();
            }

            db.Meals.Remove(meal);
            db.SaveChanges();

            return this.Ok(new { Message = "Meal #" + meal.Id + " deleted." });
        }

        [HttpPost]
        [Authorize]
        [Route("{id}/order")]
        public IHttpActionResult CreateNewOrder(int id, OrderBindingModel orderModel)
        {
            if (orderModel == null || orderModel.Quantity < 1)
            {
                return this.BadRequest("Invalid order data.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meal = db.Meals.FirstOrDefault(m => m.Id == id);
            if (meal == null)
            {
                return this.NotFound();
            }

            var currentUserId = User.Identity.GetUserId();
            if (currentUserId == null)
            {
                return this.Unauthorized();
            }

            var user = db.Users.FirstOrDefault(u => u.Id == currentUserId);

            var order = new Order()
            {
                User = user,
                CreatedOn = DateTime.Now,
                Meal = meal,
                OrderStatus = OrderStatus.Pending,
                Quantity = orderModel.Quantity
            };

            db.Orders.Add(order);
            db.SaveChanges();

            return this.Ok();
        }
    }
}
