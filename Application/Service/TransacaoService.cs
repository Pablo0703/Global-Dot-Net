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
            transacao.DataCriacao = DateTime.UtcNow;
            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();
            return transacao;
        }

        public async Task<Transacao?> BuscarPorId(int id)
        {
            return await _context.Transacoes
                .Include(t => t.Remetente)
                .Include(t => t.Destinatario)
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

        // 🟢 ATUALIZAR
        public async Task<Transacao?> Atualizar(int id, Transacao dados)
        {
            var t = await _context.Transacoes.FindAsync(id);
            if (t == null)
                return null;

            t.TrocaId = dados.TrocaId;
            t.RemetenteId = dados.RemetenteId;
            t.DestinatarioId = dados.DestinatarioId;
            t.Creditos = dados.Creditos;
            t.Tipo = dados.Tipo;
            t.Descricao = dados.Descricao;
            t.Status = dados.Status;

            await _context.SaveChangesAsync();
            return t;
        }

        // 🛑 DELETAR
        public async Task<bool> Deletar(int id)
        {
            var t = await _context.Transacoes.FindAsync(id);
            if (t == null) return false;

            _context.Transacoes.Remove(t);
            await _context.SaveChangesAsync();
            return true;
        }

        // 🔄 CONCLUIR
        public async Task<Transacao?> Concluir(int id)
        {
            var t = await _context.Transacoes.FindAsync(id);
            if (t == null) return null;

            t.Status = Transacao.CONCLUIDA;
            await _context.SaveChangesAsync();
            return t;
        }

        // 🔄 ESTORNAR
        public async Task<Transacao?> Estornar(int id)
        {
            var t = await _context.Transacoes.FindAsync(id);
            if (t == null) return null;

            t.Status = Transacao.ESTORNADA;
            await _context.SaveChangesAsync();
            return t;
        }
    }
}
