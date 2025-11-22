using Application.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Presentation.Controllers.v1;
using Presentation.DTOs;
using Domain.Entities;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Tests.App.Controllers
{
    public class AvaliacaoControllerTest
    {
        private readonly Mock<IAvaliacaoInterface> _serviceMock;
        private readonly AvaliacaoController _controller;

        public AvaliacaoControllerTest()
        {
            _serviceMock = new Mock<IAvaliacaoInterface>();
            _controller = new AvaliacaoController(_serviceMock.Object);

            var url = new Mock<IUrlHelper>();
            url.Setup(u => u.Action(It.IsAny<UrlActionContext>()))
               .Returns("http://localhost/api/v1/avaliacao/1");

            _controller.Url = url.Object;
        }

        // ============================================================
        // GET /avaliacao/{id}
        // ============================================================
        [Fact(DisplayName = "GET por ID deve retornar avaliação existente")]
        public async Task BuscarPorId_DeveRetornarAvaliacao()
        {
            var entity = new Avaliacao
            {
                Id = 1,
                TrocaId = 10,
                AvaliadorId = 20,
                AvaliadoId = 30,
                Nota = 5,
                Comentario = "Ótima troca!"
            };

            _serviceMock.Setup(s => s.BuscarPorId(1)).ReturnsAsync(entity);

            var result = await _controller.BuscarPorId(1);
            var ok = Assert.IsType<OkObjectResult>(result);

            var resource = ok.Value!;
            var dto = resource.GetType().GetProperty("Data")?.GetValue(resource) as AvaliacaoDTO;

            Assert.NotNull(dto);
            Assert.Equal(5, dto.Nota);
            Assert.Equal("Ótima troca!", dto.Comentario);
        }

        // ============================================================
        // GET /avaliacao/troca/{trocaId}
        // ============================================================
        [Fact(DisplayName = "GET por troca deve retornar lista de avaliações")]
        public async Task ListarPorTroca_DeveRetornarLista()
        {
            var trocaId = 10;

            var lista = new List<Avaliacao>
            {
                new Avaliacao { Id = 1, TrocaId = trocaId }
            };

            _serviceMock.Setup(s => s.ListarPorTroca(trocaId)).ReturnsAsync(lista);

            var result = await _controller.ListarPorTroca(trocaId);

            var ok = Assert.IsType<OkObjectResult>(result);
            var response = ok.Value!;
            var itemsProp = response.GetType().GetProperty("items");
            var dtos = itemsProp?.GetValue(response) as IEnumerable<AvaliacaoDTO>;

            Assert.NotNull(dtos);
            Assert.True(dtos.Any());
        }

        // ============================================================
        // POST /avaliacao
        // ============================================================
        [Fact(DisplayName = "POST deve retornar Created")]
        public async Task Criar_DeveRetornarCreated()
        {
            var entity = new Avaliacao
            {
                Id = 1,
                Nota = 5,
                Comentario = "Excelente!"
            };

            _serviceMock.Setup(s => s.Criar(It.IsAny<Avaliacao>()))
                        .ReturnsAsync(entity);

            var dto = new AvaliacaoDTO
            {
                AvaliadoId = 30,
                AvaliadorId = 20,
                TrocaId = 10,
                Nota = 5,
                Comentario = "Excelente!"
            };

            var result = await _controller.Criar(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var resource = created.Value!;
            var data = resource.GetType().GetProperty("Data")!.GetValue(resource) as AvaliacaoDTO;

            Assert.NotNull(data);
            Assert.Equal(5, data.Nota);
        }

        // ============================================================
        // PUT /avaliacao/{id}
        // ============================================================
        [Fact(DisplayName = "PUT deve retornar Ok ao atualizar")]
        public async Task Atualizar_DeveRetornarOk()
        {
            var entity = new Avaliacao { Id = 1, Nota = 3 };

            _serviceMock.Setup(s => s.BuscarPorId(1)).ReturnsAsync(entity);
            _serviceMock.Setup(s => s.Atualizar(It.IsAny<Avaliacao>()))
                        .Returns(Task.CompletedTask);

            var dto = new AvaliacaoDTO
            {
                TrocaId = 10,
                AvaliadorId = 20,
                AvaliadoId = 30,
                Nota = 4,
                Comentario = "Agora sim!"
            };

            var result = await _controller.Atualizar(1, dto);
            var ok = Assert.IsType<OkObjectResult>(result);

            var resource = ok.Value!;
            var data = resource.GetType().GetProperty("Data")!.GetValue(resource) as AvaliacaoDTO;

            Assert.NotNull(data);
            Assert.Equal(4, data.Nota);
        }

        // ============================================================
        // DELETE /avaliacao/{id}
        // ============================================================
        [Fact(DisplayName = "DELETE deve retornar NoContent")]
        public async Task Delete_DeveRetornarNoContent()
        {
            var entity = new Avaliacao { Id = 1 };

            _serviceMock.Setup(s => s.BuscarPorId(1)).ReturnsAsync(entity);
            _serviceMock.Setup(s => s.Remover(1)).Returns(Task.CompletedTask);

            var result = await _controller.Deletar(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "DELETE deve retornar NotFound")]
        public async Task Delete_DeveRetornarNotFound()
        {
            _serviceMock.Setup(s => s.BuscarPorId(1)).ReturnsAsync((Avaliacao?)null);

            var result = await _controller.Deletar(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
