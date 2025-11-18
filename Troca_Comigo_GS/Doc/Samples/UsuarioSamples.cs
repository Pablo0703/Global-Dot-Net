using Domain.Entities;

namespace Presentation.Doc.Samples
{
    public static class UsuarioSamples
    {
        // 📥 REQUEST SAMPLE
        public static readonly Usuario UsuarioRequest = new Usuario
        {
            NomeCompleto = "João da Silva",
            Email = "joao@email.com",
            Password = "123456",
            Role = "USER",
            Bio = "Desenvolvedor .NET e mentor voluntário",
            Location = "São Paulo - SP",
            Timezone = "America/Sao_Paulo",
            LinkedinUrl = "https://linkedin.com/in/joao"
        };

        // 📤 RESPONSE SAMPLE
        public static readonly object UsuarioResponse = new
        {
            Id = 1,
            NomeCompleto = "João da Silva",
            Email = "joao@email.com",
            Role = "USER",
            TimeCredits = 10,
            AverageRating = 4.8,
            Location = "São Paulo - SP",
            CreatedDate = DateTime.UtcNow
        };
    }
}
