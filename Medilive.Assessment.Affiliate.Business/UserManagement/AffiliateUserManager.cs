using MappingProviderCore.Abstract;
using Medilive.Assessment.Affiliate.Data.Model.UserManagement;
using Medilive.Assessment.Affiliate.Dto.UserManagement;
using Medilive.Assessment.Core.Abstract.Data;
using Medilive.Assessment.Core.Abstract.Jwt;
using Medilive.Assessment.Core.Tools.ReCaptcha;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Medilive.Assessment.Affiliate.Business
{
    public class AffiliateUserManager
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMappingServiceProvider _mappingServiceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtHelper _jwtHelper;
        private readonly IConfiguration _config;
        private readonly GoogleReCaptchaService _googleReCaptchaService;
        public AffiliateUserManager(IDataRepository dataRepository
            ,IMappingServiceProvider mappingServiceProvider
            ,IHttpContextAccessor httpContextAccessor
            ,IJwtHelper jwtHelper
            ,IConfiguration config
            ,GoogleReCaptchaService googleReCaptchaService)
        {
            _dataRepository = dataRepository;
            _mappingServiceProvider = mappingServiceProvider;
            _httpContextAccessor = httpContextAccessor;
            _jwtHelper = jwtHelper;
            _config= config;
            _googleReCaptchaService = googleReCaptchaService;
        }

        [Obsolete]
        public virtual RegisterAffiliateUserResultDto Register(RegisterAffiliateUserDto registerAffiliateUser)
        {
            var result = new RegisterAffiliateUserResultDto();
            var affiliateUser = _mappingServiceProvider.Map<AffiliateUser>(registerAffiliateUser);

            //Move following line into the mapper
            affiliateUser.UserType = string.IsNullOrEmpty(registerAffiliateUser.ReferralCode) ? UserType.Values.NORMAL_USER
                : UserType.Values.ADMINISTRATOR;

            _dataRepository.Save(affiliateUser);

            //TODO:Implement a logic here to send notifications to approve contact information
            result.IsSuccessful = true;
            return result;
        }
        public virtual SaveAffiliateUserResultDto Save(AffiliateUserDto user)
        {
            var result = new SaveAffiliateUserResultDto();
            var affiliateUser = _mappingServiceProvider.Map<AffiliateUser>(user);
            _dataRepository.Save(affiliateUser);

            //TODO:Implement a logic here to send notifications to approve contact information (if changed)

            result.IsSuccessful = true;
            return result;
        }
        public virtual AffiliateUserDto GetUserInfo() 
        {
            var userId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "Id").Value);
            var affiliateUser = _dataRepository.Get<AffiliateUser>().SingleOrDefault(model => model.Id == userId);
            var result = _mappingServiceProvider.Map<AffiliateUserDto>(affiliateUser);
            return result;
        }
        public UserSummaryDto GetUserSummary() 
        {
            var result = new UserSummaryDto();
            result.Administrators = _dataRepository.Get<AffiliateUser>().Count(affiliateUser => affiliateUser.UserType == UserType.Values.ADMINISTRATOR);
            result.Customers = _dataRepository.Get<AffiliateUser>().Count(affiliateUser => affiliateUser.UserType == UserType.Values.NORMAL_USER);
            return result;
        }
    }
}
