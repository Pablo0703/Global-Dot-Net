using Application.Interface;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "Criar habilidade", Description = "Cria uma nova habilidade para um usuário.")]
        [SwaggerResponse(201, "Habilidade criada com sucesso", typeof(HabilidadeDTO))]
        public async Task<IActionResult> Criar([FromBody] HabilidadeDTO dto)
        {
            var entidade = FromDTO(dto);
            var criado = await _service.Criar(entidade);

            return CreatedAtAction(nameof(BuscarPorId),
                new { id = criado.Id, version = "1" },
                ToDTO(criado));
        }

        // 🔎 Buscar por ID
        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Buscar habilidade por ID")]
        [SwaggerResponse(200, "Habilidade encontrada", typeof(HabilidadeDTO))]
        [SwaggerResponse(404, "Habilidade não encontrada")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var h = await _service.BuscarPorId(id);
            return h == null ? NotFound() : Ok(ToDTO(h));
        }

        // 📄 Listar todas
        [HttpGet]
        [SwaggerOperation(Summary = "Listar todas as habilidades")]
        public async Task<IActionResult> Listar()
        {
            var lista = await _service.Listar();
            return Ok(lista.Select(ToDTO));
        }

        // 📄 Listar por usuário
        [HttpGet("usuario/{usuarioId:int}")]
        [SwaggerOperation(Summary = "Listar habilidades de um usuário específico")]
        public async Task<IActionResult> ListarPorUsuario(int usuarioId)
        {
            var lista = await _service.ListarPorUsuario(usuarioId);
            return Ok(lista.Select(ToDTO));
        }

        // ✏️ Atualizar
        [HttpPut("{id:int}")]
        [SwaggerOperation(Summary = "Atualizar habilidade")]
        [SwaggerResponse(200, "Habilidade atualizada", typeof(HabilidadeDTO))]
        [SwaggerResponse(404, "Habilidade não encontrada")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] HabilidadeDTO dto)
        {
            var atualizado = await _service.Atualizar(id, FromDTO(dto));
            return atualizado == null ? NotFound() : Ok(ToDTO(atualizado));
        }

        // ❌ Deletar
        [HttpDelete("{id:int}")]
        [SwaggerOperation(Summary = "Excluir habilidade")]
        [SwaggerResponse(204, "Habilidade removida")]
        [SwaggerResponse(404, "Habilidade não encontrada")]
        public async Task<IActionResult> Deletar(int id)
        {
            return await _service.Deletar(id) ? NoContent() : NotFound();
        }

        // ============================================================
        // 🔵 MAPEAMENTO ENTITY → DTO
        // ============================================================
        private static HabilidadeDTO ToDTO(Habilidade h)
        {
            return new HabilidadeDTO
            {
                Id = h.Id,
                Nome = h.Nome,
                Categoria = h.Categoria,
                Descricao = h.Descricao,
                Nivel = h.Nivel,
                IsOffering = h.IsOffering,
                IsSeeking = h.IsSeeking,
                ValorPorHora = h.ValorPorHora,
                UsuarioId = h.UsuarioId,
                DataCriacao = h.DataCriacao
            };
        }

        // ============================================================
        // 🔵 MAPEAMENTO DTO → ENTITY
        // ============================================================
        private static Habilidade FromDTO(HabilidadeDTO dto)
        {
            return new Habilidade
            {
                Nome = dto.Nome,
                Categoria = dto.Categoria,
                Descricao = dto.Descricao,
                Nivel = dto.Nivel,
                IsOffering = dto.IsOffering,
                IsSeeking = dto.IsSeeking,
                ValorPorHora = dto.ValorPorHora,
                UsuarioId = dto.UsuarioId,
                DataCriacao = dto.DataCriacao == default ? DateTime.UtcNow : dto.DataCriacao
            };
        }
    }
}
