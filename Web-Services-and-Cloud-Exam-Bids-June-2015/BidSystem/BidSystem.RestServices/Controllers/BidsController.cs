using System;
using System.Linq;
using System.Web.Http;
using BidSystem.Data;
using BidSystem.Data.Models;
using BidSystem.RestServices.Models;
using Microsoft.AspNet.Identity;

namespace BidSystem.RestServices.Controllers
{
    public class BidsController : ApiController
    {
        private BidSystemDbContext db = new BidSystemDbContext();

        [HttpGet]
        [Authorize]
        [Route("api/bids/my")]
        public IHttpActionResult GetMyBids()
        {
            var currentUserId = User.Identity.GetUserId();

            if (currentUserId == null)
            {
                return this.Unauthorized();
            }

            var currentUserBids = db.Bids
                .Where(b => b.BidderId == currentUserId)
                .OrderByDescending(b => b.DateCreated)
                .ThenByDescending(b => b.Id)
                .Select(BidOutputModel.FromBid);

            return this.Ok(currentUserBids);
        }

        [HttpGet]
        [Authorize]
        [Route("api/bids/won")]
        public IHttpActionResult GetUserWonBids()
        {
            var currentUserId = User.Identity.GetUserId();
            if (currentUserId == null)
            {
                return this.Unauthorized();
            }

            var userWonBids = db.Bids
                .Where(b => b.BidderId == currentUserId && b.Offer.ExpirationDateTime <= DateTime.Now)
                .Where(b => b.OfferedPrice == b.Offer.Bids.Max(bid => bid.OfferedPrice))
                .OrderBy(b => b.DateCreated)
                .ThenBy(b => b.Id)
                .Select(BidOutputModel.FromBid);

            return this.Ok(userWonBids);
        }

        [HttpPost]
        [Authorize]
        [Route("api/offers/{offerId}/bid")]
        public IHttpActionResult CreateBidToExistingOffer(int offerId, BidBindingModel bidModel)
        {
            if (bidModel == null)
            {
                return this.BadRequest("Invalid bid price.");
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

            var offer = db.Offers.FirstOrDefault(o => o.Id == offerId);
            if (offer == null)
            {
                return this.NotFound();
            }

            if (offer.ExpirationDateTime < DateTime.Now)
            {
                return this.BadRequest("Offer has expired.");
            }

            if (offer.Bids.Count > 0)
            {
                var maxBidForOffer = offer.Bids.Max(b => b.OfferedPrice);
                if (bidModel.BidPrice <= maxBidForOffer)
                {
                    return this.BadRequest("Your bid should be > " + maxBidForOffer);
                }
            }
            else
            {
                if (bidModel.BidPrice <= offer.InitialPrice)
                {
                    return this.BadRequest("Your bid should be > " + offer.InitialPrice);
                }
            }

            var bid = new Bid()
            {
                BidderId = currentUserId,
                DateCreated = DateTime.Now,
                OfferId = offer.Id,
                OfferedPrice = bidModel.BidPrice,
                Comment = bidModel.Comment != null ? bidModel.Comment : null
            };

            offer.Bids.Add(bid);
            db.SaveChanges();

            return this.Ok(new
            {
                Id = bid.Id,
                Bidder = User.Identity.GetUserName(),
                Message = "Bid created."
            });
        }
    }
}
