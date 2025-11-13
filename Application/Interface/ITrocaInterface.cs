using Domain.Entities;
using System.Diagnostics;

namespace Application.Interface
{
    public interface ITrocaInterface
    {
        // ➕ Criar
        Task<Troca> Criar(Troca troca);

        // 🔎 Buscar por ID
        Task<Troca?> BuscarPorId(Guid id);

        // 📄 Listar todas
        Task<IEnumerable<Troca>> Listar();

        // 📄 Listar por mentor
        Task<IEnumerable<Troca>> ListarPorMentor(Guid mentorId);

        // 📄 Listar por aluno
        Task<IEnumerable<Troca>> ListarPorAluno(Guid alunoId);

        // 🔄 Atualizar status
        Task<Troca?> AtualizarStatus(Guid id, string status);

        // ❌ Remover
        Task<bool> Deletar(Guid id);
    }
}
