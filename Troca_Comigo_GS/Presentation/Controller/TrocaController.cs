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
    [Route("api/v{version:apiVersion}/troca")]
    public class TrocaController : ControllerBase
    {
        private readonly ITrocaInterface _service;

        public TrocaController(ITrocaInterface service)
        {
            _service = service;
        }

        // ➕ Criar
        [HttpPost]
        [SwaggerOperation(
            Summary = "Criar nova troca",
            Description = "Cria uma nova troca entre mentor e aluno, incluindo habilidade e horários."
        )]
        [SwaggerResponse(201, "Troca criada com sucesso", typeof(TrocaDTO))]
        [SwaggerResponse(400, "Dados inválidos enviados")]
        public async Task<IActionResult> Criar([FromBody] TrocaDTO dto)
        {
            var entity = FromDTO(dto);
            var criado = await _service.Criar(entity);

            return CreatedAtAction(nameof(BuscarPorId),
                new { id = criado.Id, version = "1" },
                ToResource(criado));
        }

        // 🔎 Buscar por ID
        [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Buscar troca por ID",
            Description = "Retorna os detalhes completos de uma troca."
        )]
        [SwaggerResponse(200, "Troca encontrada", typeof(TrocaDTO))]
        [SwaggerResponse(404, "Troca não encontrada")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var troca = await _service.BuscarPorId(id);
            if (troca == null) return NotFound();

            return Ok(ToResource(troca));
        }

        // 📄 Listar todas — PAGINADO
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar todas as trocas",
            Description = "Retorna todas as trocas registradas, com paginação e HATEOAS."
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
                    new LinkDTO(Url.Action(nameof(BuscarPorId), new { id = dto.Id, version = "1" }), "self", "GET")
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

        // 📄 Listar por mentor — PAGINADO
        [HttpGet("mentor/{mentorId:int}")]
        [SwaggerOperation(
            Summary = "Listar trocas por mentor",
            Description = "Retorna todas as trocas onde o usuário atua como mentor, com paginação."
        )]
        public async Task<IActionResult> ListarPorMentor(
            int mentorId,
            [FromQuery] int page = 1,
            [FromQuery] int tamanho = 10)
        {
            if (page < 1) page = 1;

            var lista = await _service.ListarPorMentor(mentorId);
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
                    new LinkDTO(Url.Action(nameof(BuscarPorId), new { id = dto.Id, version = "1" }), "self", "GET")
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

        // 📄 Listar por aluno — PAGINADO
        [HttpGet("aluno/{alunoId:int}")]
        [SwaggerOperation(
            Summary = "Listar trocas por aluno",
            Description = "Retorna todas as trocas onde o usuário atua como aluno, com paginação."
        )]
        public async Task<IActionResult> ListarPorAluno(
            int alunoId,
            [FromQuery] int page = 1,
            [FromQuery] int tamanho = 10)
        {
            if (page < 1) page = 1;

            var lista = await _service.ListarPorAluno(alunoId);
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
                    new LinkDTO(Url.Action(nameof(BuscarPorId), new { id = dto.Id, version = "1" }), "self", "GET")
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

        // ✏️ Atualizar (PUT)
        [HttpPut("{id:int}")]
        [SwaggerOperation(
            Summary = "Atualizar troca",
            Description = "Atualiza completamente os dados de uma troca existente."
        )]
        public async Task<IActionResult> Atualizar(int id, [FromBody] TrocaDTO dto)
        {
            var existente = await _service.BuscarPorId(id);
            if (existente == null) return NotFound();

            existente.MentorId = dto.MentorId;
            existente.AlunoId = dto.AlunoId;
            existente.HabilidadeId = dto.HabilidadeId;
            existente.SkillName = dto.SkillName;
            existente.ScheduledDate = dto.ScheduledDate;
            existente.DurationHours = dto.DurationHours;
            existente.Status = dto.Status;
            existente.MeetingLink = dto.MeetingLink;
            existente.Notes = dto.Notes;
            existente.CreditsValue = dto.CreditsValue;

            var atualizado = await _service.Atualizar(existente);
            return Ok(ToResource(atualizado));
        }

        // 🔄 Atualizar status (PATCH)
        [HttpPatch("{id:int}/status")]
        [SwaggerOperation(
            Summary = "Atualizar status da troca",
            Description = "Atualiza somente o status da troca."
        )]
        public async Task<IActionResult> AtualizarStatus(int id, [FromBody] string status)
        {
            var atualizado = await _service.AtualizarStatus(id, status);
            return atualizado == null ? NotFound() : Ok(ToResource(atualizado));
        }

        // ❌ Deletar
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        [SwaggerOperation(
            Summary = "Excluir troca",
            Description = "Remove uma troca do sistema."
        )]
        public async Task<IActionResult> Deletar(int id)
        {
            return await _service.Deletar(id) ? NoContent() : NotFound();
        }

        // 🔄 HATEOAS
        private ResourceDTO<TrocaDTO> ToResource(Troca t)
        {
            var dto = ToDTO(t);
            var res = new ResourceDTO<TrocaDTO>(dto);

            res.AddLink(Url.Action(nameof(BuscarPorId), new { id = t.Id, version = "1" }), "self", "GET");
            res.AddLink(Url.Action(nameof(Atualizar), new { id = t.Id, version = "1" }), "update", "PUT");
            res.AddLink(Url.Action(nameof(AtualizarStatus), new { id = t.Id, version = "1" }), "update-status", "PATCH");
            res.AddLink(Url.Action(nameof(Deletar), new { id = t.Id, version = "1" }), "delete", "DELETE");

            return res;
        }

        private static TrocaDTO ToDTO(Troca t)
        {
            return new TrocaDTO
            {
                Id = t.Id,
                MentorId = t.MentorId,
                AlunoId = t.AlunoId,
                HabilidadeId = t.HabilidadeId,
                SkillName = t.SkillName,
                ScheduledDate = t.ScheduledDate,
                DurationHours = t.DurationHours,
                Status = t.Status,
                MeetingLink = t.MeetingLink,
                Notes = t.Notes,
                CreditsValue = t.CreditsValue,
                CreatedDate = t.CreatedDate
            };
        }

        private static Troca FromDTO(TrocaDTO dto)
        {
            return new Troca
            {
                MentorId = dto.MentorId,
                AlunoId = dto.AlunoId,
                HabilidadeId = dto.HabilidadeId,
                SkillName = dto.SkillName,
                ScheduledDate = dto.ScheduledDate,
                DurationHours = dto.DurationHours,
                Status = dto.Status,
                MeetingLink = dto.MeetingLink,
                Notes = dto.Notes,
                CreditsValue = dto.CreditsValue,
                CreatedDate = dto.CreatedDate == default ? DateTime.UtcNow : dto.CreatedDate
            };
        }
    }
}
