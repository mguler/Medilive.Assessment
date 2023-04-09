using MappingProviderCore.Abstract;
using Medilive.Assessment.Affiliate.Data.Model.UserManagement;
using Medilive.Assessment.Affiliate.Dto.UserManagement;
using Medilive.Assessment.Core.Abstract.Data;
using Medilive.Assessment.Core.Abstract.Jwt;
using Medilive.Assessment.Core.Extensions;
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
            , IMappingServiceProvider mappingServiceProvider
            ,IHttpContextAccessor httpContextAccessor
            , IJwtHelper jwtHelper
            ,IConfiguration config
            , GoogleReCaptchaService googleReCaptchaService)
        {
            _dataRepository = dataRepository;
            _mappingServiceProvider = mappingServiceProvider;
            _httpContextAccessor = httpContextAccessor;
            _jwtHelper = jwtHelper;
            _config= config;
            _googleReCaptchaService = googleReCaptchaService;
        }

        public virtual bool CheckIfRefererralCodeLimitExceeded()
        {
            var ipNumber = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().Address;
            var identificationCookie = _httpContextAccessor.HttpContext.Request.Cookies["IdentificationCookie"];
            var last24hours = DateTime.Now.ToUnixTimeLong() - 86400000; //86400000 miliseconds = 24 Hours  

            // Check if the reference code trial limit has been exceeded more than 2 times within the last 24 hours.
            var refererralCodeLimitExceeded = _dataRepository.Get<RegistrationReferralCodeAudit>().Count(registrationReferralCodeAudit =>
                (registrationReferralCodeAudit.IdentificationCookie == identificationCookie
                || registrationReferralCodeAudit.IpNumber == ipNumber)
                && registrationReferralCodeAudit.AttemptOn >= last24hours) > 2;

            return refererralCodeLimitExceeded;
        }

        [Obsolete]
        public virtual bool SetReferralCodeCookie(string referralCode)
        {
            var ipNumber = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().Address;
            var identificationCookie = _httpContextAccessor.HttpContext.Request.Cookies["IdentificationCookie"];
            var last24hours = DateTime.Now.ToUnixTimeLong() - 86400000; //86400000 miliseconds = 24 Hours  

            // Check if the reference code trial limit has been exceeded more than 2 times within the last 24 hours.
            var refererralCodeLimitExceeded = _dataRepository.Get<RegistrationReferralCodeAudit>().Count(registrationReferralCodeAudit =>
                (registrationReferralCodeAudit.IdentificationCookie == identificationCookie
                || registrationReferralCodeAudit.IpNumber == ipNumber)
                && registrationReferralCodeAudit.AttemptOn >= last24hours) > 20;

            var referralTokenValidationKey = _config["Application:Keys:ReferralTokenValidationKey"];
            var referralCodeValidationResult = _jwtHelper.ValidateToken(referralTokenValidationKey, referralCode);

            //if referral code attempt limit exceeded or something went wrong during referral code validation
            if (!referralCodeValidationResult.IsSuccessful || refererralCodeLimitExceeded)
            {
                var referralCodeAudit = new RegistrationReferralCodeAudit
                {
                    IpNumber = ipNumber,
                    IdentificationCookie = identificationCookie,
                    ReferralCode = referralCode,
                    AttemptOn = DateTime.Now.ToUnixTimeLong(),
                    IsSuccessfull = false,
                };

                _dataRepository.Save(referralCodeAudit);
                return false;

            }
            _httpContextAccessor.HttpContext.Response.Cookies.Append("referralCode", referralCode);
            return true;
        }

        [Obsolete]
        public virtual RegisterAffiliateUserResultDto Register(RegisterAffiliateUserDto registerAffiliateUser)
        {
            var result = new RegisterAffiliateUserResultDto();
            var affiliateUser = _mappingServiceProvider.Map<AffiliateUser>(registerAffiliateUser);
            var referralCode = _httpContextAccessor.HttpContext.Request.Cookies["referralCode"];
            var ipNumber = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().Address;
            var identificationCookie = _httpContextAccessor.HttpContext.Request.Cookies["IdentificationCookie"];

            var referralCodeAlreadyApplied = _dataRepository.Get<RegistrationReferralCodeAudit>().Any(registrationReferralCodeAudit =>
                registrationReferralCodeAudit.ReferralCode == referralCode && registrationReferralCodeAudit.IsSuccessfull);

            var referralTokenValidationKey = _config["Application:Keys:ReferralTokenValidationKey"];
            var referralCodeValidationResult = _jwtHelper.ValidateToken(referralTokenValidationKey, referralCode);

            //Maybe we should block the user if the referral code already applied
            if (referralCodeValidationResult.IsSuccessful & !referralCodeAlreadyApplied)
            {
                affiliateUser.UserType = UserType.Values.ADMINISTRATOR;
            }
            else 
            {
                affiliateUser.UserType = UserType.Values.NORMAL_USER;
            }

            _dataRepository.Save(affiliateUser);

            if (affiliateUser.Id != 0 && referralCodeValidationResult.IsSuccessful)
            {
                var referralCodeAudit = new RegistrationReferralCodeAudit
                {
                    IpNumber = ipNumber,
                    IdentificationCookie = identificationCookie,
                    ReferralCode = referralCode,
                    AttemptOn = DateTime.Now.ToUnixTimeLong(),
                    IsSuccessfull = !referralCodeAlreadyApplied, // referans kodu zaten daha onceden kullanilmis ise bunu loglara basarisiz ref kodu girismi olrak ekle
                };
                _dataRepository.Save(referralCodeAudit);
            }

            //TODO:Implement a logic here to send notifications to approve contact information

            // Remove referral code from cookie
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("referralCode");
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
