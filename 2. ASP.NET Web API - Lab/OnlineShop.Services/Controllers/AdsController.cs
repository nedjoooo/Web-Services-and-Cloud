using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using OnlineShop.Models;
using OnlineShop.Services.Models;

namespace OnlineShop.Services.Controllers
{
    [Authorize]
    public class AdsController : BaseApiController
    {
        [AllowAnonymous]
        public IHttpActionResult GetAds()
        {
            var ads = this.Data.Ads
                .Where(a => a.Status == AdStatus.Open)
                .OrderBy(a => a.Type.Name)
                .ThenByDescending(a => a.PostedOn)
                .Select(AdViewModel.Create);

            return this.Ok(ads);
        }

        // POST api/ads
        [HttpPost]
        public IHttpActionResult CreateAd(CreateAdBindingModel model)
        {
            var userId = this.User.Identity.GetUserId();

            if (model == null)
            {
                return this.BadRequest("Model cannot be null (no data in request)");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var adType = this.Data.AdTypes
                .FirstOrDefault(at => at.Name == model.Type);

            if (adType == null)
            {
                return this.BadRequest("Type is invalid");
            }

            var ad = new Ad()
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Type = adType,
                PostedOn = DateTime.Now,
                Status = AdStatus.Open,
                OwnerId = this.User.Identity.GetUserId()
            };

            this.Data.Ads.Add(ad);
            this.Data.SaveChanges();

            var data = this.Data.Ads
                .Where(a => a.Id == ad.Id)
                .Select(AdViewModel.Create)
                .FirstOrDefault();

            return this.Ok(data);
        }

        // PUT api/ads/{id}/close
        [HttpPut]
        [Route("api/ads/{id}/close")]
        public IHttpActionResult CloseAd(int id)
        {
            var ad = this.Data.Ads.Find(id);
            if (ad == null)
            {
                return this.NotFound();
            }

            string userId = this.User.Identity.GetUserId();
            if (userId != ad.OwnerId)
            {
                return this.Unauthorized();
            }

            if (ad.Status == AdStatus.Closed)
            {
                return this.BadRequest("This ad has closed!");
            }

            ad.Status = AdStatus.Closed;
            this.Data.SaveChanges();

            var data = this.Data.Ads
                .Where(a => a.Id == ad.Id)
                .Select(AdViewModel.Create)
                .FirstOrDefault();

            return this.Ok(data);
        }
    }
}