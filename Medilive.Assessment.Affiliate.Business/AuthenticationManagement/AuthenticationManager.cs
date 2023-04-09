using MappingProviderCore.Abstract;
using Medilive.Assessment.Affiliate.Data.Model.UserManagement;
using Medilive.Assessment.Affiliate.Dto.AuthenticationManagement;
using Medilive.Assessment.Core.Abstract.Data;
using Medilive.Assessment.Core.Abstract.Jwt;
using Medilive.Assessment.Core.Tools.ReCaptcha;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Medilive.Assessment.Affiliate.Business
{
    public class AuthenticationManager
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMappingServiceProvider _mappingServiceProvider;
        private readonly IJwtHelper _jwtHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        public AuthenticationManager(IDataRepository dataRepository
            , IMappingServiceProvider mappingServiceProvider
            , IJwtHelper jwtHelper
            , IHttpContextAccessor httpContextAccessor
            ,IConfiguration config)
        {
            _dataRepository = dataRepository;
            _mappingServiceProvider = mappingServiceProvider;
            _jwtHelper = jwtHelper;
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }
        public LoginResultDto Login(LoginDto login) 
        {
            var result = new LoginResultDto();

            try
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    var passwordHash = Convert.ToBase64String(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(login.Password)));
                    var authTokenValidationKey = _config["Application:Keys:AuthTokenValidationKey"];
                    var record = _dataRepository.Get<AffiliateUser>().SingleOrDefault(model => model.Username == login.Username
                            && model.Password == passwordHash);

                    var affiliateUser = _mappingServiceProvider.Map<Dictionary<string, string>>(record);
                    var jwt = _jwtHelper.GenerateToken(authTokenValidationKey, affiliateUser);
                    result.Token = jwt.Token;
                    result.IsLoggedIn = true;
                    _httpContextAccessor.HttpContext.Response.Cookies.Append("Token", jwt.Token);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }
        public LogoutDto Logout() 
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("Token");
            return new LogoutDto();
        }
    }
}
