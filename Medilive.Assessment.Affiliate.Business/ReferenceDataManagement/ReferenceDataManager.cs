using MappingProviderCore.Abstract;
using Medilive.Assessment.Affiliate.Data.Model.UserManagement;
using Medilive.Assessment.Affiliate.Dto.ReferenceDataManagement;
using Medilive.Assessment.Core.Abstract.Data;

namespace Medilive.Assessment.Affiliate.Business.ReferenceDataManagement
{
    public class ReferenceDataManager
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMappingServiceProvider _mappingServiceProvider;
        public ReferenceDataManager(IDataRepository dataRepository, IMappingServiceProvider mappingServiceProvider) 
        {
            _dataRepository = dataRepository;
            _mappingServiceProvider = mappingServiceProvider;
        }

        public ReferenceDataDto[] GetGenderList() 
        {
            var genderList = _dataRepository.Get<Gender>().ToList();
            var result = _mappingServiceProvider.Map<List<ReferenceDataDto>>(genderList);
            return result.ToArray();
        }
    }
}
