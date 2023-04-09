using MappingProviderCore.Abstract;
using Medilive.Assessment.Affiliate.Data.Model.UserManagement;
using Medilive.Assessment.Affiliate.Dto.ReferenceDataManagement;
using Medilive.Assessment.Affiliate.Dto.UserManagement;
using System.Security.Cryptography;
using System.Text;

namespace Medilive.Assessment.Affiliate.Mapping.Configuration.UserManagement
{
    public class GenderToReferenceDataDtoMappingConfiguration : IMappingConfiguration
    {
        public void Configure(IMappingServiceProvider mappingServiceProvider)
        {
            mappingServiceProvider.Register<Gender, ReferenceDataDto>((source, target) =>
            {
                target.Name = source.Name;
                target.Id = source.Id;
                return target;
            });

            mappingServiceProvider.Register<List<Gender>, List<ReferenceDataDto>>((source, target) =>
            {
                source.ForEach(item => target.Add(mappingServiceProvider.Map<ReferenceDataDto>(item)));
                return target;
            });
        }
    }
}
