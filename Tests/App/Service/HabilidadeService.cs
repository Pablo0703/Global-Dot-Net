using Application.Service;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.App.Services
{
    public class HabilidadeServiceTest
    {
        private readonly AppDbContext _context;
        private readonly HabilidadeService _service;

        public HabilidadeServiceTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("HabilidadeServiceTestDB")
                .Options;

            _context = new AppDbContext(options);
            _service = new HabilidadeService(_context);
        }

        [Fact(DisplayName = "Criar habilidade deve retornar habilidade criada")]
        public async Task Criar_DeveCriarHabilidade()
        {
            var h = new Habilidade
            {
                Nome = "Java",
                Categoria = Habilidade.TECNOLOGIA,
                Descricao = "Programação Backend",
                Nivel = Habilidade.INTERMEDIARIO,
                IsOffering = true,
                IsSeeking = false,
                ValorPorHora = 150,
                UsuarioId = 1
            };

            var criado = await _service.Criar(h);

            criado.Should().NotBeNull();
            criado.Id.Should().BeGreaterThan(0);
            criado.Nome.Should().Be("Java");
        }

        [Fact(DisplayName = "BuscarPorId deve retornar habilidade existente")]
        public async Task BuscarPorId_DeveRetornarHabilidade()
        {
            var h = new Habilidade
            {
                Nome = "Python",
                Categoria = Habilidade.TECNOLOGIA,
                Nivel = Habilidade.AVANCADO,
                UsuarioId = 2
            };

            _context.Habilidades.Add(h);
            await _context.SaveChangesAsync();

            var result = await _service.BuscarPorId(h.Id);

            result.Should().NotBeNull();
            result!.Id.Should().Be(h.Id);
        }

        [Fact(DisplayName = "BuscarPorId deve retornar null quando não existir")]
        public async Task BuscarPorId_DeveRetornarNull()
        {
            var result = await _service.BuscarPorId(999);
            result.Should().BeNull();
        }

        [Fact(DisplayName = "Listar deve retornar lista de habilidades")]
        public async Task Listar_DeveRetornarLista()
        {
            _context.Habilidades.Add(new Habilidade
            {
                Nome = "Excel",
                Categoria = Habilidade.NEGOCIOS,
                UsuarioId = 10
            });

            await _context.SaveChangesAsync();

            var lista = await _service.Listar();

            lista.Should().NotBeNull();
            lista.Count().Should().BeGreaterThan(0);
        }

        [Fact(DisplayName = "ListarPorUsuario deve retornar habilidades do usuário")]
        public async Task ListarPorUsuario_DeveRetornarHabilidades()
        {
            _context.Habilidades.Add(new Habilidade
            {
                Nome = "Design UX",
                Categoria = Habilidade.DESIGN,
                UsuarioId = 99
            });

            _context.Habilidades.Add(new Habilidade
            {
                Nome = "Illustrator",
                Categoria = Habilidade.DESIGN,
                UsuarioId = 99
            });

            await _context.SaveChangesAsync();

            var lista = await _service.ListarPorUsuario(99);

            lista.Should().HaveCount(2);
            lista.Should().OnlyContain(h => h.UsuarioId == 99);
        }

        [Fact(DisplayName = "Atualizar deve modificar dados da habilidade")]
        public async Task Atualizar_DeveAtualizarHabilidade()
        {
            var h = new Habilidade
            {
                Nome = "C#",
                Categoria = Habilidade.TECNOLOGIA,
                Nivel = Habilidade.INTERMEDIARIO,
                UsuarioId = 5
            };

            _context.Habilidades.Add(h);
            await _context.SaveChangesAsync();

            var dados = new Habilidade
            {
                Nome = "C# Avançado",
                Categoria = Habilidade.TECNOLOGIA,
                Nivel = Habilidade.AVANCADO,
                Descricao = "Programação avançada",
                IsOffering = false,
                IsSeeking = true,
                ValorPorHora = 200,
                UsuarioId = 5
            };

            var atualizado = await _service.Atualizar(h.Id, dados);

            atualizado.Should().NotBeNull();
            atualizado!.Nome.Should().Be("C# Avançado");
            atualizado.Nivel.Should().Be(Habilidade.AVANCADO);
            atualizado.IsSeeking.Should().BeTrue();
        }

        [Fact(DisplayName = "Atualizar habilidade inexistente deve retornar null")]
        public async Task Atualizar_DeveRetornarNull()
        {
            var dados = new Habilidade
            {
                Nome = "Nada"
            };

            var result = await _service.Atualizar(999, dados);

            result.Should().BeNull();
        }

        [Fact(DisplayName = "Deletar deve remover habilidade")]
        public async Task Deletar_DeveRemover()
        {
            var h = new Habilidade
            {
                Nome = "Git",
                Categoria = Habilidade.TECNOLOGIA,
                UsuarioId = 1
            };

            _context.Habilidades.Add(h);
            await _context.SaveChangesAsync();

            var ok = await _service.Deletar(h.Id);

            ok.Should().BeTrue();
        }

        [Fact(DisplayName = "Deletar habilidade inexistente deve retornar false")]
        public async Task Deletar_DeveRetornarFalse()
        {
            var result = await _service.Deletar(555);
            result.Should().BeFalse();
        }
    }
}
