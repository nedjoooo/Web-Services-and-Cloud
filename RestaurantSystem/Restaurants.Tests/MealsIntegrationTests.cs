using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Owin;
using Restaurants.Data;
using Restaurants.Models;
using Restaurants.Services;

namespace Restaurants.Tests
{
    [TestClass]
    public class MealsIntegrationTests
    {
        private static TestServer TestWebServer { get; set; }

        public static HttpClient HttpClient { get; private set; }

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // Start OWIN testing HTTP server with Web API support
            TestWebServer = TestServer.Create(appBuilder =>
            {
                var config = new HttpConfiguration();
                WebApiConfig.Register(config);
                var webAppStartup = new Startup();
                webAppStartup.Configuration(appBuilder);
                appBuilder.UseWebApi(config);
            });
            HttpClient = TestWebServer.HttpClient;
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            // Stop the OWIN testing HTTP server
            TestWebServer.Dispose();
        }

        public static HttpResponseMessage RegisterUserHttpPost(string username, string password, string confirmPassword, string email)
        {
            var postContent = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("confirmPassword", confirmPassword),
                new KeyValuePair<string, string>("email@email.bg", email)
            });
            var httpResponse = MealsIntegrationTests.HttpClient.PostAsync("/api/account/register", postContent).Result;
            return httpResponse;
        }

        public static HttpResponseMessage LoginUserHttpPost(string username, string password)
        {
            var postContent = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });
            var httpResponse = MealsIntegrationTests.HttpClient.PostAsync("/api/account/login", postContent).Result;
            return httpResponse;
        }

        [TestMethod]
        public void GetMealsByRestaurant_ShouldReturn200OK_ExistingRestaurant()
        {
            var context = new RestaurantsContext();
            var existingRestaurant = context.Restaurants.FirstOrDefault();
            if (existingRestaurant == null)
            {
                Assert.Fail("Cannot perform test - no restaurants in db.");
            }

            var endPoint = string.Format("api/restaurants/{0}/meals", existingRestaurant.Id);
            var responce = HttpClient.GetAsync(endPoint).Result;
            Assert.AreEqual(HttpStatusCode.OK, responce.StatusCode);
        }
    }
}
