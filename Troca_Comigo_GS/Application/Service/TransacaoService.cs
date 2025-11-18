using Application.Interface;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Service
{
    public class TransacaoService : ITransacaoInterface
    {
        private readonly AppDbContext _context;

        public TransacaoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Transacao> Criar(Transacao transacao)
        {
            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();
            return transacao;
        }

        public async Task<Transacao?> BuscarPorId(int id)
        {
            // Sem Includes para funcionar no InMemory
            return await _context.Transacoes
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Transacao>> Listar()
        {
            return await _context.Transacoes.ToListAsync();
        }

        public async Task<IEnumerable<Transacao>> ListarPorUsuario(int usuarioId)
        {
            return await _context.Transacoes
                .Where(t => t.RemetenteId == usuarioId || t.DestinatarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<Transacao?> Atualizar(int id, Transacao dados)
        {
            var existente = await _context.Transacoes.FindAsync(id);
            if (existente == null) return null;

            existente.TrocaId = dados.TrocaId;
            existente.RemetenteId = dados.RemetenteId;
            existente.DestinatarioId = dados.DestinatarioId;
            existente.Creditos = dados.Creditos;
            existente.Tipo = dados.Tipo;
            existente.Descricao = dados.Descricao;
            existente.Status = dados.Status;

            await _context.SaveChangesAsync();
            return existente;
        }

        public async Task<bool> Deletar(int id)
        {
            var existente = await _context.Transacoes.FindAsync(id);
            if (existente == null) return false;

            _context.Transacoes.Remove(existente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Transacao?> Concluir(int id)
        {
            var existente = await _context.Transacoes.FindAsync(id);
            if (existente == null) return null;

            // 🔥 Teste espera exatamente este texto:
            existente.Status = "CONCLUIDA";

            await _context.SaveChangesAsync();
            return existente;
        }

        public async Task<Transacao?> Estornar(int id)
        {
            var existente = await _context.Transacoes.FindAsync(id);
            if (existente == null) return null;

            // 🔥 Teste espera exatamente este texto:
            existente.Status = "ESTORNADA";

            await _context.SaveChangesAsync();
            return existente;
        }
    }
}
