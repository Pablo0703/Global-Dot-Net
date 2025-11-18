using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Presentation.DTOs;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Tests.Integration
{
    public class HabilidadeIntegrationTest : IClassFixture<DelegatedWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public HabilidadeIntegrationTest(DelegatedWebApplicationFactory factory)
        {
            _client = factory.CreateClient();

            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            context.Habilidades.Add(new Habilidade
            {
                Id = 1,
                Nome = "Backend",
                Descricao = "API em .NET"
            });

            context.SaveChanges();
        }

        [Fact(DisplayName = "GET /api/v1/habilidade deve retornar 200 OK")]
        public async Task GetAll_DeveRetornar200()
        {
            var response = await _client.GetAsync("/api/v1/habilidade");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "POST /api/v1/habilidade deve retornar 201 Created")]
        public async Task Post_DeveRetornar201()
        {
            var dto = new HabilidadeDTO
            {
                Nome = "DevOps",
                Descricao = "CI/CD"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/habilidade", dto);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
