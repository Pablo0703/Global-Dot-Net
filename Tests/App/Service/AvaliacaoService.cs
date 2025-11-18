using Application.Service;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.App.Services
{
    public class AvaliacaoServiceTest
    {
        private readonly AppDbContext _context;
        private readonly AvaliacaoService _service;

        public AvaliacaoServiceTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("AvaliacaoServiceTestDB")
                .Options;

            _context = new AppDbContext(options);
            _service = new AvaliacaoService(_context);
        }

        [Fact(DisplayName = "Criar avaliação deve retornar avaliação criada")]
        public async Task Criar_DeveCriarAvaliacao()
        {
            var a = new Avaliacao
            {
                TrocaId = 1,
                AvaliadorId = 2,
                AvaliadoId = 3,
                Nota = 5,
                Comentario = "Excelente sessão"
            };

            var criado = await _service.Criar(a);

            criado.Should().NotBeNull();
            criado.Id.Should().BeGreaterThan(0);
            criado.Nota.Should().Be(5);
        }

        [Fact(DisplayName = "BuscarPorId deve retornar avaliação existente")]
        public async Task BuscarPorId_DeveRetornarAvaliacao()
        {
            var a = new Avaliacao
            {
                TrocaId = 10,
                AvaliadoId = 20,
                AvaliadorId = 30,
                Nota = 4
            };

            _context.Avaliacoes.Add(a);
            await _context.SaveChangesAsync();

            var result = await _service.BuscarPorId(a.Id);

            result.Should().NotBeNull();
            result!.Id.Should().Be(a.Id);
            result.Nota.Should().Be(4);
        }

        [Fact(DisplayName = "BuscarPorId deve retornar null quando não existir")]
        public async Task BuscarPorId_DeveRetornarNull()
        {
            var result = await _service.BuscarPorId(999);
            result.Should().BeNull();
        }

        [Fact(DisplayName = "ListarPorAvaliado deve retornar avaliações corretas")]
        public async Task ListarPorAvaliado_DeveRetornarAvaliacoes()
        {
            // preparations
            _context.Avaliacoes.AddRange(
                new Avaliacao { TrocaId = 1, AvaliadorId = 2, AvaliadoId = 50, Nota = 5 },
                new Avaliacao { TrocaId = 2, AvaliadorId = 3, AvaliadoId = 50, Nota = 4 },
                new Avaliacao { TrocaId = 3, AvaliadorId = 4, AvaliadoId = 10, Nota = 3 } // outro usuário
            );

            await _context.SaveChangesAsync();

            var lista = await _service.ListarPorAvaliado(50);

            lista.Should().HaveCount(2);
            lista.Should().OnlyContain(a => a.AvaliadoId == 50);
        }

        [Fact(DisplayName = "ListarPorTroca deve retornar avaliações vinculadas à troca")]
        public async Task ListarPorTroca_DeveRetornarAvaliacoes()
        {
            _context.Avaliacoes.AddRange(
                new Avaliacao { TrocaId = 100, AvaliadorId = 1, AvaliadoId = 2, Nota = 3 },
                new Avaliacao { TrocaId = 100, AvaliadorId = 3, AvaliadoId = 4, Nota = 5 },
                new Avaliacao { TrocaId = 200, AvaliadorId = 5, AvaliadoId = 6, Nota = 4 }
            );

            await _context.SaveChangesAsync();

            var lista = await _service.ListarPorTroca(100);

            lista.Should().HaveCount(2);
            lista.Should().OnlyContain(a => a.TrocaId == 100);
        }

        [Fact(DisplayName = "Atualizar deve alterar nota e comentário")]
        public async Task Atualizar_DeveAtualizarAvaliacao()
        {
            var a = new Avaliacao
            {
                TrocaId = 1,
                AvaliadorId = 2,
                AvaliadoId = 3,
                Nota = 2,
                Comentario = "Fraco"
            };

            _context.Avaliacoes.Add(a);
            await _context.SaveChangesAsync();

            a.Nota = 5;
            a.Comentario = "Melhorou muito";

            await _service.Atualizar(a);

            var atualizado = await _context.Avaliacoes.FindAsync(a.Id);

            atualizado!.Nota.Should().Be(5);
            atualizado.Comentario.Should().Be("Melhorou muito");
        }

        [Fact(DisplayName = "Remover deve deletar avaliação existente")]
        public async Task Remover_DeveExcluirAvaliacao()
        {
            var a = new Avaliacao
            {
                TrocaId = 1,
                AvaliadoId = 2,
                AvaliadorId = 3,
                Nota = 5
            };

            _context.Avaliacoes.Add(a);
            await _context.SaveChangesAsync();

            await _service.Remover(a.Id);

            var result = await _context.Avaliacoes.FindAsync(a.Id);

            result.Should().BeNull();
        }

        [Fact(DisplayName = "Remover avaliação inexistente deve não lançar erro")]
        public async Task Remover_InexistenteNaoLancaErro()
        {
            var act = async () => await _service.Remover(999);

            await act.Should().NotThrowAsync();
        }
    }
}
