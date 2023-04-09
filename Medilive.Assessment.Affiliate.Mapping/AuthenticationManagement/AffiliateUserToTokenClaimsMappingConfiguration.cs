using MappingProviderCore.Abstract;
using Medilive.Assessment.Affiliate.Data.Model.UserManagement;
using Medilive.Assessment.Affiliate.Dto.UserManagement;

namespace Medilive.Assessment.Affiliate.Mapping.Configuration.UserManagement
{
    public class AffiliateUserToTokenClaimsMappingConfiguration : IMappingConfiguration
    {
        public void Configure(IMappingServiceProvider mappingServiceProvider)
        {
            mappingServiceProvider.Register<AffiliateUser, Dictionary<string, string>>((source, target) =>
            {
                target.Add("Id", source.Id.ToString());
                target.Add("Name", source.Name);
                target.Add("Lastname", source.Lastname);
                //result.Add("Birthdate",source.Birthdate,);
                target.Add("GenderId", source.Gender.ToString());
                target.Add("Username", source.Username);
                target.Add("UserType", source.UserType.ToString());
                target.Add("Phone", source.Phone);
                target.Add("Email", source.Email);
                return target;
            });
        }
    }
}
