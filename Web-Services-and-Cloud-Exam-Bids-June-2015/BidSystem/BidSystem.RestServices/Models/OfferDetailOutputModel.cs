using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using BidSystem.Data.Models;

namespace BidSystem.RestServices.Models
{
    public class OfferDetailOutputModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Seller { get; set; }
        public DateTime DatePublished { get; set; }
        public decimal InitialPrice { get; set; }
        public DateTime ExpirationDateTime { get; set; }
        public bool IsExpired { get; set; }
        public string BidWinner { get; set; }
        public IEnumerable<OfferBudOutputModel> Bids { get; set; }

        public static Expression<Func<Offer, OfferDetailOutputModel>> FromOfferWithDetail
        {
            get
            {
                return o => new OfferDetailOutputModel()
                {
                    Id = o.Id,
                    Title = o.Title,
                    Description = o.Description,
                    Seller = o.Seller.UserName,
                    DatePublished = o.DatePublished,
                    InitialPrice = o.InitialPrice,
                    ExpirationDateTime = o.ExpirationDateTime,
                    IsExpired = o.ExpirationDateTime <= DateTime.Now,
                    BidWinner = o.ExpirationDateTime <= DateTime.Now && o.Bids.Count > 0 ?
                        o.Bids.OrderByDescending(b => b.OfferedPrice).FirstOrDefault().Bidder.UserName : null,
                    Bids = o.Bids
                        .OrderByDescending(b => b.DateCreated)
                        .ThenByDescending(b => b.Id)
                        .Select(b => new OfferBudOutputModel()
                        {
                            Id = b.Id,
                            OfferId = b.OfferId,
                            DateCreated = b.DateCreated,
                            Bidder = b.Bidder.UserName,
                            OfferedPrice = b.OfferedPrice,
                            Comment = b.Comment
                        })
                };
            }
        }
    }
}