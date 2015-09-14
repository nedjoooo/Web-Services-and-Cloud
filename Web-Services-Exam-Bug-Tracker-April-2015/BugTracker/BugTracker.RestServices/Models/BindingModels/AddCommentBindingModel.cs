using System.ComponentModel.DataAnnotations;

namespace BugTracker.RestServices.Models.BindingModels
{
    public class AddCommentBindingModel
    {
        [Required]
        public string Text { get; set; }
    }
}