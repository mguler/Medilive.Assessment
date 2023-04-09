using System.Security.Claims;

namespace Medilive.Assessment.Core.Abstract.Jwt
{
    public interface IJwtValidationResult
    {
        public bool IsSuccessful { get; set; }
        public Claim[] Claims { get; set; }
    }
}
