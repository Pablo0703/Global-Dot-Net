using Application.Interface;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Application.Service
{
    public class TransacaoService : ITransacaoInterface
    {
        private readonly AppDbContext _context;

        public TransacaoService(AppDbContext context)
        {
            _context = context;
        }

        // ➕ Criar
        public async Task<Transacao> Criar(Transacao transacao)
        {
            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();
            return transacao;
        }

        // 🔎 Buscar por ID
        public async Task<Transacao?> BuscarPorId(Guid id)
        {
            return await _context.Transacoes
                .Include(t => t.Remetente)
                .Include(t => t.Destinatario)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        // 📄 Listar
        public async Task<IEnumerable<Transacao>> Listar()
        {
            return await _context.Transacoes.ToListAsync();
        }

        // 📄 Listar por usuário
        public async Task<IEnumerable<Transacao>> ListarPorUsuario(Guid usuarioId)
        {
            return await _context.Transacoes
                .Where(t => t.RemetenteId == usuarioId || t.DestinatarioId == usuarioId)
                .ToListAsync();
        }

        // 🔄 Concluir
        public async Task<Transacao?> Concluir(Guid id)
        {
            var t = await _context.Transacoes.FindAsync(id);
            if (t == null) return null;

            t.Status = Transacao.CONCLUIDA;
            await _context.SaveChangesAsync();
            return t;
        }

        // 🔄 Estornar
        public async Task<Transacao?> Estornar(Guid id)
        {
            var t = await _context.Transacoes.FindAsync(id);
            if (t == null) return null;

            t.Status = Transacao.ESTORNADA;
            await _context.SaveChangesAsync();
            return t;
        }
    }
}
