using Application.Interface;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/avaliacoes")]
    public class AvaliacaoController : ControllerBase
    {
        private readonly IAvaliacaoInterface _service;

        public AvaliacaoController(IAvaliacaoInterface service)
        {
            _service = service;
        }

        // ➕ Criar
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] Avaliacao avaliacao)
        {
            var criado = await _service.Criar(avaliacao);
            return CreatedAtAction(nameof(BuscarPorId), new { id = criado.Id, version = "1" }, criado);
        }

        // 🔎 Buscar
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var avaliacao = await _service.BuscarPorId(id);
            return avaliacao == null ? NotFound() : Ok(avaliacao);
        }

        // 📄 Listar por usuário avaliado
        [HttpGet("avaliado/{id:guid}")]
        public async Task<IActionResult> ListarPorAvaliado(Guid id)
        {
            return Ok(await _service.ListarPorAvaliado(id));
        }

        // 📄 Listar por troca
        [HttpGet("troca/{id:guid}")]
        public async Task<IActionResult> ListarPorTroca(Guid id)
        {
            return Ok(await _service.ListarPorTroca(id));
        }
    }
}
