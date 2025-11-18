using Domain.Entities;

namespace Application.Interface
{
    public interface IUsuarioInterface
    {
        // ➕ Criar
        Task<Usuario> Criar(Usuario usuario);

        // 🔎 Buscar por ID
        Task<Usuario?> BuscarPorId(int id);

        // 📧 Buscar por e-mail
        Task<Usuario?> BuscarPorEmail(string email);

        // 📄 Listar todos
        Task<IEnumerable<Usuario>> Listar();

        // ✏️ Atualizar
        Task<Usuario?> Atualizar(int id, Usuario usuario);

        // ❌ Remover
        Task<bool> Deletar(int id);
    }
}
