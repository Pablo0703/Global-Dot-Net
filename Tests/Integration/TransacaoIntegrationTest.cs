using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Presentation.DTOs;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Tests.Integration
{
    public class TransacaoIntegrationTest : IClassFixture<DelegatedWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public TransacaoIntegrationTest(DelegatedWebApplicationFactory factory)
        {
            _client = factory.CreateClient();

            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            context.Database.EnsureCreated();

            context.Transacoes.Add(new Transacao
            {
                Id = 1,
                TrocaId = 10,
                RemetenteId = 1,
                DestinatarioId = 2,
                Creditos = 50,
                Tipo = "TRANSFER",
                Status = "PENDING"
            });

            context.SaveChanges();
        }

        [Fact(DisplayName = "GET /api/v1/transacoes deve retornar 200 OK")]
        public async Task GetAll_DeveRetornar200()
        {
            var response = await _client.GetAsync("/api/v1/transacoes");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "POST /api/v1/transacoes deve retornar 201 Created")]
        public async Task Post_DeveRetornar201()
        {
            var dto = new TransacaoDTO
            {
                TrocaId = 10,
                RemetenteId = 1,
                DestinatarioId = 2,
                Creditos = 20,
                Tipo = "TRANSFER",
                Descricao = "Teste integração"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/transacoes", dto);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
