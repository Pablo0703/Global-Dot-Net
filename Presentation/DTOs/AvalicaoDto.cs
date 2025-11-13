public class AvaliacaoDTO
{
    public Guid Id { get; set; }

    public required Guid TrocaId { get; set; }

    public required Guid AvaliadorId { get; set; }

    public required Guid AvaliadoId { get; set; }

    public required int Nota { get; set; }

    public string? Comentario { get; set; }

    public DateTime DataCriacao { get; set; }
}
