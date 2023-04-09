using MappingProviderCore.Abstract;
using Medilive.Assessment.Affiliate.Data.Model.UserManagement;
using Medilive.Assessment.Affiliate.Dto.UserManagement;
using System.Security.Cryptography;
using System.Text;

namespace Medilive.Assessment.Affiliate.Mapping.Configuration.UserManagement
{
    public class RegisterAffiliateUserDtoToAffiliateUserMappingConfiguration : IMappingConfiguration
    {
        public void Configure(IMappingServiceProvider mappingServiceProvider)
        {
            mappingServiceProvider.Register<RegisterAffiliateUserDto, AffiliateUser>((source, target) =>
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    var password = Convert.ToBase64String(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(source.Password)));
                    target.Name = source.Name;
                    target.Lastname = source.Lastname;
                    target.Username = source.Username;
                    target.Gender = (Gender.Values)source.Gender;
                    target.Password = password;
                    target.Phone = source.Phone;
                    target.Email = source.Email;
                }
                return target;
            });
        }
    }
}
