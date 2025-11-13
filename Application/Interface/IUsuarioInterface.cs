using Domain.Entities;

namespace Application.Interface
{
    public interface IUsuarioInterface
    {
        // ➕ Criar
        Task<Usuario> Criar(Usuario usuario);

        // 🔎 Buscar por ID
        Task<Usuario?> BuscarPorId(Guid id);

        // 📧 Buscar por e-mail
        Task<Usuario?> BuscarPorEmail(string email);

        // 📄 Listar todos
        Task<IEnumerable<Usuario>> Listar();

        // ✏️ Atualizar
        Task<Usuario?> Atualizar(Guid id, Usuario usuario);

        // ❌ Remover
        Task<bool> Deletar(Guid id);
    }
}
