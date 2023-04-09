using MappingProviderCore.Abstract;
using Medilive.Assessment.Affiliate.Data.Model.UserManagement;
using Medilive.Assessment.Affiliate.Dto.UserManagement;
using System.Security.Cryptography;
using System.Text;

namespace Medilive.Assessment.Affiliate.Mapping.Configuration.UserManagement
{
    public class AffiliateUserToAffiliateUserDtoMappingConfiguration : IMappingConfiguration
    {
        public void Configure(IMappingServiceProvider mappingServiceProvider)
        {
            mappingServiceProvider.Register<AffiliateUser, AffiliateUserDto>((source, target) =>
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    var password = Convert.ToBase64String(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(source.Password)));
                    target.Name = source.Name;
                    target.Lastname = source.Lastname;
                    target.Username = source.Username;
                    target.Gender = source.Gender == Gender.Values.NONE ? "Undefined" : source.Gender == Gender.Values.FEMALE ? "Female" : "Male";
                    target.UserType = source.UserType == UserType.Values.ADMINISTRATOR ? "Administrator" : "Standart User";
                    target.Phone = source.Phone;
                    target.Email = source.Email;
                }
                return target;
            });
        }
    }
}
