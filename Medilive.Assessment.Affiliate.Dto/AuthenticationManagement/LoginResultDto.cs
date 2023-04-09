namespace Medilive.Assessment.Affiliate.Dto.AuthenticationManagement
{
    public class LoginResultDto
    {
        public bool IsLoggedIn { get; set; }
        public string Token { get; set; }
        public Dictionary<string,string> UserData { get; set; }
    }
}
