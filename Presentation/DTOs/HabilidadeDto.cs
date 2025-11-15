using Presentation.Hateoas;

namespace Presentation.DTOs
{
    public class HabilidadeDTO
    {
        public int Id { get; set; }

        public string Nome { get; set; } = "";
        public string Categoria { get; set; } = "";
        public string? Descricao { get; set; }

        // NIVEL É STRING NA ENTITY
        public string Nivel { get; set; } = "";

        public bool IsOffering { get; set; }
        public bool IsSeeking { get; set; }

        // IGUAL À ENTITY (nullable)
        public double? ValorPorHora { get; set; }

        public int UsuarioId { get; set; }
        public DateTime DataCriacao { get; set; }

        // 🔗 HATEOAS
        public List<LinkDTO>? Links { get; set; }
    }
}
