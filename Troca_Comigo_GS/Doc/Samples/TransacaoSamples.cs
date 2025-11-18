using Domain.Entities;

namespace Presentation.Doc.Samples
{
    public static class TransacaoSamples
    {
        // 📥 REQUEST SAMPLE
        public static readonly Transacao TransacaoRequest = new Transacao
        {
            RemetenteId = 1,
            DestinatarioId = 2,
            Creditos = 5,
            Tipo = "PAGAMENTO_SESSAO",
            Descricao = "Pagamento por mentoria",
            Status = "PENDENTE"
        };

        // 📤 RESPONSE SAMPLE
        public static readonly object TransacaoResponse = new
        {
            Id = 900,
            RemetenteId = 1,
            DestinatarioId = 2,
            Creditos = 5,
            Tipo = "PAGAMENTO_SESSAO",
            Status = "CONCLUIDA",
            DataCriacao = DateTime.UtcNow
        };
    }
}
