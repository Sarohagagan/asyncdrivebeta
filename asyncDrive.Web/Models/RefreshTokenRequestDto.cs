namespace asyncDrive.Web.Models
{
    public class RefreshTokenRequestDto
    {
        public string UserId { get; set; }
        public string RefreshToken { get; set; }
    }
}