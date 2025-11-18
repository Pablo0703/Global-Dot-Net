using Domain.Entities;

namespace Application.Interface
{
    public interface IAvaliacaoInterface
    {
        Task<Avaliacao> Criar(Avaliacao avaliacao);
        Task<Avaliacao?> BuscarPorId(int id);
        Task<IEnumerable<Avaliacao>> Listar();
        Task<IEnumerable<Avaliacao>> ListarPorAvaliado(int usuarioId);
        Task<IEnumerable<Avaliacao>> ListarPorTroca(int trocaId);
        Task Atualizar(Avaliacao avaliacao);
        Task Remover(int id);
    }
}
