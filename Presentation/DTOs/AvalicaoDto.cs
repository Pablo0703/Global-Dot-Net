using Presentation.Hateoas;

namespace Presentation.DTOs
{
    public class AvaliacaoDTO
    {
        public int Id { get; set; }

        public required int TrocaId { get; set; }
        public required int AvaliadorId { get; set; }
        public required int AvaliadoId { get; set; }
        public required int Nota { get; set; }

        public string? Comentario { get; set; }
        public DateTime DataCriacao { get; set; }

        // 🔗 Suporte para HATEOAS
        public List<LinkDTO>? Links { get; set; }
    }
}
