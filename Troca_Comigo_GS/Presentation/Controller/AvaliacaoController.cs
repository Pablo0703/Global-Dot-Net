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
    [Route("api/v{version:apiVersion}/avaliacao")]
    public class AvaliacaoController : ControllerBase
    {
        private readonly IAvaliacaoInterface _service;

        public AvaliacaoController(IAvaliacaoInterface service)
        {
            _service = service;
        }

        // ➕ Criar
        [HttpPost]
        [SwaggerOperation(
            Summary = "Criar nova avaliação",
            Description = "Cria uma nova avaliação vinculada a uma troca e aos usuários avaliador/avaliado."
        )]
        [SwaggerResponse(201, "Avaliação criada com sucesso", typeof(AvaliacaoDTO))]
        [SwaggerResponse(400, "Dados inválidos enviados")]
        public async Task<IActionResult> Criar([FromBody] AvaliacaoDTO dto)
        {
            var avaliacao = new Avaliacao
            {
                TrocaId = dto.TrocaId,
                AvaliadoId = dto.AvaliadoId,
                AvaliadorId = dto.AvaliadorId,
                Nota = dto.Nota,
                Comentario = dto.Comentario,
                DataCriacao = DateTime.UtcNow
            };

            var criado = await _service.Criar(avaliacao);

            return CreatedAtAction(nameof(BuscarPorId),
                new { id = criado.Id, version = "1" },
                ToResource(criado));
        }

        // 🔎 Buscar por ID
        [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Buscar avaliação por ID",
            Description = "Retorna os detalhes de uma avaliação específica, incluindo links HATEOAS."
        )]
        [SwaggerResponse(200, "Avaliação encontrada", typeof(AvaliacaoDTO))]
        [SwaggerResponse(404, "Avaliação não encontrada")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var avaliacao = await _service.BuscarPorId(id);
            if (avaliacao == null) return NotFound();

            return Ok(ToResource(avaliacao));
        }

        // 📄 Listar por avaliado (com paginação)
        [HttpGet("avaliado/{usuarioId:int}")]
        [SwaggerOperation(
            Summary = "Listar avaliações recebidas por um usuário",
            Description = "Retorna todas as avaliações onde o usuário foi avaliado, com paginação."
        )]
        public async Task<IActionResult> ListarPorAvaliado(
            int usuarioId,
            [FromQuery] int page = 1,
            [FromQuery] int tamanho = 10)
        {
            if (page < 1) page = 1;

            var lista = await _service.ListarPorAvaliado(usuarioId);
            var total = lista.Count();

            var dados = lista
                .Skip((page - 1) * tamanho)
                .Take(tamanho)
                .ToList();

            var itens = dados.Select(a =>
            {
                var dto = ToDTO(a);
                dto.Links = new()
                {
                    new LinkDTO(Url.Action(nameof(BuscarPorId), new { id = a.Id, version = "1" }), "self", "GET")
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

        // 📄 Listar por troca (com paginação)
        [HttpGet("troca/{trocaId:int}")]
        [SwaggerOperation(
            Summary = "Listar avaliações de uma troca",
            Description = "Retorna todas as avaliações vinculadas a uma troca, com paginação."
        )]
        public async Task<IActionResult> ListarPorTroca(
            int trocaId,
            [FromQuery] int page = 1,
            [FromQuery] int tamanho = 10)
        {
            if (page < 1) page = 1;

            var lista = await _service.ListarPorTroca(trocaId);
            var total = lista.Count();

            var dados = lista
                .Skip((page - 1) * tamanho)
                .Take(tamanho)
                .ToList();

            var itens = dados.Select(a =>
            {
                var dto = ToDTO(a);
                dto.Links = new()
                {
                    new LinkDTO(Url.Action(nameof(BuscarPorId), new { id = a.Id, version = "1" }), "self", "GET")
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
            Summary = "Atualizar avaliação",
            Description = "Atualiza os campos de nota e comentário de uma avaliação existente."
        )]
        [SwaggerResponse(200, "Avaliação atualizada com sucesso", typeof(AvaliacaoDTO))]
        [SwaggerResponse(404, "Avaliação não encontrada")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AvaliacaoDTO dto)
        {
            var avaliacao = await _service.BuscarPorId(id);
            if (avaliacao == null) return NotFound();

            avaliacao.Nota = dto.Nota;
            avaliacao.Comentario = dto.Comentario;

            await _service.Atualizar(avaliacao);

            return Ok(ToResource(avaliacao));
        }

        // 📄 Listar tudo (com paginação)
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar todas as avaliações",
            Description = "Retorna todas as avaliações cadastradas com paginação e HATEOAS."
        )]
        [SwaggerResponse(200, "Lista retornada com sucesso", typeof(IEnumerable<AvaliacaoDTO>))]
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

            var itens = dados.Select(a =>
            {
                var dto = ToDTO(a);
                dto.Links = new()
                {
                    new LinkDTO(Url.Action(nameof(BuscarPorId), new { id = a.Id, version = "1" }), "self", "GET")
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

        // ❌ Deletar
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        [SwaggerOperation(
            Summary = "Excluir avaliação",
            Description = "Remove uma avaliação do sistema. Apenas administradores podem realizar essa ação."
        )]
        [SwaggerResponse(204, "Avaliação removida com sucesso")]
        [SwaggerResponse(404, "Avaliação não encontrada")]
        public async Task<IActionResult> Deletar(int id)
        {
            var avaliacao = await _service.BuscarPorId(id);
            if (avaliacao == null) return NotFound();

            await _service.Remover(id);
            return NoContent();
        }

        // 🔄 HATEOAS
        private ResourceDTO<AvaliacaoDTO> ToResource(Avaliacao a)
        {
            var dto = ToDTO(a);
            var res = new ResourceDTO<AvaliacaoDTO>(dto);

            res.AddLink(Url.Action(nameof(BuscarPorId), new { id = a.Id, version = "1" }), "self", "GET");
            res.AddLink(Url.Action(nameof(Atualizar), new { id = a.Id, version = "1" }), "update", "PUT");
            res.AddLink(Url.Action(nameof(Deletar), new { id = a.Id, version = "1" }), "delete", "DELETE");

            return res;
        }

        private AvaliacaoDTO ToDTO(Avaliacao a)
        {
            return new AvaliacaoDTO
            {
                Id = a.Id,
                TrocaId = a.TrocaId,
                AvaliadoId = a.AvaliadoId,
                AvaliadorId = a.AvaliadorId,
                Nota = a.Nota,
                Comentario = a.Comentario,
                DataCriacao = a.DataCriacao
            };
        }
    }
}
