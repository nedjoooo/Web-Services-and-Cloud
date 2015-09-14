using System;
using System.Linq;
using BugTracker.Data.Models;
using BugTracker.RestServices.Models.BindingModels;
using BugTracker.RestServices.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace BugTracker.RestServices.Controllers
{
    using System.Web.Http;
    using BugTracker.Data;

    public class CommentsController : ApiController
    {
        private BugTrackerDbContext db = new BugTrackerDbContext();

        // GET: api/comments
        [HttpGet]
        [Route("api/comments")]
        public IHttpActionResult GetAllComents()
        {
            var comments = db.Comments
                .OrderByDescending(c => c.PublishDate)
                .Select(CommentDetailViewModel.Create);

            return this.Ok(comments);
        }

        // GET: api/bugs/{id}/comments
        [HttpGet]
        [Route("api/bugs/{id}/comments")]
        public IHttpActionResult GetCommentsForGivenBug(int id)
        {
            var bug = db.Bugs.FirstOrDefault(b => b.Id == id);

            if (bug == null)
            {
                return this.NotFound();
            }

            var bugComments = bug.Comments
                .OrderByDescending(c => c.PublishDate)
                .ThenByDescending(c => c.Id)
                .Select(c => new CommentViewModel()
                {
                    Id = c.Id,
                    Text = c.Text,
                    Author = c.Author != null ? c.Author.UserName : null,
                    DateCreated = c.PublishDate
                });

            return this.Ok(bugComments);
        }

        //  POST: api/bugs/{id}/comments
        [HttpPost]
        [Route("api/bugs/{id}/comments")]
        public IHttpActionResult AddCommentForGivenBug(int id, AddCommentBindingModel addCommentModel)
        {
            if (addCommentModel == null)
            {
                return this.BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bug = db.Bugs.FirstOrDefault(b => b.Id == id);

            if (bug == null)
            {
                return this.NotFound();
            }

            var currentUserId = User.Identity.GetUserId();
            var author = db.Users.FirstOrDefault(u => u.Id == currentUserId);

            Comment comment = new Comment()
            {
                Text = addCommentModel.Text,
                PublishDate = DateTime.Now,
                Author = author
            };

            bug.Comments.Add(comment);
            db.SaveChanges();

            if (author == null)
            {
                return this.Ok(new
                {
                    Id = comment.Id,
                    Message = "Added anonymous comment for bug #" + bug.Id
                });
            }

            return this.Ok(new
            {
                Id = comment.Id,
                Author = author.UserName,
                Message = "User comment added for bug #" + bug.Id
            });
        }
    }
}
