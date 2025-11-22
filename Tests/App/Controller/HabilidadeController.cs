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
    public class HabilidadeControllerTest
    {
        private readonly Mock<IHabilidadeInterface> _serviceMock;
        private readonly HabilidadeController _controller;

        public HabilidadeControllerTest()
        {
            _serviceMock = new Mock<IHabilidadeInterface>();
            _controller = new HabilidadeController(_serviceMock.Object);

            var urlMock = new Mock<IUrlHelper>();
            urlMock.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                   .Returns("http://localhost/api/v1/habilidades/1");

            _controller.Url = urlMock.Object;
        }

        // ====================================================
        // GET /habilidades
        // ====================================================
        [Fact(DisplayName = "GET deve retornar lista de habilidades")]
        public async Task Listar_DeveRetornarLista()
        {
            var lista = new List<Habilidade>
    {
        new Habilidade { Id = 1, Nome = "C# Básico" }
    };

            _serviceMock.Setup(s => s.Listar()).ReturnsAsync(lista);

            var result = await _controller.Listar();

            var ok = Assert.IsType<OkObjectResult>(result);
            var response = ok.Value!;

            var itemsProp = response.GetType().GetProperty("items");
            var dtos = itemsProp?.GetValue(response) as IEnumerable<HabilidadeDTO>;

            Assert.NotNull(dtos);
            Assert.True(dtos.Any());
        }


        // ====================================================
        // GET /habilidades/{id}
        // ====================================================
        [Fact(DisplayName = "GET por ID deve retornar habilidade existente")]
        public async Task BuscarPorId_DeveRetornarHabilidade()
        {
            var entity = new Habilidade
            {
                Id = 1,
                Nome = "Banco de Dados"
            };

            _serviceMock.Setup(s => s.BuscarPorId(1)).ReturnsAsync(entity);

            var result = await _controller.BuscarPorId(1);

            var ok = Assert.IsType<OkObjectResult>(result);

            var resource = ok.Value!;
            var data = resource.GetType().GetProperty("Data")?.GetValue(resource) as HabilidadeDTO;

            Assert.NotNull(data);
            Assert.Equal("Banco de Dados", data.Nome);
        }

        // ====================================================
        // POST /habilidades
        // ====================================================
        [Fact(DisplayName = "POST deve retornar Created")]
        public async Task Criar_DeveRetornarCreated()
        {
            var entity = new Habilidade
            {
                Id = 1,
                Nome = "C#"
            };

            _serviceMock.Setup(s => s.Criar(It.IsAny<Habilidade>()))
                        .ReturnsAsync(entity);

            var dto = new HabilidadeDTO
            {
                Nome = "C#"
            };

            var result = await _controller.Criar(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var resource = created.Value!;
            var data = resource.GetType().GetProperty("Data")!.GetValue(resource) as HabilidadeDTO;

            Assert.NotNull(data);
            Assert.Equal("C#", data.Nome);
        }

        // ====================================================
        // PUT /habilidades/{id}
        // ====================================================
        [Fact(DisplayName = "PUT deve retornar Ok ao atualizar habilidade")]
        public async Task Atualizar_DeveRetornarOk()
        {
            var entity = new Habilidade { Id = 1, Nome = "Java" };

            _serviceMock.Setup(s => s.Atualizar(1, It.IsAny<Habilidade>()))
                        .ReturnsAsync(entity);

            var dto = new HabilidadeDTO { Nome = "Java Atualizado" };

            var result = await _controller.Atualizar(1, dto);

            var ok = Assert.IsType<OkObjectResult>(result);
            var resource = ok.Value!;
            var data = resource.GetType().GetProperty("Data")!.GetValue(resource) as HabilidadeDTO;

            Assert.NotNull(data);
        }

        // ====================================================
        // DELETE /habilidades/{id}
        // ====================================================
        [Fact(DisplayName = "DELETE deve retornar NoContent")]
        public async Task Deletar_DeveRetornarNoContent()
        {
            _serviceMock.Setup(s => s.Deletar(1)).ReturnsAsync(true);

            var result = await _controller.Deletar(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "DELETE deve retornar NotFound quando não existir")]
        public async Task Deletar_DeveRetornarNotFound()
        {
            _serviceMock.Setup(s => s.Deletar(99)).ReturnsAsync(false);

            var result = await _controller.Deletar(99);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
