namespace web_api.Models
{
    public class AuthenticatedResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
