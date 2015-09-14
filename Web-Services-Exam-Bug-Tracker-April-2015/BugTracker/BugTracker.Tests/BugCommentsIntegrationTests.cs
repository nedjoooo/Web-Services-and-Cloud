namespace BugTracker.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using BugTracker.Tests.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BugCommentsIntegrationTests
    {
        [TestMethod]
        public void CreateComments_ShouldListCreatedCommentsCorrectly()
        {
            // Arrange -> create a new bug with two comments
            TestingEngine.CleanDatabase();

            var bugTitle = "Bug Title";
            var httpPostResponse = TestingEngine.SubmitBugHttpPost(bugTitle, null);
            Assert.AreEqual(HttpStatusCode.Created, httpPostResponse.StatusCode);
            var submittedBug = httpPostResponse.Content.ReadAsAsync<BugModel>().Result;

            var httpPostResponceFirstComment = TestingEngine.SubmitCommentHttpPost(submittedBug.Id, "Comment 1");
            Assert.AreEqual(HttpStatusCode.OK, httpPostResponceFirstComment.StatusCode);
            Thread.Sleep(2);

            var httpPostResponceSecondComment = TestingEngine.SubmitCommentHttpPost(submittedBug.Id, "Comment 2");
            Assert.AreEqual(HttpStatusCode.OK, httpPostResponceSecondComment.StatusCode);

            // Act -> find bug comments
            var httpResponce = TestingEngine.HttpClient.GetAsync("/api/bugs/" + submittedBug.Id + "/comments").Result;

            // Assert -> check the returned comments
            Assert.AreEqual(HttpStatusCode.OK, httpResponce.StatusCode);
            var submittedComments = httpResponce.Content.ReadAsAsync<List<CommentModel>>().Result;

            Assert.AreEqual(2, submittedComments.Count);

            Assert.IsTrue(submittedComments[0].Id > 0);
            Assert.AreEqual("Comment 2", submittedComments[0].Text);
            Assert.AreEqual(null, submittedComments[0].Author);
            Assert.IsTrue(submittedComments[0].DateCreated - DateTime.Now < TimeSpan.FromMinutes(1));

            Assert.IsTrue(submittedComments[1].Id > 0);
            Assert.AreEqual("Comment 1", submittedComments[1].Text);
            Assert.AreEqual(null, submittedComments[1].Author);
            Assert.IsTrue(submittedComments[1].DateCreated - DateTime.Now < TimeSpan.FromMinutes(1));
        }
    }
}
