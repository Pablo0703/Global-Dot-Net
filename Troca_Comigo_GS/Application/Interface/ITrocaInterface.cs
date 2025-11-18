using Domain.Entities;

namespace Application.Interface
{
    public interface ITrocaInterface
    {
        Task<Troca> Criar(Troca troca);
        Task<Troca?> BuscarPorId(int id);
        Task<IEnumerable<Troca>> Listar();

        Task<IEnumerable<Troca>> ListarPorMentor(int mentorId);
        Task<IEnumerable<Troca>> ListarPorAluno(int alunoId);

        Task<Troca?> AtualizarStatus(int id, string status);
        Task<Troca?> Atualizar(Troca troca);

        Task<bool> Deletar(int id);
    }
}
