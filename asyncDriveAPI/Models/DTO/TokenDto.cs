namespace asyncDriveAPI.Models.DTO
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
    public class RefreshTokenRequestDto
    {
        public string UserId { get; set; }
        public string RefreshToken { get; set; }
    }
}
