using Presentation.Controllers.v1;
using Swashbuckle.AspNetCore.Filters;

namespace Presentation.Doc.Samples
{
    public class LoginRequestExample : IExamplesProvider<LoginRequest>
    {
        public LoginRequest GetExamples()
        {
            return new LoginRequest
            {
                UserId = 1,
                FullName = "João da Silva",
                Email = "joao.silva@example.com",
                Role = "USER"
            };
        }
    }

    public class TokenResponseExample : IExamplesProvider<TokenResponse>
    {
        public TokenResponse GetExamples()
        {
            return new TokenResponse
            {
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
                ExpiresIn = 7200,
                Type = "Bearer"
            };
        }
    }
}
