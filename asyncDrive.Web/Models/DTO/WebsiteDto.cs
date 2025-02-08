

namespace asyncDrive.Web.Models.DTO
{
    public class WebsiteDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DNS { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public string JsonData { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UserId { get; set; } // Represents the ID of the user who owns the website
    }
    public class CreateWebsiteDto
    {
        public string Title { get; set; }
        public string DNS { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public string JsonData { get; set; }
        public string UserId { get; set; }
    }
    public class UpdateWebsiteDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DNS { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public string JsonData { get; set; }
    }
    public class WebsiteDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DNS { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public string JsonData { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; } // Added from the User entity
        public string UserEmail { get; set; } // Added from the User entity
    }
    public class WebsiteSummaryDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DNS { get; set; }
        public bool Status { get; set; }
    }
}
