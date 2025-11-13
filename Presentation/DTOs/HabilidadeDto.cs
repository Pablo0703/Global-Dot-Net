public class HabilidadeDTO
{
    public Guid Id { get; set; }

    public required string Nome { get; set; }

    public required string Categoria { get; set; }

    public string? Descricao { get; set; }

    public required string Nivel { get; set; }

    public required bool IsOffering { get; set; }

    public required bool IsSeeking { get; set; }

    public double? ValorPorHora { get; set; }

    public required Guid UsuarioId { get; set; }

    public DateTime DataCriacao { get; set; }
}
