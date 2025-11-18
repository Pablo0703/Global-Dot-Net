using Application.Service.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Presentation.Doc.Samples;

namespace Presentation.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwt;

        public AuthController(JwtService jwt)
        {
            _jwt = jwt;
        }

        // ============================================================
        // 🔐 LOGIN / GERAR TOKEN
        // ============================================================
        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Realizar login",
            Description = "Recebe os dados do usuário e gera um token JWT válido por 2 horas. **Não possui validação real de credenciais aqui**, apenas gera o token."
        )]

        // 📌 Exemplos no Swagger
        [SwaggerRequestExample(typeof(LoginRequest), typeof(LoginRequestExample))]
        [SwaggerResponseExample(200, typeof(TokenResponseExample))]

        [SwaggerResponse(200, "Token gerado com sucesso", typeof(TokenResponse))]
        [SwaggerResponse(400, "Dados inválidos")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email))
                return BadRequest("Dados inválidos.");

            // 🔥 Gera token (no futuro valida no banco)
            var token = _jwt.GenerateToken(
                request.UserId,
                request.FullName,
                request.Email,
                request.Role
            );

            return Ok(new TokenResponse
            {
                Token = token,
                ExpiresIn = 7200,
                Type = "Bearer"
            });
        }
    }

    // ============================================================
    // 🔵 MODELO DO REQUEST
    // ============================================================
    public class LoginRequest
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "User";
    }

    // ============================================================
    // 🔵 MODELO DO RESPONSE
    // ============================================================
    public class TokenResponse
    {
        public string Token { get; set; } = "";
        public int ExpiresIn { get; set; }
        public string Type { get; set; } = "Bearer";
    }
}
