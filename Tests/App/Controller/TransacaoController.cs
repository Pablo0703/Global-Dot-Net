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
    public class TransacaoControllerTest
    {
        private readonly Mock<ITransacaoInterface> _serviceMock;
        private readonly TransacaoController _controller;

        public TransacaoControllerTest()
        {
            _serviceMock = new Mock<ITransacaoInterface>();
            _controller = new TransacaoController(_serviceMock.Object);

            var urlMock = new Mock<IUrlHelper>();
            urlMock.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                   .Returns("http://localhost/api/v1/transacoes/1");

            _controller.Url = urlMock.Object;
        }

        // ============================================================
        // GET /transacoes/{id}
        // ============================================================
        [Fact(DisplayName = "GET por ID deve retornar transação existente")]
        public async Task BuscarPorId_DeveRetornarTransacao()
        {
            var entity = new Transacao
            {
                Id = 1,
                TrocaId = 10,
                Creditos = 50,
                Status = "COMPLETED"
            };

            _serviceMock.Setup(s => s.BuscarPorId(1)).ReturnsAsync(entity);

            var result = await _controller.BuscarPorId(1);

            var ok = Assert.IsType<OkObjectResult>(result);

            var resource = ok.Value!;
            var dto = resource.GetType().GetProperty("Data")?.GetValue(resource) as TransacaoDTO;

            Assert.NotNull(dto);
            Assert.Equal(50, dto.Creditos);
            Assert.Equal("COMPLETED", dto.Status);
        }

        // ============================================================
        // GET /transacoes
        // ============================================================
        [Fact(DisplayName = "GET deve retornar lista de transações")]
        public async Task Listar_DeveRetornarLista()
        {
            var lista = new List<Transacao>
            {
                new Transacao { Id = 1, Creditos = 10 }
            };

            _serviceMock.Setup(s => s.Listar()).ReturnsAsync(lista);

            var result = await _controller.Listar();

            var ok = Assert.IsType<OkObjectResult>(result);
            var dtos = ok.Value as IEnumerable<TransacaoDTO>;

            Assert.NotNull(dtos);
            Assert.True(dtos.Any());
        }

        // ============================================================
        // GET /transacoes/usuario/{id}
        // ============================================================
        [Fact(DisplayName = "GET por usuário deve retornar lista de transações")]
        public async Task ListarPorUsuario_DeveRetornarLista()
        {
            var lista = new List<Transacao>
            {
                new Transacao { Id = 1, RemetenteId = 99 }
            };

            _serviceMock.Setup(s => s.ListarPorUsuario(99)).ReturnsAsync(lista);

            var result = await _controller.ListarPorUsuario(99);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dtos = ok.Value as IEnumerable<TransacaoDTO>;

            Assert.NotNull(dtos);
            Assert.True(dtos.Any());
        }

        // ============================================================
        // POST /transacoes
        // ============================================================
        [Fact(DisplayName = "POST deve retornar Created")]
        public async Task Criar_DeveRetornarCreated()
        {
            var entity = new Transacao
            {
                Id = 1,
                Creditos = 10,
                Tipo = "TRANSFER"
            };

            _serviceMock.Setup(s => s.Criar(It.IsAny<Transacao>()))
                        .ReturnsAsync(entity);

            var dto = new TransacaoDTO
            {
                Creditos = 10,
                Tipo = "TRANSFER"
            };

            var result = await _controller.Criar(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result);

            var resource = created.Value!;
            var data = resource.GetType().GetProperty("Data")?.GetValue(resource) as TransacaoDTO;

            Assert.NotNull(data);
            Assert.Equal(10, data.Creditos);
        }

        // ============================================================
        // PUT /transacoes/{id}
        // ============================================================
        [Fact(DisplayName = "PUT deve retornar Ok quando atualizar transação")]
        public async Task Atualizar_DeveRetornarOk()
        {
            var entity = new Transacao
            {
                Id = 1,
                Creditos = 20
            };

            _serviceMock.Setup(s => s.Atualizar(1, It.IsAny<Transacao>()))
                        .ReturnsAsync(entity);

            var dto = new TransacaoDTO { Creditos = 20 };

            var result = await _controller.Atualizar(1, dto);

            var ok = Assert.IsType<OkObjectResult>(result);
            var resource = ok.Value!;
            var data = resource.GetType().GetProperty("Data")?.GetValue(resource) as TransacaoDTO;

            Assert.NotNull(data);
            Assert.Equal(20, data.Creditos);
        }

        // ============================================================
        // PATCH concluir
        // ============================================================
        [Fact(DisplayName = "PATCH concluir deve retornar Ok")]
        public async Task Concluir_DeveRetornarOk()
        {
            var entity = new Transacao { Id = 1, Status = "COMPLETED" };

            _serviceMock.Setup(s => s.Concluir(1)).ReturnsAsync(entity);

            var result = await _controller.Concluir(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(ok.Value);
        }

        // ============================================================
        // PATCH estornar
        // ============================================================
        [Fact(DisplayName = "PATCH estornar deve retornar Ok")]
        public async Task Estornar_DeveRetornarOk()
        {
            var entity = new Transacao { Id = 1, Status = "REVERTED" };

            _serviceMock.Setup(s => s.Estornar(1)).ReturnsAsync(entity);

            var result = await _controller.Estornar(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(ok.Value);
        }

        // ============================================================
        // DELETE
        // ============================================================
        [Fact(DisplayName = "DELETE deve retornar NoContent")]
        public async Task Deletar_DeveRetornarNoContent()
        {
            _serviceMock.Setup(s => s.Deletar(1)).ReturnsAsync(true);

            var result = await _controller.Deletar(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "DELETE deve retornar NotFound")]
        public async Task Deletar_DeveRetornarNotFound()
        {
            _serviceMock.Setup(s => s.Deletar(99)).ReturnsAsync(false);

            var result = await _controller.Deletar(99);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
