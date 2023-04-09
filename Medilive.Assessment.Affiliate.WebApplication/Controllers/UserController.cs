using MappingProviderCore.Abstract;
using Medilive.Assessment.Affiliate.Business;
using Medilive.Assessment.Affiliate.Dto;
using Medilive.Assessment.Affiliate.Dto.UserManagement;
using Medilive.Assessment.Core.Abstract.RuleEngine;
using Microsoft.AspNetCore.Mvc;

namespace Medilive.Assessment.Affiliate.WebApplication.Controllers
{
    public class UserController : Controller
    {
        private readonly AffiliateUserManager _affiliateUserManager;
        private readonly IMappingServiceProvider _mappingServiceProvider;
        private readonly IRuleServiceProvider _ruleServiceProvider;
        public UserController(AffiliateUserManager affiliateUserManager, IRuleServiceProvider ruleServiceProvider, IMappingServiceProvider mappingServiceProvider)
        {
            _affiliateUserManager = affiliateUserManager;
            _mappingServiceProvider = mappingServiceProvider;
            _ruleServiceProvider = ruleServiceProvider;
        }

        [HttpGet]
        [Obsolete]
        public IActionResult Register(string referralCode)
        {
            //If referral code is set
            if (!string.IsNullOrEmpty(referralCode))
            {
                // An ambiguous or suspicious request
                var result = _affiliateUserManager.SetReferralCodeCookie(referralCode);
                if (!result) 
                {
                    return BadRequest();
                }
            }
            return View();
        }

        [HttpPut]
        [Consumes("application/json")]
        [Obsolete]
        public JsonResult Register([FromBody]Request<RegisterAffiliateUserDto> request)
        {
            var response = new Response<RegisterAffiliateUserResultDto>();

            //TODO: IF WE USE A METHOD INTERCEPTOR AND EXECUTE FOLLOWING CODE BLOCK WITHIN THE INTERCEPTOR, IT WOULD BE A BETTER APPROACH
            #region Validations and Rule Checks
            var ruleCheckResult = _ruleServiceProvider.ApplyRules("Register", request.Data) as Rule;

            var referralCodeLimitExceeded = _affiliateUserManager.CheckIfRefererralCodeLimitExceeded();
            if (referralCodeLimitExceeded)
            {
                response.Messages = new List<ResponseMessageDto>();
                response.Messages.Add(new ResponseMessageDto { Text = "An error occured, contact to system administrator" });
            }

            if (ruleCheckResult != null && ruleCheckResult.HasErrors)
            {
                var messages = ruleCheckResult.GetMessages();
                response.Messages = _mappingServiceProvider.Map<List<ResponseMessageDto>>(messages);
                return Json(response);
            }
            #endregion End Of Validations and Rule Checks

            response.IsSuccessful = true;
            response.Data = _affiliateUserManager.Register(request.Data);
            return Json(response);
        }
        public IActionResult UserHome()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetUserInfo()
        {
            var response = new Response<AffiliateUserDto>();
            response.Data = _affiliateUserManager.GetUserInfo();
            response.IsSuccessful = true;
            return Json(response);
        }
        [HttpGet]
        public JsonResult GetUserSummary()
        {
            var response = new Response<UserSummaryDto>();
            var claim = User.Claims.SingleOrDefault(claim => claim.Type == "UserType");
            if (claim.Value != "ADMINISTRATOR")
            {
                return Json(response);
            }
            response.Data = _affiliateUserManager.GetUserSummary();
            response.IsSuccessful = true;
            return Json(response);
        }
    }
}
