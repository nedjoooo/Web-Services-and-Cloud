using System.Linq.Expressions;
using BugTracker.Data.Models;

namespace BugTracker.RestServices.Models.ViewModels
{
    using System;

    public class CommentDetailViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string Author { get; set; }

        public DateTime DateCreated { get; set; }

        public int BugId { get; set; }

        public string BugTitle { get; set; }

        public static Expression<Func<Comment, CommentDetailViewModel>> Create
        {
            get
            {
                return c => new CommentDetailViewModel()
                {
                    Id = c.Id,
                    Text = c.Text,
                    Author = c.Author.UserName,
                    DateCreated = c.PublishDate,
                    BugId = c.BugId,
                    BugTitle = c.Bug.Title
                };
            }
        }
    }
}