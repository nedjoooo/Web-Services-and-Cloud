using System;
using System.Net;
using BidSystem.Data.Models;
using BidSystem.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BidSystem.Tests
{
    [TestClass]
    public class OfferDetailsIntegrationTests
    {
        [TestMethod]
        public void CreateOffer_ShouldReturnCreatedOfferCorrectly()
        {
            // Arrange -> clean the database and register new user
            TestingEngine.CleanDatabase();
            var userSession = TestingEngine.RegisterUser("peter", "pAssW@rd#123456");

            // Act -> create a few offers
            var offersToAdds = new OfferModel[]
            {
                new OfferModel() { Title = "First Offer (Expired)", Description = "Description", InitialPrice = 200, ExpirationDateTime = DateTime.Now.AddDays(-5)},
                new OfferModel() { Title = "Third Offer (Active 6 months)", InitialPrice = 120, ExpirationDateTime = DateTime.Now.AddMonths(6)},
            };

            foreach (var offer in offersToAdds)
            {
                var httpResult = TestingEngine.CreateOfferHttpPost(userSession.Access_Token, offer.Title, offer.Description, offer.InitialPrice, offer.ExpirationDateTime);
                Assert.AreEqual(HttpStatusCode.Created, httpResult.StatusCode);
            }


        }
    }
}
