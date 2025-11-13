using Application.Interface;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Application.Service
{
    public class HabilidadeService : IHabilidadeInterface
    {
        private readonly AppDbContext _context;

        public HabilidadeService(AppDbContext context)
        {
            _context = context;
        }

        // ➕ Criar
        public async Task<Habilidade> Criar(Habilidade habilidade)
        {
            _context.Habilidades.Add(habilidade);
            await _context.SaveChangesAsync();
            return habilidade;
        }

        // 🔎 Buscar por ID
        public async Task<Habilidade?> BuscarPorId(Guid id)
        {
            return await _context.Habilidades
                .Include(h => h.Usuario)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        // 📄 Listar todas
        public async Task<IEnumerable<Habilidade>> Listar()
        {
            return await _context.Habilidades.ToListAsync();
        }

        // 📄 Listar por usuário
        public async Task<IEnumerable<Habilidade>> ListarPorUsuario(Guid usuarioId)
        {
            return await _context.Habilidades
                .Where(h => h.UsuarioId == usuarioId)
                .ToListAsync();
        }

        // ✏️ Atualizar
        public async Task<Habilidade?> Atualizar(Guid id, Habilidade habilidade)
        {
            var existente = await _context.Habilidades.FindAsync(id);
            if (existente == null) return null;

            existente.Nome = habilidade.Nome;
            existente.Categoria = habilidade.Categoria;
            existente.Descricao = habilidade.Descricao;
            existente.Nivel = habilidade.Nivel;
            existente.IsOffering = habilidade.IsOffering;
            existente.IsSeeking = habilidade.IsSeeking;
            existente.ValorPorHora = habilidade.ValorPorHora;

            await _context.SaveChangesAsync();
            return existente;
        }

        // ❌ Deletar
        public async Task<bool> Deletar(Guid id)
        {
            var habilidade = await _context.Habilidades.FindAsync(id);
            if (habilidade == null) return false;

            _context.Habilidades.Remove(habilidade);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
