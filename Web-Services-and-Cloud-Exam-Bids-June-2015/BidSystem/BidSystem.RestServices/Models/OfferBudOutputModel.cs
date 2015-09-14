using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BidSystem.RestServices.Models
{
    public class OfferBudOutputModel
    {
        public int Id { get; set; }
        public int OfferId { get; set; }
        public DateTime DateCreated { get; set; }
        public string Bidder { get; set; }
        public decimal OfferedPrice { get; set; }
        public string Comment { get; set; }
    }
}