using Application.Interface;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Service
{
    public class TrocaService : ITrocaInterface
    {
        private readonly AppDbContext _context;

        public TrocaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Troca> Criar(Troca troca)
        {
            troca.CreatedDate = DateTime.UtcNow;
            _context.Trocas.Add(troca);
            await _context.SaveChangesAsync();
            return troca;
        }

        public async Task<Troca?> BuscarPorId(int id)
        {
            // 🔥 Removido Include — EF InMemory não suporta navegação
            return await _context.Trocas
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Troca>> Listar()
        {
            return await _context.Trocas.ToListAsync();
        }

        public async Task<IEnumerable<Troca>> ListarPorMentor(int mentorId)
        {
            return await _context.Trocas
                .Where(t => t.MentorId == mentorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Troca>> ListarPorAluno(int alunoId)
        {
            return await _context.Trocas
                .Where(t => t.AlunoId == alunoId)
                .ToListAsync();
        }

        public async Task<Troca?> AtualizarStatus(int id, string status)
        {
            var troca = await _context.Trocas.FindAsync(id);
            if (troca == null) return null;

            troca.Status = status;
            await _context.SaveChangesAsync();

            return troca;
        }

        public async Task<Troca?> Atualizar(Troca troca)
        {
            var existente = await _context.Trocas.FindAsync(troca.Id);
            if (existente == null) return null;

            _context.Entry(existente).CurrentValues.SetValues(troca);
            await _context.SaveChangesAsync();

            return existente;
        }

        public async Task<bool> Deletar(int id)
        {
            var troca = await _context.Trocas.FindAsync(id);
            if (troca == null) return false;

            _context.Trocas.Remove(troca);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
