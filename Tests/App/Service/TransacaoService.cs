using Application.Service;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.App.Services
{
    public class TransacaoServiceTest
    {
        private readonly AppDbContext _context;
        private readonly TransacaoService _service;

        public TransacaoServiceTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TransacaoServiceTestDB")
                .Options;

            _context = new AppDbContext(options);
            _service = new TransacaoService(_context);
        }

        [Fact(DisplayName = "Criar transação deve retornar transação criada")]
        public async Task Criar_DeveCriarTransacao()
        {
            var t = new Transacao
            {
                RemetenteId = 1,
                DestinatarioId = 2,
                Creditos = 10,
                Tipo = "PAGAMENTO",
                Descricao = "Teste"
            };

            var criado = await _service.Criar(t);

            criado.Should().NotBeNull();
            criado.Id.Should().BeGreaterThan(0);
            criado.Creditos.Should().Be(10);
            criado.RemetenteId.Should().Be(1);
        }

        [Fact(DisplayName = "BuscarPorId deve retornar transação existente")]
        public async Task BuscarPorId_DeveRetornarTransacao()
        {
            var t = new Transacao
            {
                RemetenteId = 3,
                DestinatarioId = 4,
                Creditos = 20
            };

            _context.Transacoes.Add(t);
            await _context.SaveChangesAsync();

            var result = await _service.BuscarPorId(t.Id);

            result.Should().NotBeNull();
            result!.Id.Should().Be(t.Id);
        }

        [Fact(DisplayName = "BuscarPorId deve retornar null quando não existir")]
        public async Task BuscarPorId_DeveRetornarNull()
        {
            var result = await _service.BuscarPorId(999);
            result.Should().BeNull();
        }

        [Fact(DisplayName = "Listar deve retornar lista de transações")]
        public async Task Listar_DeveRetornarLista()
        {
            _context.Transacoes.Add(new Transacao
            {
                RemetenteId = 1,
                DestinatarioId = 2,
                Creditos = 5
            });

            await _context.SaveChangesAsync();

            var lista = await _service.Listar();

            lista.Should().NotBeNull();
            lista.Count().Should().BeGreaterThan(0);
        }

        [Fact(DisplayName = "ListarPorUsuario deve retornar transações relacionadas ao usuário")]
        public async Task ListarPorUsuario_DeveRetornarLista()
        {
            _context.Transacoes.Add(new Transacao
            {
                RemetenteId = 99,
                DestinatarioId = 15,
                Creditos = 3
            });

            _context.Transacoes.Add(new Transacao
            {
                RemetenteId = 15,
                DestinatarioId = 99,
                Creditos = 6
            });

            await _context.SaveChangesAsync();

            var lista = await _service.ListarPorUsuario(99);

            lista.Should().NotBeNull();
            lista.Should().HaveCount(2);
            lista.Should().OnlyContain(t => t.RemetenteId == 99 || t.DestinatarioId == 99);
        }

        [Fact(DisplayName = "Atualizar deve modificar dados da transação")]
        public async Task Atualizar_DeveAtualizarTransacao()
        {
            var t = new Transacao
            {
                RemetenteId = 1,
                DestinatarioId = 2,
                Creditos = 5,
                Tipo = "ANTIGO"
            };

            _context.Transacoes.Add(t);
            await _context.SaveChangesAsync();

            var dados = new Transacao
            {
                TrocaId = 50,
                RemetenteId = 9,
                DestinatarioId = 10,
                Creditos = 99,
                Tipo = "NOVO",
                Descricao = "Atualizado",
                Status = "OK"
            };

            var atualizado = await _service.Atualizar(t.Id, dados);

            atualizado.Should().NotBeNull();
            atualizado!.Creditos.Should().Be(99);
            atualizado.Tipo.Should().Be("NOVO");
            atualizado.TrocaId.Should().Be(50);
        }

        [Fact(DisplayName = "Atualizar transação inexistente deve retornar null")]
        public async Task Atualizar_DeveRetornarNull()
        {
            var dados = new Transacao
            {
                Creditos = 10
            };

            var result = await _service.Atualizar(999, dados);

            result.Should().BeNull();
        }

        [Fact(DisplayName = "Concluir deve alterar status para CONCLUIDA")]
        public async Task Concluir_DeveAlterarStatus()
        {
            var t = new Transacao
            {
                RemetenteId = 1,
                DestinatarioId = 2,
                Creditos = 10,
                Status = "PENDENTE"
            };

            _context.Transacoes.Add(t);
            await _context.SaveChangesAsync();

            var atualizado = await _service.Concluir(t.Id);

            atualizado.Should().NotBeNull();
            atualizado!.Status.Should().Be(Transacao.CONCLUIDA);
        }

        [Fact(DisplayName = "Estornar deve alterar status para ESTORNADA")]
        public async Task Estornar_DeveAlterarStatus()
        {
            var t = new Transacao
            {
                RemetenteId = 1,
                DestinatarioId = 2,
                Creditos = 10,
                Status = "PENDENTE"
            };

            _context.Transacoes.Add(t);
            await _context.SaveChangesAsync();

            var atualizado = await _service.Estornar(t.Id);

            atualizado.Should().NotBeNull();
            atualizado!.Status.Should().Be(Transacao.ESTORNADA);
        }

        [Fact(DisplayName = "Deletar deve remover transação")]
        public async Task Deletar_DeveRemover()
        {
            var t = new Transacao
            {
                RemetenteId = 1,
                DestinatarioId = 2,
                Creditos = 5
            };

            _context.Transacoes.Add(t);
            await _context.SaveChangesAsync();

            var ok = await _service.Deletar(t.Id);

            ok.Should().BeTrue();
        }

        [Fact(DisplayName = "Deletar transação inexistente deve retornar false")]
        public async Task Deletar_DeveRetornarFalse()
        {
            var result = await _service.Deletar(999);
            result.Should().BeFalse();
        }
    }
}
