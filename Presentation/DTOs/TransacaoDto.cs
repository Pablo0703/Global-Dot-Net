namespace Presentation.DTOs;

public class TransacaoDTO
{
    public int Id { get; set; }

    public int? TrocaId { get; set; }

    public required int RemetenteId { get; set; }

    public required int DestinatarioId { get; set; }

    public required double Creditos { get; set; }

    public required string Tipo { get; set; }

    public string? Descricao { get; set; }

    public required string Status { get; set; }

    public DateTime DataCriacao { get; set; }
}
