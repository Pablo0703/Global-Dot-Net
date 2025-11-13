using Application.Interface;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/habilidades")]
    public class HabilidadeController : ControllerBase
    {
        private readonly IHabilidadeInterface _service;

        public HabilidadeController(IHabilidadeInterface service)
        {
            _service = service;
        }

        // ➕ Criar
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] Habilidade habilidade)
        {
            var criado = await _service.Criar(habilidade);
            return CreatedAtAction(nameof(BuscarPorId), new { id = criado.Id, version = "1" }, criado);
        }

        // 🔎 Buscar por ID
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var hab = await _service.BuscarPorId(id);
            return hab == null ? NotFound() : Ok(hab);
        }

        // 📄 Listar
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            return Ok(await _service.Listar());
        }

        // 📄 Listar por usuário
        [HttpGet("usuario/{usuarioId:guid}")]
        public async Task<IActionResult> ListarPorUsuario(Guid usuarioId)
        {
            return Ok(await _service.ListarPorUsuario(usuarioId));
        }

        // ✏️ Atualizar
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] Habilidade habilidade)
        {
            var atualizado = await _service.Atualizar(id, habilidade);
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
