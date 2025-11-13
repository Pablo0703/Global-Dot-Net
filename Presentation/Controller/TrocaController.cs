using Application.Interface;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/trocas")]
    public class TrocaController : ControllerBase
    {
        private readonly ITrocaInterface _service;

        public TrocaController(ITrocaInterface service)
        {
            _service = service;
        }

        // ➕ Criar
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] Troca troca)
        {
            var criado = await _service.Criar(troca);
            return CreatedAtAction(nameof(BuscarPorId), new { id = criado.Id, version = "1" }, criado);
        }

        // 🔎 Buscar por ID
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var troca = await _service.BuscarPorId(id);
            return troca == null ? NotFound() : Ok(troca);
        }

        // 📄 Listar
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            return Ok(await _service.Listar());
        }

        // 📄 Listar por mentor
        [HttpGet("mentor/{id:guid}")]
        public async Task<IActionResult> ListarPorMentor(Guid id)
        {
            return Ok(await _service.ListarPorMentor(id));
        }

        // 📄 Listar por aluno
        [HttpGet("aluno/{id:guid}")]
        public async Task<IActionResult> ListarPorAluno(Guid id)
        {
            return Ok(await _service.ListarPorAluno(id));
        }

        // 🔄 Atualizar status
        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> AtualizarStatus(Guid id, [FromBody] string status)
        {
            var atualizado = await _service.AtualizarStatus(id, status);
            return atualizado == null ? NotFound() : Ok(atualizado);
        }

        // ❌ Deletar
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            return await _service.Deletar(id) ? NoContent() : NotFound();
        }
    }
}
