using System;
using System.Linq;
using BugTracker.Data;
using BugTracker.Data.Models;
using BugTracker.RestServices.Models.BindingModels;
using BugTracker.RestServices.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace BugTracker.RestServices.Controllers
{
    using System.Web.Http;

    [RoutePrefix("api/bugs")]
    public class BugsController : ApiController
    {
        private BugTrackerDbContext db = new BugTrackerDbContext();

        // GET: api/bugs
        [HttpGet]
        public IHttpActionResult ListAllBugs()
        {
            var bugs = db.Bugs
                .OrderByDescending(b => b.SubmitDate)
                .Select(BugViewModel.Create);

            return this.Ok(bugs);
        }

        // GET: api/bugs/{id}
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetBugDetailsById(int id)
        {
            var bug = db.Bugs
                .Where(b => b.Id == id)
                .Select(BugDetailViewModel.Create)
                .FirstOrDefault();

            if (bug == null)
            {
                return this.NotFound();
            }

            return this.Ok(bug);
        }

        // POST: api/bugs
        [HttpPost]
        public IHttpActionResult SubmitNewBug(BugBindingModel bugModel)
        {
            if (bugModel.Title == null)
            {
                return BadRequest("Invalid bug title");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserId = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(u => u.Id == currentUserId);

            var bug = new Bug()
            {
                Title = bugModel.Title,
                Description = bugModel.Description,
                Status = Status.Open,
                SubmitDate = DateTime.Now,
                Author = user != null ? user : null
            };

            db.Bugs.Add(bug);
            db.SaveChanges();

            var bugOutput = new BugViewModel()
            {
                Id = bug.Id,
                Author = bug.Author != null ? bug.Author.UserName : null,
                Status = bug.Status.ToString(),
                Title = bug.Title,
                DateCreated = bug.SubmitDate
            };

            if (currentUserId != null)
            {
                bugOutput.Author = bug.Author.UserName;
                return CreatedAtRoute(
                    "DefaultApi", 
                    new
                    {
                        id = bugOutput.Id,
                        Author = bugOutput.Author,
                        Message = "User bug submitted."
                    },
                    new
                    {
                        id = bugOutput.Id,
                        Author = bugOutput.Author,
                        Message = "User bug submitted."
                    });
            }

            return CreatedAtRoute(
                    "DefaultApi",
                    new
                    {
                        id = bugOutput.Id,
                        Message = "Anonymous bug submitted."
                    },
                    new
                    {
                        id = bugOutput.Id,
                        Message = "Anonymous bug submitted."
                    });
        }

        //  PATCH: api/bugs/{id}
        [HttpPatch]
        [Route("{id}")]
        public IHttpActionResult EditExistingBug(int id, EditBindingModel editingModel)
        {
            if (editingModel == null)
            {
                return this.BadRequest("Invalid model data.");
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

            if (editingModel.Title != null)
            {
                bug.Title = editingModel.Title;
            }

            if (editingModel.Description != null)
            {
                bug.Description = editingModel.Description;
            }

            if (editingModel.Status != null)
            {
                if (editingModel.Status == "Open") { bug.Status = Status.Open; }
                if (editingModel.Status == "InProgress") { bug.Status = Status.InProgress; }
                if (editingModel.Status == "Fixed") { bug.Status = Status.Fixed; }
                if (editingModel.Status == "Closed") { bug.Status = Status.Closed; }
            }

            db.SaveChanges();

            return this.Ok(new {Message = "Bug #" + bug.Id + " patched."});
        }

        // DELETE: api/bugs/{id}
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteBugById(int id)
        {
            var bug = db.Bugs.FirstOrDefault(b => b.Id == id);

            if (bug == null)
            {
                return this.NotFound();
            }

            db.Bugs.Remove(bug);
            db.SaveChanges();

            return this.Ok(new { Message = "Bug #" + bug.Id + " deleted." });
        }

        // GET: api/bugs/filter
        [HttpGet]
        [Route("filter")]
        public IHttpActionResult ListBugsByFilter(
            [FromUri] string keyword = null,
            [FromUri] string statuses = null,
            [FromUri] string author = null)
        {
            var bugs = db.Bugs.ToList().AsQueryable();

            if (keyword != null)
            {
                bugs = bugs.Where(b => b.Title.Contains(keyword));
            }

            if (statuses != null)
            {
                string[] givenStatuses = statuses.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                bugs = bugs.Where(b => givenStatuses.Contains(b.Status.ToString()));
            }

            if (author != null)
            {
                bugs = bugs.Where(b => b.Author != null && b.Author.UserName == author);
            }

            var bugsByFilter = bugs
                .OrderByDescending(b => b.Id)
                .Select(BugViewModel.Create);

            return this.Ok(bugsByFilter);
        }
    }
}
