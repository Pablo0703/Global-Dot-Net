using Domain.Entities;

namespace Presentation.Doc.Samples
{
    public static class HabilidadeSamples
    {
        // 📥 Request
        public static readonly Habilidade HabilidadeRequest = new Habilidade
        {
            Nome = "C# Avançado",
            Categoria = Habilidade.TECNOLOGIA,
            Nivel = Habilidade.EXPERT,
            Descricao = "Mentoria completa sobre C#, ASP.NET e arquitetura.",
            IsOffering = true,
            ValorPorHora = 50,
            UsuarioId = Guid.NewGuid()
        };

        // 📤 Response
        public static readonly object HabilidadeResponse = new
        {
            Id = Guid.NewGuid(),
            Nome = "C# Avançado",
            Categoria = "TECNOLOGIA",
            Nivel = "EXPERT",
            Descricao = "Mentoria completa sobre C#, ASP.NET e arquitetura.",
            IsOffering = true,
            UsuarioId = Guid.NewGuid(),
            DataCriacao = DateTime.UtcNow
        };
    }
}
