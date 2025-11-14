using Domain.Entities;

namespace Application.Interface
{
    public interface IAvaliacaoInterface
    {
        Task<Avaliacao> Criar(Avaliacao avaliacao);
        Task<Avaliacao?> BuscarPorId(int id);
        Task<IEnumerable<Avaliacao>> ListarPorAvaliado(int usuarioId);
        Task<IEnumerable<Avaliacao>> ListarPorTroca(int trocaId);
        Task Remover(int id);
        Task Atualizar(Avaliacao avaliacao);
    }
}
