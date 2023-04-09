using System.Data.Common;

namespace Medilive.Assessment.Affiliate.Dto.UserManagement
{
    public class AffiliateUserDto
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public int GenderId { get; set; }
        public string UserType { get; set; }
        public int UserTypeId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public override string ToString()
        {
            return $"{Name} {Lastname}";
        }
    }
}
