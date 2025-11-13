using Domain.Entities;

namespace Application.Interface
{
    public interface ITransacaoInterface
    {
        // ➕ Criar
        Task<Transacao> Criar(Transacao transacao);

        // 🔎 Buscar por ID
        Task<Transacao?> BuscarPorId(Guid id);

        // 📄 Listar todas
        Task<IEnumerable<Transacao>> Listar();

        // 📄 Listar por usuário
        Task<IEnumerable<Transacao>> ListarPorUsuario(Guid usuarioId);

        // 🔄 Concluir transação
        Task<Transacao?> Concluir(Guid id);

        // 🔄 Estornar
        Task<Transacao?> Estornar(Guid id);
    }
}
