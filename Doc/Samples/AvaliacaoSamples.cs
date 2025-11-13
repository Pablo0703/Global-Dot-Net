using Domain.Entities;

namespace Presentation.Doc.Samples
{
    public static class AvaliacaoSamples
    {
        // 📥 Request
        public static readonly Avaliacao AvaliacaoRequest = new Avaliacao
        {
            TrocaId = Guid.NewGuid(),
            AvaliadorId = Guid.NewGuid(),
            AvaliadoId = Guid.NewGuid(),
            Nota = 5,
            Comentario = "Mentoria excelente! Explicou tudo com calma."
        };

        // 📤 Response
        public static readonly object AvaliacaoResponse = new
        {
            Id = Guid.NewGuid(),
            TrocaId = Guid.NewGuid(),
            AvaliadorId = Guid.NewGuid(),
            AvaliadoId = Guid.NewGuid(),
            Nota = 5,
            Comentario = "Mentoria excelente! Explicou tudo com calma.",
            DataCriacao = DateTime.UtcNow
        };
    }
}
