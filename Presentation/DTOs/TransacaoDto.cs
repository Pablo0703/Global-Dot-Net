using Presentation.Hateoas;

namespace Presentation.DTOs
{
    public class TransacaoDTO
    {
        public int Id { get; set; }

        // ✨ TrocaId é opcional na Entity → deve ser opcional no DTO
        public int? TrocaId { get; set; }

        public int RemetenteId { get; set; }
        public int DestinatarioId { get; set; }

        public double Creditos { get; set; }

        public string Tipo { get; set; } = "";

        // ✨ Descricao é nullable na Entity → nullable no DTO
        public string? Descricao { get; set; }

        public string Status { get; set; } = "";

        public DateTime DataCriacao { get; set; }

        // 🔗 HATEOAS
        public List<LinkDTO>? Links { get; set; }
    }
}
