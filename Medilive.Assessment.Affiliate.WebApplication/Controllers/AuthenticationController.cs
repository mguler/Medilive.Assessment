using MappingProviderCore.Abstract;
using Medilive.Assessment.Affiliate.Business;
using Medilive.Assessment.Affiliate.Dto;
using Medilive.Assessment.Affiliate.Dto.AuthenticationManagement;
using Medilive.Assessment.Core.Abstract.RuleEngine;
using Microsoft.AspNetCore.Mvc;

namespace Medilive.Assessment.Affiliate.WebApplication.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly AuthenticationManager _authenticationManager;
        private readonly IMappingServiceProvider _mappingServiceProvider;
        private readonly IRuleServiceProvider _ruleServiceProvider;
        public AuthenticationController(AuthenticationManager authenticationManager, IRuleServiceProvider ruleServiceProvider, IMappingServiceProvider mappingServiceProvider)
        {
            _authenticationManager = authenticationManager;
            _mappingServiceProvider = mappingServiceProvider;
            _ruleServiceProvider = ruleServiceProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Consumes("application/json")]
        public JsonResult Login([FromBody]Request<LoginDto> request)
        {
            var response = new Response<LoginResultDto>();

            //TODO: IF WE USE A METHOD INTERCEPTOR AND EXECUTE FOLLOWING CODE BLOCK WITHIN THE INTERCEPTOR, IT WOULD BE A BETTER APPROACH
            #region Validations and Rule Checks
            var ruleCheckResult = _ruleServiceProvider.ApplyRules("Login", request.Data) as Rule;

            if (ruleCheckResult != null && ruleCheckResult.HasErrors)
            {
                var messages = ruleCheckResult.GetMessages();
                response.Messages = _mappingServiceProvider.Map<List<ResponseMessageDto>>(messages);
                return Json(response);
            }
            #endregion End Of Validations and Rule Checks

            response.Data = _authenticationManager.Login(request.Data);
            response.IsSuccessful = true;
            return Json(response);
        }
        [HttpGet]
        public IActionResult Logout()
        {
            var response = new Response<LoginResultDto>();
            _authenticationManager.Logout();
            response.IsSuccessful = true;
            return Redirect("/redirect?message=You have logged out, you will be redirected to the homepage within 5 seconds.&url=/");
        }
    }
}
