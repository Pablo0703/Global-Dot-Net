using Application.Interface;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs;
using Presentation.Hateoas;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/habilidade")]
    public class HabilidadeController : ControllerBase
    {
        private readonly IHabilidadeInterface _service;

        public HabilidadeController(IHabilidadeInterface service)
        {
            _service = service;
        }

        // ➕ Criar
        [HttpPost]
        [SwaggerOperation(
            Summary = "Criar nova habilidade",
            Description = "Cria uma nova habilidade vinculada a um usuário, incluindo descrição, nível e categoria."
        )]
        [SwaggerResponse(201, "Habilidade criada com sucesso", typeof(HabilidadeDTO))]
        [SwaggerResponse(400, "Dados inválidos enviados")]
        public async Task<IActionResult> Criar([FromBody] HabilidadeDTO dto)
        {
            var entidade = FromDTO(dto);
            var criado = await _service.Criar(entidade);

            return CreatedAtAction(nameof(BuscarPorId),
                new { id = criado.Id, version = "1" },
                ToResource(criado));
        }

        // 🔎 Buscar por ID
        [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Buscar habilidade por ID",
            Description = "Retorna os detalhes de uma habilidade específica cadastrada no sistema."
        )]
        [SwaggerResponse(200, "Habilidade encontrada", typeof(HabilidadeDTO))]
        [SwaggerResponse(404, "Habilidade não encontrada")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var h = await _service.BuscarPorId(id);
            if (h == null) return NotFound();

            return Ok(ToResource(h));
        }

        // 📄 Listar todas (com paginação)
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar habilidades",
            Description = "Retorna todas as habilidades cadastradas com paginação e HATEOAS."
        )]
        [SwaggerResponse(200, "Lista retornada com sucesso")]
        public async Task<IActionResult> Listar(
            [FromQuery] int page = 1,
            [FromQuery] int tamanho = 10)
        {
            if (page < 1) page = 1;

            var lista = await _service.Listar();
            var total = lista.Count();

            var dados = lista
                .Skip((page - 1) * tamanho)
                .Take(tamanho)
                .ToList();

            var itens = dados.Select(h =>
            {
                var dto = ToDTO(h);
                dto.Links = new()
                {
                    new LinkDTO(Url.Action(nameof(BuscarPorId), new { id = h.Id, version = "1" }), "self", "GET")
                };
                return dto;
            });

            var links = new List<LinkDTO>
            {
                new LinkDTO(Url.Action(null, null, new { page, tamanho }), "self", "GET"),
                new LinkDTO(Url.Action(null, null, new { page = page + 1, tamanho }), "next", "GET"),
                new LinkDTO(page > 1 ? Url.Action(null, null, new { page = page - 1, tamanho }) : null, "prev", "GET")
            };

            return Ok(new
            {
                total,
                page,
                tamanho,
                items = itens,
                _links = links
            });
        }

        // 📄 Listar por usuário (com paginação)
        [HttpGet("usuario/{usuarioId:int}")]
        [SwaggerOperation(
            Summary = "Listar habilidades por usuário",
            Description = "Retorna todas as habilidades cadastradas para um usuário específico, com paginação."
        )]
        [SwaggerResponse(200, "Lista retornada com sucesso")]
        public async Task<IActionResult> ListarPorUsuario(
            int usuarioId,
            [FromQuery] int page = 1,
            [FromQuery] int tamanho = 10)
        {
            if (page < 1) page = 1;

            var lista = await _service.ListarPorUsuario(usuarioId);
            var total = lista.Count();

            var dados = lista
                .Skip((page - 1) * tamanho)
                .Take(tamanho)
                .ToList();

            var itens = dados.Select(h =>
            {
                var dto = ToDTO(h);
                dto.Links = new()
                {
                    new LinkDTO(Url.Action(nameof(BuscarPorId), new { id = h.Id, version = "1" }), "self", "GET")
                };
                return dto;
            });

            var links = new List<LinkDTO>
            {
                new LinkDTO(Url.Action(null, null, new { page, tamanho }), "self", "GET"),
                new LinkDTO(Url.Action(null, null, new { page = page + 1, tamanho }), "next", "GET"),
                new LinkDTO(page > 1 ? Url.Action(null, null, new { page = page - 1, tamanho }) : null, "prev", "GET")
            };

            return Ok(new
            {
                total,
                page,
                tamanho,
                items = itens,
                _links = links
            });
        }

        // ✏️ Atualizar
        [HttpPut("{id:int}")]
        [SwaggerOperation(
            Summary = "Atualizar habilidade",
            Description = "Atualiza os dados de uma habilidade existente no sistema."
        )]
        [SwaggerResponse(200, "Habilidade atualizada com sucesso", typeof(HabilidadeDTO))]
        [SwaggerResponse(404, "Habilidade não encontrada")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] HabilidadeDTO dto)
        {
            var atualizado = await _service.Atualizar(id, FromDTO(dto));
            if (atualizado == null) return NotFound();

            return Ok(ToResource(atualizado));
        }

        // ❌ Deletar
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        [SwaggerOperation(
            Summary = "Excluir habilidade",
            Description = "Remove uma habilidade do sistema. Apenas administradores podem realizar essa operação."
        )]
        [SwaggerResponse(204, "Habilidade removida com sucesso")]
        [SwaggerResponse(404, "Habilidade não encontrada")]
        public async Task<IActionResult> Deletar(int id)
        {
            return await _service.Deletar(id) ? NoContent() : NotFound();
        }

        // 🔄 HATEOAS
        private ResourceDTO<HabilidadeDTO> ToResource(Habilidade h)
        {
            var dto = ToDTO(h);
            var res = new ResourceDTO<HabilidadeDTO>(dto);

            res.AddLink(Url.Action(nameof(BuscarPorId), new { id = h.Id, version = "1" }), "self", "GET");
            res.AddLink(Url.Action(nameof(Atualizar), new { id = h.Id, version = "1" }), "update", "PUT");
            res.AddLink(Url.Action(nameof(Deletar), new { id = h.Id, version = "1" }), "delete", "DELETE");

            return res;
        }

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
