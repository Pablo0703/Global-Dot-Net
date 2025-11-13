using Application.Interface;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/transacoes")]
    public class TransacaoController : ControllerBase
    {
        private readonly ITransacaoInterface _service;

        public TransacaoController(ITransacaoInterface service)
        {
            _service = service;
        }

        // ➕ Criar
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] Transacao transacao)
        {
            var criado = await _service.Criar(transacao);
            return CreatedAtAction(nameof(BuscarPorId), new { id = criado.Id, version = "1" }, criado);
        }

        // 🔎 Buscar
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var t = await _service.BuscarPorId(id);
            return t == null ? NotFound() : Ok(t);
        }

        // 📄 Listar
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            return Ok(await _service.Listar());
        }

        // 📄 Listar por usuário
        [HttpGet("usuario/{id:guid}")]
        public async Task<IActionResult> ListarPorUsuario(Guid id)
        {
            return Ok(await _service.ListarPorUsuario(id));
        }

        // 🔄 Concluir
        [HttpPatch("{id:guid}/concluir")]
        public async Task<IActionResult> Concluir(Guid id)
        {
            var t = await _service.Concluir(id);
            return t == null ? NotFound() : Ok(t);
        }

        // 🔄 Estornar
        [HttpPatch("{id:guid}/estornar")]
        public async Task<IActionResult> Estornar(Guid id)
        {
            var t = await _service.Estornar(id);
            return t == null ? NotFound() : Ok(t);
        }
    }
}
