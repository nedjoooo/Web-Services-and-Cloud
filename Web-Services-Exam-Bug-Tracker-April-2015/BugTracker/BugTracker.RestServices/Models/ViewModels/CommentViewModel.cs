using System;
using System.Linq.Expressions;
using BugTracker.Data.Models;

namespace BugTracker.RestServices.Models.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string Author { get; set; }

        public DateTime DateCreated { get; set; }
    }
}