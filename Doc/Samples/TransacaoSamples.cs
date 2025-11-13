using Domain.Entities;

namespace Presentation.Doc.Samples
{
    public static class TransacaoSamples
    {
        // 📥 Request
        public static readonly Transacao TransacaoRequest = new Transacao
        {
            RemetenteId = Guid.NewGuid(),
            DestinatarioId = Guid.NewGuid(),
            Creditos = 5,
            Tipo = Transacao.PAGAMENTO_SESSAO,
            Status = Transacao.PENDENTE,
            Descricao = "Pagamento por mentoria"
        };

        // 📤 Response
        public static readonly object TransacaoResponse = new
        {
            Id = Guid.NewGuid(),
            RemetenteId = Guid.NewGuid(),
            DestinatarioId = Guid.NewGuid(),
            Creditos = 5,
            Tipo = "PAGAMENTO_SESSAO",
            Status = "CONCLUIDA",
            DataCriacao = DateTime.UtcNow
        };
    }
}
