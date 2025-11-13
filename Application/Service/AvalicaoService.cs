using Application.Interface;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Application.Service
{
    public class AvaliacaoService : IAvaliacaoInterface
    {
        private readonly AppDbContext _context;

        public AvaliacaoService(AppDbContext context)
        {
            _context = context;
        }

        // ➕ Criar
        public async Task<Avaliacao> Criar(Avaliacao avaliacao)
        {
            _context.Avaliacoes.Add(avaliacao);
            await _context.SaveChangesAsync();
            return avaliacao;
        }

        // 🔎 Buscar por ID
        public async Task<Avaliacao?> BuscarPorId(Guid id)
        {
            return await _context.Avaliacoes
                .Include(a => a.Avaliador)
                .Include(a => a.Avaliado)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        // 📄 Listar por avaliado
        public async Task<IEnumerable<Avaliacao>> ListarPorAvaliado(Guid usuarioId)
        {
            return await _context.Avaliacoes
                .Where(a => a.AvaliadoId == usuarioId)
                .ToListAsync();
        }

        // 📄 Listar por troca
        public async Task<IEnumerable<Avaliacao>> ListarPorTroca(Guid trocaId)
        {
            return await _context.Avaliacoes
                .Where(a => a.TrocaId == trocaId)
                .ToListAsync();
        }
    }
}
