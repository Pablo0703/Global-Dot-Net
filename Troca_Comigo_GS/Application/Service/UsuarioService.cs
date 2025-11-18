using Application.Interface;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Application.Service
{
    public class UsuarioService : IUsuarioInterface
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        // ➕ Criar
        public async Task<Usuario> Criar(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        // 🔎 Buscar por ID
        public async Task<Usuario?> BuscarPorId(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Habilidades)
                .Include(u => u.TrocasComoMentor)
                .Include(u => u.TrocasComoAluno)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        // 📧 Buscar por e-mail
        public async Task<Usuario?> BuscarPorEmail(string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        // 📄 Listar
        public async Task<IEnumerable<Usuario>> Listar()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // ✏️ Atualizar
        public async Task<Usuario?> Atualizar(int id, Usuario usuario)
        {
            var existente = await _context.Usuarios.FindAsync(id);
            if (existente == null) return null;

            existente.NomeCompleto = usuario.NomeCompleto;
            existente.Bio = usuario.Bio;
            existente.Location = usuario.Location;
            existente.Timezone = usuario.Timezone;
            existente.LinkedinUrl = usuario.LinkedinUrl;
            existente.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existente;
        }

        // ❌ Deletar
        public async Task<bool> Deletar(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
