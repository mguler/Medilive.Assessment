using Medilive.Assessment.Affiliate.Data.Model.UserManagement;
using Medilive.Assessment.Affiliate.Dto.AuthenticationManagement;
using Medilive.Assessment.Core.Abstract.Data;
using Medilive.Assessment.Core.Abstract.RuleEngine;
using Medilive.Assessment.Core.Extensions;
using Medilive.Assessment.Core.Tools.ReCaptcha;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Medilive.Assessment.Rules.Configuration.AuthenticationManagement.Login
{
    public class LoginUsernamePasswordCheckRule : Rule
    {
        public LoginUsernamePasswordCheckRule(IDataRepository dataRepository, GoogleReCaptchaService _googleReCaptchaService) {

            Define<LoginDto>((loginInfo) => {


                // Check if the model is null
                if (loginInfo == null)
                {
                    AddMessage("One or more fields are not valid. Please check the form"
                        , "FormValidationGeneral", Priority.Error);
                    return;
                }

                var isHuman = _googleReCaptchaService.IsHuman(loginInfo.Captcha);
                if (!isHuman)
                {
                    AddMessage("You couldn't passed the captcha validation", "LoginError", Priority.Error);
                    return;
                }

                if (!loginInfo.Username.IsMatch("[a-zA-z0-9_]{5,16}")
                    || !loginInfo.Password.IsMatch("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[^\\w\\d\\s:])([^\\s]){8,16}$"))
                {
                    AddMessage("Username and/or password doesn't match", "LoginError", Priority.Error);
                    return;
                }

                // Check if the user is exists
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    var password = Convert.ToBase64String(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(loginInfo.Password)));
                    var isUserExists = dataRepository.Get<AffiliateUser>().Any(affiliateUser => affiliateUser.Username == loginInfo.Username
                    && affiliateUser.Password == password);

                    if (!isUserExists)
                    {
                        AddMessage("Username and/or password doesn't match", "LoginError", "FormValidation", Priority.Error);
                    }
                }
            });
        }
    }
}
