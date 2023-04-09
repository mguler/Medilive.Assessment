using MappingProviderCore.Abstract;
using Medilive.Assessment.Affiliate.Data.Model.UserManagement;
using Medilive.Assessment.Affiliate.Dto;
using Medilive.Assessment.Affiliate.Dto.UserManagement;
using Medilive.Assessment.Core.Abstract.RuleEngine;
using System.Text;

namespace Medilive.Assessment.Affiliate.Mapping.Configuration
{
    public class ErrorMessagesToResponseMessageMappingConfiguration : IMappingConfiguration
    {
        public void Configure(IMappingServiceProvider mappingServiceProvider)
        {
            mappingServiceProvider.Register<Message, ResponseMessageDto>((source, target) =>
            {
                target.Text = source.Text;
                target.Code = source.Code;
                target.Type = source.Type;
                target.Priority = source.Priority.ToString();
                return target;
            });

            mappingServiceProvider.Register<List<Message>, List<ResponseMessageDto>>((source, target) =>
            {
                source.ForEach(item => target.Add(mappingServiceProvider.Map<ResponseMessageDto>(item)));
                return target;
            });
        }
    }
}
