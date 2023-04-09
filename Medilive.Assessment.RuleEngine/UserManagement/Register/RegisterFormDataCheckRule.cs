using Medilive.Assessment.Affiliate.Data.Model.UserManagement;
using Medilive.Assessment.Affiliate.Dto.UserManagement;
using Medilive.Assessment.Core.Abstract.Data;
using Medilive.Assessment.Core.Abstract.RuleEngine;
using Medilive.Assessment.Core.Extensions;
using Medilive.Assessment.Core.Tools.ReCaptcha;

namespace Medilive.Assessment.Rules.Configuration.AuthenticationManagement.Login
{
    public class RegisterFormDataCheckRule : Rule
    {
        public RegisterFormDataCheckRule(IDataRepository dataRepository, GoogleReCaptchaService _googleReCaptchaService) {
            
            Define<RegisterAffiliateUserDto>((userInfo) => {

                // Check if the model is null
                if (userInfo == null)
                {
                    AddMessage("One or more fields are not valid. Please check the form"
                        , "FormValidationGeneral", Priority.Error);
                    return;
                }

                var isHuman = _googleReCaptchaService.IsHuman(userInfo.Captcha);
                if (!isHuman)
                {
                    AddMessage("You couldn't passed the captcha validation", "FormValidation", "captcha", Priority.Error);
                    return;
                }

                // Checks whether a given text has a length of at least 2 and at most 64 characters,
                // consists of only letters and spaces, and each word is composed of at least 2 letters.
                if (userInfo.Name == null || !userInfo.Name.IsMatch("^(?=.{2,64}$)[a-zA-ZğüşıöçĞÜŞİÖÇ]+(?:\\s[a-zA-ZğüşıöçĞÜŞİÖÇ]+)*$"))
                {
                    AddMessage("The name field can be between 2 and 64 characters long and can only consist of uppercase/lowercase letters and space characters."
                        , "FormValidation", "name", Priority.Error);
                }

                // Checks whether the field is between 2 and 32 characters long and consists only of uppercase or lowercase letters.
                if (userInfo.Lastname == null || !userInfo.Lastname.IsMatch("^[a-zA-ZğüşıöçĞÜŞİÖÇ]{2,64}$"))
                {
                    AddMessage("The lastname field must be between 2 and 32 characters long and consist only of uppercase and lowercase letters."
                        , "FormValidation", "lastname", Priority.Error);
                }

                // Checks whether the entered information conforms to the email template.
                if (userInfo.Email == null || !userInfo.Email.IsMatch("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$"))
                {
                    AddMessage("It must conform to the email format."
                        , "FormValidation", "email", Priority.Error);
                }
                else
                {
                    // Checks whether the entered email address is already registered.
                    var isEmailAlreadyExists = dataRepository.Get<AffiliateUser>().Any(affiliateUser => affiliateUser.Email == userInfo.Email);
                    if (isEmailAlreadyExists)
                    {
                        AddMessage("The email address you entered is already in our records.", "FormValidation", "Email", Priority.Error);
                    }
                }


                // Checks whether the phone number is 10 characters long and consists only of digits.
                if (userInfo.Phone == null || !userInfo.Phone.IsMatch("[0-9]{10}"))
                {
                    AddMessage("The phone number must be 10 characters long and consist only of digits."
                        , "FormValidation", "phone", Priority.Error);
                }
                else
                {
                    // Checks whether the entered phone number is already registered.
                    var isPhoneNumberAlreadyExists = dataRepository.Get<AffiliateUser>().Any(affiliateUser => affiliateUser.Phone == userInfo.Phone);
                    if (isPhoneNumberAlreadyExists)
                    {
                        AddMessage("The phone number you entered is already in our records.", "FormValidation", "Email", Priority.Error);
                    }
                }

                // Checks whether the field is 8-16 characters long and consists of uppercase/lowercase letters, digits
                // , and underscore characters.
                if (userInfo.Username == null || !userInfo.Username.IsMatch("[a-zA-z0-9_]{5,16}"))
                {
                    AddMessage("This field can only be 5-16 characters long and consist only of uppercase/lowercase letters, digits, and underscore characters."
                        , "FormValidation", "username", Priority.Error);

                    var isUserExists = dataRepository.Get<AffiliateUser>().Any(affiliateUser => affiliateUser.Username == userInfo.Username);

                    if (isUserExists)
                    {
                        AddMessage("The username you have selected has already been used by someone else", "Username", "FormValidation", Priority.Error);
                    }

                }

                // Checks whether a password with at least one digit, one uppercase letter, one lowercase letter,
                // and one special character, and with a length of at least 8 and at most 16 characters, has been entered or not 
                if (userInfo.Password == null || !userInfo.Password.IsMatch("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[^\\w\\d\\s:])([^\\s]){8,16}$"))
                {
                    AddMessage("The password must be at least 8 characters long and no more than 16 characters long, and it must contain at least one uppercase letter, one lowercase letter, one digit, and one special character."
                        , "FormValidation", "password", Priority.Error);
                }

                // Checks whether two passwords match or not.
                if (userInfo.RePassword == null || userInfo.Password != userInfo.RePassword)
                {
                    AddMessage("The passwords entered in both fields must be the same."
                        , "FormValidation", "repassword", Priority.Error);
                }
            });
        }
    }
}
