using Domain.Entities;

namespace Presentation.Doc.Samples
{
    public static class AvaliacaoSamples
    {
        // 📥 REQUEST SAMPLE
        public static readonly Avaliacao AvaliacaoRequest = new Avaliacao
        {
            TrocaId = 100,
            AvaliadorId = 1,
            AvaliadoId = 2,
            Nota = 5,
            Comentario = "Mentoria excelente! Explicou tudo com calma."
        };

        // 📤 RESPONSE SAMPLE
        public static readonly object AvaliacaoResponse = new
        {
            Id = 500,
            TrocaId = 100,
            AvaliadorId = 1,
            AvaliadoId = 2,
            Nota = 5,
            Comentario = "Mentoria excelente! Explicou tudo com calma.",
            DataCriacao = DateTime.UtcNow
        };
    }
}
