using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using BugTracker.Data.Models;

namespace BugTracker.RestServices.Models.ViewModels
{
    public class BugViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Status { get; set; }

        public string Author { get; set; }

        public DateTime DateCreated { get; set; }

        public static Expression<Func<Bug, BugViewModel>> Create
        {
            get
            {
                return b => new BugViewModel()
                {
                    Id = b.Id,
                    Author = b.Author != null ? b.Author.UserName : null,
                    Status = b.Status.ToString(),
                    Title = b.Title,
                    DateCreated = b.SubmitDate
                };
            }
        }
    }
}