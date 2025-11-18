using Application.Service;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.App.Services
{
    public class UsuarioServiceTest
    {
        private readonly AppDbContext _context;
        private readonly UsuarioService _service;

        public UsuarioServiceTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("UsuarioServiceTestDB")
                .Options;

            _context = new AppDbContext(options);
            _service = new UsuarioService(_context);
        }

        [Fact(DisplayName = "Criar usuário deve retornar usuário criado")]
        public async Task Criar_DeveCriarUsuario()
        {
            var usuario = new Usuario
            {
                FullName = "Pablo Lopes",
                Email = "pablo@test.com",
                Password = "123456",
                Role = "USER",
                Bio = "Bio teste",
                Location = "São Paulo",
                Timezone = "America/Sao_Paulo",
                LinkedinUrl = "http://linkedin.com",
                TimeCredits = 10
            };

            var criado = await _service.Criar(usuario);

            criado.Should().NotBeNull();
            criado.Id.Should().BeGreaterThan(0);
            criado.FullName.Should().Be("Pablo Lopes");
            criado.Email.Should().Be("pablo@test.com");
        }

        [Fact(DisplayName = "BuscarPorId deve retornar usuário existente")]
        public async Task BuscarPorId_DeveRetornarUsuario()
        {
            var user = new Usuario
            {
                FullName = "Teste",
                Email = "teste@teste.com",
                Password = "123",
                Role = "USER"
            };

            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();

            var result = await _service.BuscarPorId(user.Id);

            result.Should().NotBeNull();
            result!.Id.Should().Be(user.Id);
        }

        [Fact(DisplayName = "BuscarPorId deve retornar null quando não existir")]
        public async Task BuscarPorId_DeveRetornarNull()
        {
            var result = await _service.BuscarPorId(999);
            result.Should().BeNull();
        }

        [Fact(DisplayName = "Listar deve retornar lista de usuários")]
        public async Task Listar_DeveRetornarLista()
        {
            _context.Usuarios.Add(new Usuario
            {
                FullName = "User 1",
                Email = "u1@test.com",
                Password = "123"
            });

            await _context.SaveChangesAsync();

            var lista = await _service.Listar();

            lista.Should().NotBeNull();
            lista.Count().Should().BeGreaterThan(0);
        }

        [Fact(DisplayName = "Atualizar deve alterar dados do usuário")]
        public async Task Atualizar_DeveAtualizarUsuario()
        {
            var user = new Usuario
            {
                FullName = "Antigo Nome",
                Email = "old@test.com",
                Password = "123"
            };

            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();

            var atualizacao = new Usuario
            {
                FullName = "Nome Novo",
                Bio = "Nova Bio",
                Location = "RJ",
                Timezone = "America/Rio"
            };

            var atualizado = await _service.Atualizar(user.Id, atualizacao);

            atualizado.Should().NotBeNull();
            atualizado!.FullName.Should().Be("Nome Novo");
            atualizado.Bio.Should().Be("Nova Bio");
            atualizado.Location.Should().Be("RJ");
        }

        [Fact(DisplayName = "Deletar deve remover usuário e retornar true")]
        public async Task Deletar_DeveRemover()
        {
            var user = new Usuario
            {
                FullName = "Para Deletar",
                Email = "delete@test.com",
                Password = "123"
            };

            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();

            var resultado = await _service.Deletar(user.Id);

            resultado.Should().BeTrue();
        }

        [Fact(DisplayName = "Deletar usuário inexistente deve retornar false")]
        public async Task Deletar_DeveRetornarFalse()
        {
            var resultado = await _service.Deletar(999);
            resultado.Should().BeFalse();
        }
    }
}
