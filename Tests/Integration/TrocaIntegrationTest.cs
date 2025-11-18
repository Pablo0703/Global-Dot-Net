using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Tests.Integration
{
    public class TrocaIntegrationTest : IClassFixture<DelegatedWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public TrocaIntegrationTest(DelegatedWebApplicationFactory factory)
        {
            _client = factory.CreateClient();

            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // seed
            context.Trocas.Add(new Troca
            {
                Id = 1,
                MentorId = 10,
                AlunoId = 20,
                HabilidadeId = 30
            });

            context.SaveChanges();
        }

        [Fact(DisplayName = "GET /api/v1/troca deve retornar 200 OK")]
        public async Task GetAll_DeveRetornar200()
        {
            var response = await _client.GetAsync("/api/v1/troca");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "POST /api/v1/troca deve retornar 201 Created")]
        public async Task Post_DeveRetornar201()
        {
            var nova = new
            {
                MentorId = 10,
                AlunoId = 21,
                HabilidadeId = 30
            };

            var response = await _client.PostAsJsonAsync("/api/v1/troca", nova);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
