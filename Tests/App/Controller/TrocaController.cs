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
    public class TrocaControllerTest
    {
        private readonly Mock<ITrocaInterface> _serviceMock;
        private readonly TrocaController _controller;

        public TrocaControllerTest()
        {
            _serviceMock = new Mock<ITrocaInterface>();
            _controller = new TrocaController(_serviceMock.Object);

            var urlMock = new Mock<IUrlHelper>();
            urlMock.Setup(u => u.Action(It.IsAny<UrlActionContext>()))
                   .Returns("http://localhost/api/v1/trocas/1");

            _controller.Url = urlMock.Object;
        }

        // ============================================================
        // GET /trocas/{id}
        // ============================================================
        [Fact(DisplayName = "GET por ID deve retornar troca existente")]
        public async Task BuscarPorId_DeveRetornarTroca()
        {
            var entity = new Troca
            {
                Id = 1,
                MentorId = 10,
                AlunoId = 20,
                HabilidadeId = 5
            };

            _serviceMock.Setup(s => s.BuscarPorId(1)).ReturnsAsync(entity);

            var result = await _controller.BuscarPorId(1);
            var ok = Assert.IsType<OkObjectResult>(result);

            var resource = ok.Value!;
            var dto = resource.GetType().GetProperty("Data")?.GetValue(resource) as TrocaDTO;

            Assert.NotNull(dto);
            Assert.Equal(10, dto.MentorId);
            Assert.Equal(20, dto.AlunoId);
        }

        // ============================================================
        // GET /trocas
        // ============================================================
        [Fact(DisplayName = "GET deve retornar lista de trocas")]
        public async Task Listar_DeveRetornarLista()
        {
            var lista = new List<Troca>
    {
        new Troca { Id = 1, MentorId = 10, AlunoId = 20 }
    };

            _serviceMock.Setup(s => s.Listar()).ReturnsAsync(lista);

            var result = await _controller.Listar();

            var ok = Assert.IsType<OkObjectResult>(result);
            var response = ok.Value!;

            var itemsProp = response.GetType().GetProperty("items");
            var dtos = itemsProp?.GetValue(response) as IEnumerable<TrocaDTO>;

            Assert.NotNull(dtos);
            Assert.True(dtos.Any());
        }


        // ============================================================
        // POST /trocas
        // ============================================================
        [Fact(DisplayName = "POST deve retornar Created")]
        public async Task Criar_DeveRetornarCreated()
        {
            var entity = new Troca
            {
                Id = 1,
                MentorId = 10,
                AlunoId = 20,
                HabilidadeId = 3
            };

            _serviceMock.Setup(s => s.Criar(It.IsAny<Troca>()))
                        .ReturnsAsync(entity);

            var dto = new TrocaDTO
            {
                MentorId = 10,
                AlunoId = 20,
                HabilidadeId = 3
            };

            var result = await _controller.Criar(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var resource = created.Value!;
            var data = resource.GetType().GetProperty("Data")?.GetValue(resource) as TrocaDTO;

            Assert.NotNull(data);
            Assert.Equal(10, data.MentorId);
            Assert.Equal(3, data.HabilidadeId);
        }

        // ============================================================
        // PUT /trocas/{id}
        // ============================================================
        [Fact(DisplayName = "PUT deve retornar Ok quando atualizar troca")]
        public async Task Atualizar_DeveRetornarOk()
        {
            var entity = new Troca
            {
                Id = 1,
                MentorId = 10,
                AlunoId = 20,
                HabilidadeId = 5
            };

            _serviceMock.Setup(s => s.BuscarPorId(1)).ReturnsAsync(entity);
            _serviceMock.Setup(s => s.Atualizar(It.IsAny<Troca>()))
                        .ReturnsAsync(entity);

            var dto = new TrocaDTO
            {
                MentorId = 99,
                AlunoId = 20,
                HabilidadeId = 5
            };

            var result = await _controller.Atualizar(1, dto);

            var ok = Assert.IsType<OkObjectResult>(result);
            var resource = ok.Value!;
            var data = resource.GetType().GetProperty("Data")?.GetValue(resource) as TrocaDTO;

            Assert.NotNull(data);
        }

        // ============================================================
        // DELETE /trocas/{id}
        // ============================================================
        [Fact(DisplayName = "DELETE deve retornar NoContent quando remover com sucesso")]
        public async Task Deletar_DeveRetornarNoContent()
        {
            _serviceMock.Setup(s => s.Deletar(1)).ReturnsAsync(true);

            var result = await _controller.Deletar(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "DELETE deve retornar NotFound quando ID não existir")]
        public async Task Deletar_DeveRetornarNotFound()
        {
            _serviceMock.Setup(s => s.Deletar(99)).ReturnsAsync(false);

            var result = await _controller.Deletar(99);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
