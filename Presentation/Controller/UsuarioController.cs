using Application.Interface;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioInterface _service;
        private readonly LinkGenerator _links;

        public UsuarioController(IUsuarioInterface service, LinkGenerator links)
        {
            _service = service;
            _links = links;
        }

        // ➕ Criar usuário
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] Usuario usuario)
        {
            var criado = await _service.Criar(usuario);
            return CreatedAtAction(nameof(BuscarPorId), new { id = criado.Id, version = "1" }, criado);
        }

        // 🔎 Buscar por ID
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var usuario = await _service.BuscarPorId(id);
            if (usuario == null) return NotFound();

            return Ok(new
            {
                usuario.Id,
                usuario.FullName,
                usuario.Email,
                links = new
                {
                    self = _links.GetUriByAction(HttpContext, nameof(BuscarPorId), values: new { id, version = "1" }),
                    habilidades = _links.GetUriByAction(HttpContext, nameof(ListarHabilidades), values: new { id, version = "1" })
                }
            });
        }

        // 📄 Listar com paginação
        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] int page = 1, [FromQuery] int tamanho = 10)
        {
            if (page < 1) page = 1;

            var lista = await _service.Listar();
            var total = lista.Count();
            var dados = lista.Skip((page - 1) * tamanho).Take(tamanho);

            return Ok(new
            {
                total,
                page,
                tamanho,
                items = dados,
                links = new
                {
                    self = _links.GetUriByAction(HttpContext, nameof(Listar), values: new { page, tamanho, version = "1" })
                }
            });
        }

        // 🔗 Listar habilidades do usuário
        [HttpGet("{id:guid}/habilidades")]
        public async Task<IActionResult> ListarHabilidades(Guid id)
        {
            var usuario = await _service.BuscarPorId(id);
            if (usuario == null) return NotFound();

            return Ok(usuario.Habilidades);
        }

        // ✏️ Atualizar
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] Usuario usuario)
        {
            var atualizado = await _service.Atualizar(id, usuario);
            if (atualizado == null) return NotFound();

            return Ok(atualizado);
        }

        // ❌ Deletar
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            var removido = await _service.Deletar(id);
            return removido ? NoContent() : NotFound();
        }
    }
}
