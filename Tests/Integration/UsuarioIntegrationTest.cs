using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Presentation.DTOs;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Tests.Integration
{
    public class UsuarioIntegrationTest : IClassFixture<DelegatedWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public UsuarioIntegrationTest(DelegatedWebApplicationFactory factory)
        {
            _client = factory.CreateClient();

            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            context.Database.EnsureCreated();

            context.Usuarios.Add(new Usuario
            {
                Id = 1,
                FullName = "Pablo Teste",
                Email = "teste@teste.com",
                Password = "123",
                Role = "USER",
                TimeCredits = 50
            });

            context.SaveChanges();
        }

        [Fact(DisplayName = "GET /api/v1/usuarios deve retornar 200 OK")]
        public async Task GetAll_DeveRetornar200()
        {
            var response = await _client.GetAsync("/api/v1/usuarios");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "POST /api/v1/usuarios deve retornar 201 Created")]
        public async Task Post_DeveRetornar201()
        {
            var dto = new UsuarioDTO
            {
                FullName = "Novo Usuario",
                Email = "novo@email.com",
                Password = "123456",
                Role = "USER",
                TimeCredits = 100
            };

            var response = await _client.PostAsJsonAsync("/api/v1/usuarios", dto);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
