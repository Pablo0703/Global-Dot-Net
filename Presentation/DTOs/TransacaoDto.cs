public class TransacaoDTO
{
    public Guid Id { get; set; }

    public Guid? TrocaId { get; set; }

    public required Guid RemetenteId { get; set; }

    public required Guid DestinatarioId { get; set; }

    public required double Creditos { get; set; }

    public required string Tipo { get; set; }

    public string? Descricao { get; set; }

    public required string Status { get; set; }

    public DateTime DataCriacao { get; set; }
}
