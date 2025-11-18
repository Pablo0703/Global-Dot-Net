using Domain.Entities;

namespace Presentation.Doc.Samples
{
    public static class HabilidadeSamples
    {
        // 📥 REQUEST SAMPLE
        public static readonly Habilidade HabilidadeRequest = new Habilidade
        {
            Nome = "C# Avançado",
            Categoria = "TECNOLOGIA",
            Nivel = "EXPERT",
            Descricao = "Mentoria completa sobre C#, ASP.NET e arquitetura.",
            IsOffering = true,
            IsSeeking = false,
            ValorPorHora = 50,
            UsuarioId = 1
        };

        // 📤 RESPONSE SAMPLE
        public static readonly object HabilidadeResponse = new
        {
            Id = 300,
            Nome = "C# Avançado",
            Categoria = "TECNOLOGIA",
            Nivel = "EXPERT",
            Descricao = "Mentoria completa sobre C#, ASP.NET e arquitetura.",
            IsOffering = true,
            IsSeeking = false,
            ValorPorHora = 50,
            UsuarioId = 1,
            DataCriacao = DateTime.UtcNow
        };
    }
}
