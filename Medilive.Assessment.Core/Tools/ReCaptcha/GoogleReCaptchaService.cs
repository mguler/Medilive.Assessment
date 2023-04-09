using Medilive.Assessment.Core.Extensions;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace Medilive.Assessment.Core.Tools.ReCaptcha
{
    /// <summary>
    /// ReCaptcha registration link : https://www.google.com/recaptcha/admin/create
    /// </summary>
    public class GoogleReCaptchaService
    {
        private readonly string _url;
        private readonly string _privateKey;
        public GoogleReCaptchaService(string url,string privateKey) 
        {
            _url = url;
            _privateKey = privateKey;
        }
        public bool IsHuman(string response) 
        {
            using (var webClient = new WebClient()) 
            {
                var form = new NameValueCollection();
                form.Add("secret", _privateKey);
                form.Add("response", response);
                var apiResponse = webClient.UploadValues(_url, "POST", form);

                var responseString = Encoding.UTF8.GetString(apiResponse);
                var result = responseString.IsMatch("\"success\": true");
                return result;
            }
        }
    }
}
