namespace Medilive.Assessment.Affiliate.Dto.UserManagement
{
    public class RegisterAffiliateUserDto
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public int? Gender { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Captcha { get; set; }
        public string ReferralCode { get; set; }
    }
}
