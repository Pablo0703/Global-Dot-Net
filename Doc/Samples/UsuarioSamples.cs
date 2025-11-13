using Domain.Entities;

namespace Presentation.Doc.Samples
{
    public static class UsuarioSamples
    {
        // 📥 Request
        public static readonly Usuario UsuarioRequest = new Usuario
        {
            FullName = "João da Silva",
            Email = "joao@email.com",
            Password = "123456",
            Role = Usuario.USER,
            Bio = "Desenvolvedor .NET e mentor voluntário",
            Location = "São Paulo - SP",
            Timezone = "America/Sao_Paulo",
            LinkedinUrl = "https://linkedin.com/in/joao"
        };

        // 📤 Response
        public static readonly object UsuarioResponse = new
        {
            Id = Guid.NewGuid(),
            FullName = "João da Silva",
            Email = "joao@email.com",
            Role = "USER",
            TimeCredits = 10.0,
            AverageRating = 4.8,
            Location = "São Paulo - SP",
            CreatedDate = DateTime.UtcNow
        };
    }
}
