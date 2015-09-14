using BugTracker.Data.Models;

namespace BugTracker.RestServices.Models.BindingModels
{
    public class EditBindingModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }
    }
}