using Domain.Entities;

namespace Application.Interface
{
    public interface IHabilidadeInterface
    {
        // ➕ Criar
        Task<Habilidade> Criar(Habilidade habilidade);

        // 🔎 Buscar por ID
        Task<Habilidade?> BuscarPorId(Guid id);

        // 📄 Listar todas
        Task<IEnumerable<Habilidade>> Listar();

        // 📄 Listar por usuário
        Task<IEnumerable<Habilidade>> ListarPorUsuario(Guid usuarioId);

        // ✏️ Atualizar
        Task<Habilidade?> Atualizar(Guid id, Habilidade habilidade);

        // ❌ Remover
        Task<bool> Deletar(Guid id);
    }
}
