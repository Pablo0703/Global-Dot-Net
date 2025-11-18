using Application.Interface;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Service
{
    public class HabilidadeService : IHabilidadeInterface
    {
        private readonly AppDbContext _context;

        public HabilidadeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Habilidade> Criar(Habilidade habilidade)
        {
            _context.Habilidades.Add(habilidade);
            await _context.SaveChangesAsync();
            return habilidade;
        }

        public async Task<Habilidade?> BuscarPorId(int id)
        {
            // 🔥 Removido Include — agora compatível com InMemory
            return await _context.Habilidades
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<IEnumerable<Habilidade>> Listar()
        {
            return await _context.Habilidades.ToListAsync();
        }

        public async Task<IEnumerable<Habilidade>> ListarPorUsuario(int usuarioId)
        {
            return await _context.Habilidades
                .Where(h => h.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<Habilidade?> Atualizar(int id, Habilidade habilidade)
        {
            var existente = await _context.Habilidades.FindAsync(id);

            if (existente == null)
                return null;

            existente.Nome = habilidade.Nome;
            existente.Categoria = habilidade.Categoria;
            existente.Descricao = habilidade.Descricao;
            existente.Nivel = habilidade.Nivel;
            existente.UsuarioId = habilidade.UsuarioId;
            existente.IsOffering = habilidade.IsOffering;
            existente.IsSeeking = habilidade.IsSeeking;
            existente.ValorPorHora = habilidade.ValorPorHora;

            await _context.SaveChangesAsync();
            return existente;
        }

        public async Task<bool> Deletar(int id)
        {
            var h = await _context.Habilidades.FindAsync(id);
            if (h == null) return false;

            _context.Habilidades.Remove(h);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
