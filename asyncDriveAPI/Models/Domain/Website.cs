using System.ComponentModel.DataAnnotations.Schema;

namespace asyncDriveAPI.Models.Domain
{
    public class Website
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DNS { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public string JsonData { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        // Foreign Key to ApplicationUser
        public string UserId { get; set; }
        public ApplicationUser User { get; set; } // Navigation property to the User
    }

}
