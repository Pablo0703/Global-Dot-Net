using Application.Interface;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Service
{
    public class AvaliacaoService : IAvaliacaoInterface
    {
        private readonly AppDbContext _context;

        public AvaliacaoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Avaliacao> Criar(Avaliacao avaliacao)
        {
            _context.Avaliacoes.Add(avaliacao);
            await _context.SaveChangesAsync();
            return avaliacao;
        }

        public async Task<Avaliacao?> BuscarPorId(int id)
        {
            return await _context.Avaliacoes
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Avaliacao>> ListarPorAvaliado(int usuarioId)
        {
            return await _context.Avaliacoes
                .Where(a => a.AvaliadoId == usuarioId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Avaliacao>> ListarPorTroca(int trocaId)
        {
            return await _context.Avaliacoes
                .Where(a => a.TrocaId == trocaId)
                .ToListAsync();
        }

        public async Task Remover(int id)
        {
            var avaliacao = await _context.Avaliacoes.FindAsync(id);

            if (avaliacao == null)
                return;

            _context.Avaliacoes.Remove(avaliacao);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Avaliacao>> Listar()
        {
            return await _context.Avaliacoes.ToListAsync();
        }


        public async Task Atualizar(Avaliacao avaliacao)
        {
            _context.Avaliacoes.Update(avaliacao);
            await _context.SaveChangesAsync();
        }
    }
}
