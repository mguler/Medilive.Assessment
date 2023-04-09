using Medilive.Assessment.Core.Abstract.Jwt;
using System.Security.Claims;

namespace Medilive.Assessment.Core.Tools.Jwt
{
    public class JwtValidationResult : IJwtValidationResult
    {
        public bool IsSuccessful { get; set; }
        public Claim[] Claims  { get; set; }
    }
}
