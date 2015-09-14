using System.Linq;
using System.Web.Http;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Restaurants.Data;
using Restaurants.Models;
using Restaurants.Services.Models;

namespace Restaurants.Services.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        private RestaurantsContext db = new RestaurantsContext();

        [HttpGet]
        [Authorize]
        public IHttpActionResult ViewPendingOrders(
            [FromUri] int startPage,
            [FromUri] int limit,
            [FromUri] int? mealId = null)
        {
            var currentUserId = User.Identity.GetUserId();
            if (currentUserId == null)
            {
                this.Unauthorized();
            }

            var orders = db.Orders
                .Where(o => o.OrderStatus == OrderStatus.Pending && o.UserId == currentUserId)
                .OrderByDescending(o => o.CreatedOn)
                .Select(o => new
                {
                    Id = o.Id,
                    Meal = new MealOutputModel()
                    {
                        Id = o.MealId,
                        Name = o.Meal.Name,
                        Price = o.Meal.Price,
                        Type = o.Meal.Type.Name
                    },
                    Quantity = o.Quantity,
                    Status = o.OrderStatus.ToString(),
                    CreatedOn = o.CreatedOn                  
                })
                .AsQueryable();

            if (mealId != null)
            {
                orders = orders.Where(o => o.Meal.Id == mealId);
            }

            if (limit < 2 || limit > 10)
            {
                return this.BadRequest("Limit should be in range [2..10]");
            }

            if (startPage == 0)
            {
                orders = orders.Take(limit);
            }
            else
            {
                startPage = startPage*limit;
                orders = orders.Skip(startPage).Take(limit);
            }

            return this.Ok(orders);
        }
    }
}
