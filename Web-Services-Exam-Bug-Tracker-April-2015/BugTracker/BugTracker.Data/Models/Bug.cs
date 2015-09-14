using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Data.Models
{
    public class Bug
    {
        public Bug()
        {
            this.Comments = new HashSet<Comment>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public Status Status { get; set; }

        public string AuthorId { get; set; }

        public virtual User Author { get; set; }

        public DateTime SubmitDate { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
