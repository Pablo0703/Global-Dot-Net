using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Presentation.DTOs;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Tests.Integration
{
    public class AvaliacaoIntegrationTest : IClassFixture<DelegatedWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AvaliacaoIntegrationTest(DelegatedWebApplicationFactory factory)
        {
            _client = factory.CreateClient();

            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            context.Avaliacoes.Add(new Avaliacao
            {
                Id = 1,
                TrocaId = 10,
                AvaliadorId = 20,
                AvaliadoId = 30,
                Nota = 5
            });

            context.SaveChanges();
        }

        [Fact(DisplayName = "GET /api/v1/avaliacao deve retornar 200 OK")]
        public async Task GetAll_DeveRetornar200()
        {
            var response = await _client.GetAsync("/api/v1/avaliacao");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "POST /api/v1/avaliacao deve retornar 201 Created")]
        public async Task Post_DeveRetornar201()
        {
            var dto = new AvaliacaoDTO
            {
                TrocaId = 10,
                AvaliadoId = 30,
                AvaliadorId = 20,
                Nota = 4
            };

            var response = await _client.PostAsJsonAsync("/api/v1/avaliacao", dto);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
