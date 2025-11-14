using Domain.Entities;

namespace Application.Interface
{
    public interface IHabilidadeInterface
    {
        // ➕ Criar
        Task<Habilidade> Criar(Habilidade habilidade);

        // 🔎 Buscar por ID
        Task<Habilidade?> BuscarPorId(int id);

        // 📄 Listar todas
        Task<IEnumerable<Habilidade>> Listar();

        // 📄 Listar por usuário
        Task<IEnumerable<Habilidade>> ListarPorUsuario(int usuarioId);

        // ✏️ Atualizar
        Task<Habilidade?> Atualizar(int id, Habilidade habilidade);

        // ❌ Remover
        Task<bool> Deletar(int id);
    }
}
