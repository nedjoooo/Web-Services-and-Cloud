namespace BidSystem.RestServices.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using BidSystem.Data;
    using BidSystem.Data.Models;
    using BidSystem.RestServices.Models;
    using Microsoft.AspNet.Identity;

    public class OffersController : ApiController
    {
        private BidSystemDbContext db = new BidSystemDbContext();

        [HttpGet]
        [Route("api/offers/all")]
        public IHttpActionResult GetAllOffers()
        {
            var offers = db.Offers
                .OrderByDescending(o => o.DatePublished)
                .ThenByDescending(o => o.Id)
                .Select(OfferOutputModel.FromOffer);

            return this.Ok(offers);
        }

        [HttpGet]
        [Route("api/offers/active")]
        public IHttpActionResult GetAllActiveOffers()
        {
            var offers = db.Offers
                .Where(o => o.ExpirationDateTime >= DateTime.Now)
                .OrderBy(o => o.ExpirationDateTime)
                .ThenByDescending(o => o.Id)
                .Select(OfferOutputModel.FromOffer);

            return this.Ok(offers);
        }

        [HttpGet]
        [Route("api/offers/expired")]
        public IHttpActionResult GetAllExpiredOffers()
        {
            var offers = db.Offers
                .Where(o => o.ExpirationDateTime < DateTime.Now)
                .OrderBy(o => o.ExpirationDateTime)
                .ThenByDescending(o => o.Id)
                .Select(OfferOutputModel.FromOffer);

            return this.Ok(offers);
        }

        [HttpGet]
        [Route("api/offers/details/{id}")]
        public IHttpActionResult GetOfferById(int id)
        {
            var offer = db.Offers
                .Where(o => o.Id == id)
                .Select(OfferDetailOutputModel.FromOfferWithDetail)
                .FirstOrDefault();

            if (offer == null)
            {
                return this.NotFound();
            }

            return this.Ok(offer);
        }

        [HttpGet]
        [Route("api/offers/my")]
        [Authorize]
        public IHttpActionResult GetCurrentUserOffers()
        {
            var currentUserId = User.Identity.GetUserId();

            if (currentUserId == null)
            {
                return this.Unauthorized();
            }

            var currentUserOffers = db.Offers
                .Where(o => o.SellerId == currentUserId)
                .OrderByDescending(o => o.DatePublished)
                .ThenByDescending(o => o.Id)
                .Select(OfferOutputModel.FromOffer);

            return this.Ok(currentUserOffers);
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult CreateNewOffer(OfferBindingModel offerModel)
        {
            if (offerModel == null)
            {
                return this.BadRequest("Invalid offer data.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserId = User.Identity.GetUserId();
            var currentUser = db.Users.FirstOrDefault(u => u.Id == currentUserId);

            if (currentUser == null)
            {
                return this.Unauthorized();
            }

            Offer offer = new Offer()
            {
                Title = offerModel.Title,
                Description = offerModel.Description,
                InitialPrice = offerModel.InitialPrice,
                ExpirationDateTime = DateTime.Parse(offerModel.ExpirationDateTime),
                SellerId = currentUserId,
                DatePublished = DateTime.Now
            };

            db.Offers.Add(offer);
            db.SaveChanges();

            return CreatedAtRoute(
                    "DefaultApi",
                    new
                    {
                        id = offer.Id
                    },
                    new
                    {
                        id = offer.Id,
                        Seller = currentUser.UserName,
                        Message = "Offer created."
                    });
        }
    }
}
