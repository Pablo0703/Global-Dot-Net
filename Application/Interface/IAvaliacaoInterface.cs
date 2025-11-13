using Domain.Entities;

namespace Application.Interface
{
    public interface IAvaliacaoInterface
    {
        // ➕ Criar
        Task<Avaliacao> Criar(Avaliacao avaliacao);

        // 🔎 Buscar por ID
        Task<Avaliacao?> BuscarPorId(Guid id);

        // 📄 Listar por usuário
        Task<IEnumerable<Avaliacao>> ListarPorAvaliado(Guid usuarioId);

        // 📄 Listar por troca
        Task<IEnumerable<Avaliacao>> ListarPorTroca(Guid trocaId);
    }
}
