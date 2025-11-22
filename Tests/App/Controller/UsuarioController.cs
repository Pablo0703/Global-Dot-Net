using Application.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Presentation.Controllers.v1;
using Presentation.DTOs;
using Domain.Entities;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Tests.App.Controllers
{
    public class UsuarioControllerTest
    {
        private readonly Mock<IUsuarioInterface> _serviceMock;
        private readonly UsuarioController _controller;

        public UsuarioControllerTest()
        {
            _serviceMock = new Mock<IUsuarioInterface>();
            _controller = new UsuarioController(_serviceMock.Object);

            // Mock seguro para evitar NullReference no HATEOAS
            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("http://localhost/api/v1/usuarios/1");

            _controller.Url = urlHelperMock.Object;
        }

        // =============================
        // GET /usuarios/{id}
        // =============================
        [Fact(DisplayName = "GET por ID deve retornar usuário existente")]
        public async Task BuscarPorId_DeveRetornarUsuario()
        {
            var entity = new Usuario
            {
                Id = 1,
                NomeCompleto = "João Silva",
                Email = "joao@email.com"
            };

            _serviceMock.Setup(s => s.BuscarPorId(1)).ReturnsAsync(entity);

            var result = await _controller.BuscarPorId(1);
            var ok = Assert.IsType<OkObjectResult>(result);

            var response = ok.Value!;
            var dataProp = response.GetType().GetProperty("Data");
            var dataValue = dataProp?.GetValue(response) as UsuarioDTO;

            Assert.NotNull(dataValue);
            Assert.Equal("João Silva", dataValue.NomeCompleto);
        }

        // =============================
        // GET paginado
        // =============================
        [Fact(DisplayName = "GET deve retornar lista paginada de usuários")]
        public async Task Listar_DeveRetornarLista()
        {
            var lista = new List<Usuario>
            {
                new Usuario { Id = 1, NomeCompleto = "Teste", Email = "t@t.com" }
            };

            _serviceMock.Setup(s => s.Listar()).ReturnsAsync(lista);

            var result = await _controller.Listar(1, 10);
            var ok = Assert.IsType<OkObjectResult>(result);

            var response = ok.Value!;
            var itemsProp = response.GetType().GetProperty("items");
            var itemsList = itemsProp?.GetValue(response) as IEnumerable<UsuarioDTO>;

            Assert.NotNull(itemsList);
            Assert.True(itemsList.Any());
        }

        // =============================
        // POST /usuarios
        // =============================
        [Fact(DisplayName = "POST deve retornar Created quando usuário for criado")]
        public async Task Criar_DeveRetornarCreated()
        {
            var entity = new Usuario
            {
                Id = 1,
                NomeCompleto = "Novo User",
                Email = "novo@teste.com"
            };

            _serviceMock.Setup(s => s.Criar(It.IsAny<Usuario>()))
                        .ReturnsAsync(entity);

            var dto = new UsuarioDTO
            {
                NomeCompleto = "Novo User",
                Email = "novo@teste.com",
                Password = "123456",
                Role = "USER"
            };

            var result = await _controller.Criar(dto);
            var created = Assert.IsType<CreatedAtActionResult>(result);

            var response = created.Value!;
            var dataProp = response.GetType().GetProperty("Data");
            var dataValue = dataProp?.GetValue(response) as UsuarioDTO;

            Assert.NotNull(dataValue);
            Assert.Equal("Novo User", dataValue.NomeCompleto);
        }

        // =============================
        // PUT /usuarios/{id}
        // =============================
        [Fact(DisplayName = "PUT deve retornar Ok quando atualizar usuário")]
        public async Task Atualizar_DeveRetornarOk()
        {
            var entity = new Usuario { Id = 1, NomeCompleto = "Antigo" };

            _serviceMock.Setup(s => s.BuscarPorId(1)).ReturnsAsync(entity);
            _serviceMock.Setup(s => s.Atualizar(1, It.IsAny<Usuario>()))
                        .ReturnsAsync(entity);

            var dto = new UsuarioDTO { NomeCompleto = "Atualizado" };

            var result = await _controller.Atualizar(1, dto);
            var ok = Assert.IsType<OkObjectResult>(result);

            var response = ok.Value!;
            var data = response.GetType().GetProperty("Data")?.GetValue(response) as UsuarioDTO;

            Assert.NotNull(data);
        }

        // =============================
        // DELETE /usuarios/{id}
        // =============================
        [Fact(DisplayName = "DELETE deve retornar NoContent quando remover com sucesso")]
        public async Task Delete_DeveRetornarNoContent()
        {
            _serviceMock.Setup(s => s.Deletar(1)).ReturnsAsync(true);

            var result = await _controller.Deletar(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "DELETE deve retornar NotFound quando ID não existir")]
        public async Task Delete_DeveRetornarNotFound()
        {
            _serviceMock.Setup(s => s.Deletar(99)).ReturnsAsync(false);

            var result = await _controller.Deletar(99);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
