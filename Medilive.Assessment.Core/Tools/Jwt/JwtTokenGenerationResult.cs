using Medilive.Assessment.Core.Abstract.Jwt;

namespace Medilive.Assessment.Core.Tools.Jwt
{
    public class JwtTokenGenerationResult : IJwtTokenGenerationResult
    {
        public bool IsSuccessful { get; set; }
        public string Token { get; set; }
    }
}
