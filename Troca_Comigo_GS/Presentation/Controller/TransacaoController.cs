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
        [SwaggerOperation(
            Summary = "Criar nova transação",
            Description = "Cria uma nova transação de créditos entre usuários dentro de uma troca."
        )]
        [SwaggerResponse(201, "Transação criada com sucesso", typeof(TransacaoDTO))]
        [SwaggerResponse(400, "Dados inválidos enviados")]
        public async Task<IActionResult> Criar([FromBody] TransacaoDTO dto)
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
            Summary = "Buscar transação por ID",
            Description = "Retorna detalhes de uma transação específica."
        )]
        [SwaggerResponse(200, "Transação encontrada", typeof(TransacaoDTO))]
        [SwaggerResponse(404, "Transação não encontrada")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var t = await _service.BuscarPorId(id);
            if (t == null) return NotFound();

            return Ok(ToResource(t));
        }

        // 📄 Listar tudo — COM PAGINAÇÃO
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar todas as transações",
            Description = "Retorna todas as transações registradas no sistema, com paginação e HATEOAS."
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

            var itens = dados.Select(t =>
            {
                var dto = ToDTO(t);
                dto.Links = new()
                {
                    new LinkDTO(Url.Action(nameof(BuscarPorId), new { id = t.Id, version = "1" }), "self", "GET")
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

        // 📄 Listar por usuário — COM PAGINAÇÃO
        [HttpGet("usuario/{id:int}")]
        [SwaggerOperation(
            Summary = "Listar transações de um usuário",
            Description = "Retorna todas as transações onde o usuário participou, com paginação."
        )]
        [SwaggerResponse(200, "Lista retornada com sucesso")]
        public async Task<IActionResult> ListarPorUsuario(
            int id,
            [FromQuery] int page = 1,
            [FromQuery] int tamanho = 10)
        {
            if (page < 1) page = 1;

            var lista = await _service.ListarPorUsuario(id);
            var total = lista.Count();

            var dados = lista
                .Skip((page - 1) * tamanho)
                .Take(tamanho)
                .ToList();

            var itens = dados.Select(t =>
            {
                var dto = ToDTO(t);
                dto.Links = new()
                {
                    new LinkDTO(Url.Action(nameof(BuscarPorId), new { id = t.Id, version = "1" }), "self", "GET")
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
            Summary = "Atualizar transação",
            Description = "Atualiza os dados de uma transação existente."
        )]
        [SwaggerResponse(200, "Transação atualizada com sucesso", typeof(TransacaoDTO))]
        [SwaggerResponse(404, "Transação não encontrada")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] TransacaoDTO dto)
        {
            var atualizado = await _service.Atualizar(id, FromDTO(dto));
            return atualizado == null ? NotFound() : Ok(ToResource(atualizado));
        }

        // 🔄 Concluir
        [HttpPatch("{id:int}/concluir")]
        [SwaggerOperation(
            Summary = "Concluir transação",
            Description = "Marca a transação como concluída."
        )]
        public async Task<IActionResult> Concluir(int id)
        {
            var t = await _service.Concluir(id);
            return t == null ? NotFound() : Ok(ToResource(t));
        }

        // 🔄 Estornar
        [HttpPatch("{id:int}/estornar")]
        [SwaggerOperation(
            Summary = "Estornar transação",
            Description = "Reverte o crédito transferido."
        )]
        public async Task<IActionResult> Estornar(int id)
        {
            var t = await _service.Estornar(id);
            return t == null ? NotFound() : Ok(ToResource(t));
        }

        // ❌ Deletar
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        [SwaggerOperation(
            Summary = "Excluir transação",
            Description = "Remove uma transação do sistema."
        )]
        public async Task<IActionResult> Deletar(int id)
        {
            return await _service.Deletar(id) ? NoContent() : NotFound();
        }

        // 🔄 HATEOAS
        private ResourceDTO<TransacaoDTO> ToResource(Transacao t)
        {
            var dto = ToDTO(t);
            var res = new ResourceDTO<TransacaoDTO>(dto);

            res.AddLink(Url.Action(nameof(BuscarPorId), new { id = t.Id, version = "1" }), "self", "GET");
            res.AddLink(Url.Action(nameof(Atualizar), new { id = t.Id, version = "1" }), "update", "PUT");
            res.AddLink(Url.Action(nameof(Deletar), new { id = t.Id, version = "1" }), "delete", "DELETE");
            res.AddLink(Url.Action(nameof(Concluir), new { id = t.Id, version = "1" }), "concluir", "PATCH");
            res.AddLink(Url.Action(nameof(Estornar), new { id = t.Id, version = "1" }), "estornar", "PATCH");

            return res;
        }

        private static TransacaoDTO ToDTO(Transacao t)
        {
            return new TransacaoDTO
            {
                Id = t.Id,
                TrocaId = t.TrocaId,
                RemetenteId = t.RemetenteId,
                DestinatarioId = t.DestinatarioId,
                Creditos = t.Creditos,
                Tipo = t.Tipo,
                Descricao = t.Descricao,
                Status = t.Status,
                DataCriacao = t.DataCriacao
            };
        }

        private static Transacao FromDTO(TransacaoDTO dto)
        {
            return new Transacao
            {
                TrocaId = dto.TrocaId,
                RemetenteId = dto.RemetenteId,
                DestinatarioId = dto.DestinatarioId,
                Creditos = dto.Creditos,
                Tipo = dto.Tipo,
                Descricao = dto.Descricao,
                Status = dto.Status
            };
        }
    }
}
