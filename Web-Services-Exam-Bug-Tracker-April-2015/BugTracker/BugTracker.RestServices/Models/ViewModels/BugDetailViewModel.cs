using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BugTracker.RestServices.Models.ViewModels
{
    using System;
    using System.Linq.Expressions;
    using BugTracker.Data.Models;

    public class BugDetailViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public string Author { get; set; }

        public DateTime DateCreated { get; set; }

        public IEnumerable<CommentDetailViewModel> Comments { get; set; }

        public static Expression<Func<Bug, BugDetailViewModel>> Create
        {
            get
            {
                return b => new BugDetailViewModel()
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,                    
                    Status = b.Status.ToString(),
                    Author = b.Author.UserName,
                    DateCreated = b.SubmitDate,
                    Comments = b.Comments
                        .OrderByDescending(c => c.PublishDate)
                        .ThenByDescending(c => c.Id)
                        .Select(c => new CommentDetailViewModel()
                        {
                            Id = c.Id,
                            Text = c.Text,
                            Author = c.Author.UserName,
                            DateCreated = c.PublishDate
                        })
                };
            }
        }
    }
}