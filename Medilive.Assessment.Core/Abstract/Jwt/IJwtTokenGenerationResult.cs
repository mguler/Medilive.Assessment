namespace Medilive.Assessment.Core.Abstract.Jwt
{
    public interface IJwtTokenGenerationResult
    {
        public bool IsSuccessful { get; set; }
        public string Token { get; set; }
    }
}
