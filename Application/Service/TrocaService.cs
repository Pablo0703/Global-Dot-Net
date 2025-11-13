using Application.Interface;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace Application.Service
{
    public class TrocaService : ITrocaInterface
    {
        private readonly AppDbContext _context;

        public TrocaService(AppDbContext context)
        {
            _context = context;
        }

        // ➕ Criar
        public async Task<Troca> Criar(Troca troca)
        {
            _context.Trocas.Add(troca);
            await _context.SaveChangesAsync();
            return troca;
        }

        // 🔎 Buscar por ID
        public async Task<Troca?> BuscarPorId(Guid id)
        {
            return await _context.Trocas
                .Include(t => t.Mentor)
                .Include(t => t.Aluno)
                .Include(t => t.Habilidade)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        // 📄 Listar
        public async Task<IEnumerable<Troca>> Listar()
        {
            return await _context.Trocas.ToListAsync();
        }

        // 📄 Listar por mentor
        public async Task<IEnumerable<Troca>> ListarPorMentor(Guid mentorId)
        {
            return await _context.Trocas
                .Where(t => t.MentorId == mentorId)
                .ToListAsync();
        }

        // 📄 Listar por aluno
        public async Task<IEnumerable<Troca>> ListarPorAluno(Guid alunoId)
        {
            return await _context.Trocas
                .Where(t => t.AlunoId == alunoId)
                .ToListAsync();
        }

        // 🔄 Atualizar status
        public async Task<Troca?> AtualizarStatus(Guid id, string status)
        {
            var troca = await _context.Trocas.FindAsync(id);
            if (troca == null) return null;

            troca.Status = status;
            await _context.SaveChangesAsync();

            return troca;
        }

        // ❌ Deletar
        public async Task<bool> Deletar(Guid id)
        {
            var troca = await _context.Trocas.FindAsync(id);
            if (troca == null) return false;

            _context.Trocas.Remove(troca);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
