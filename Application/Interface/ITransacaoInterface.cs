using Domain.Entities;

namespace Application.Interface
{
    public interface ITransacaoInterface
    {
        Task<Transacao> Criar(Transacao transacao);
        Task<Transacao?> BuscarPorId(int id);
        Task<IEnumerable<Transacao>> Listar();
        Task<IEnumerable<Transacao>> ListarPorUsuario(int usuarioId);

        Task<Transacao?> Atualizar(int id, Transacao dados); // <-- ADICIONADO
        Task<bool> Deletar(int id);                          // <-- ADICIONADO

        Task<Transacao?> Concluir(int id);
        Task<Transacao?> Estornar(int id);
    }
}
