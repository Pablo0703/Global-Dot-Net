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
        [SwaggerOperation(
            Summary = "Criar nova troca",
            Description = "Cria uma nova troca entre mentor e aluno, incluindo habilidade, horário e créditos negociados."
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
            Description = "Retorna os detalhes completos de uma troca específica, incluindo links HATEOAS."
        )]
        [SwaggerResponse(200, "Troca encontrada", typeof(TrocaDTO))]
        [SwaggerResponse(404, "Troca não encontrada")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var troca = await _service.BuscarPorId(id);
            if (troca == null) return NotFound();

            return Ok(ToResource(troca));
        }

        // 📄 Listar tudo
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar todas as trocas",
            Description = "Retorna todas as trocas registradas no sistema com links HATEOAS."
        )]
        [SwaggerResponse(200, "Lista retornada com sucesso")]
        public async Task<IActionResult> Listar()
        {
            var lista = await _service.Listar();

            return Ok(lista.Select(t =>
            {
                var dto = ToDTO(t);
                dto.Links = new()
                {
                    new LinkDTO(Url.Action(nameof(BuscarPorId), new { id = t.Id, version = "1" }), "self", "GET")
                };
                return dto;
            }));
        }

        // 📄 Listar por mentor
        [HttpGet("mentor/{mentorId:int}")]
        [SwaggerOperation(
            Summary = "Listar trocas por mentor",
            Description = "Retorna todas as trocas onde o usuário atua como mentor."
        )]
        [SwaggerResponse(200, "Lista retornada com sucesso")]
        public async Task<IActionResult> ListarPorMentor(int mentorId)
        {
            var lista = await _service.ListarPorMentor(mentorId);

            return Ok(lista.Select(t =>
            {
                var dto = ToDTO(t);
                dto.Links = new()
                {
                    new LinkDTO(Url.Action(nameof(BuscarPorId), new { id = t.Id, version = "1" }), "self", "GET")
                };
                return dto;
            }));
        }

        // 📄 Listar por aluno
        [HttpGet("aluno/{alunoId:int}")]
        [SwaggerOperation(
            Summary = "Listar trocas por aluno",
            Description = "Retorna todas as trocas onde o usuário atua como aluno."
        )]
        [SwaggerResponse(200, "Lista retornada com sucesso")]
        public async Task<IActionResult> ListarPorAluno(int alunoId)
        {
            var lista = await _service.ListarPorAluno(alunoId);

            return Ok(lista.Select(t =>
            {
                var dto = ToDTO(t);
                dto.Links = new()
                {
                    new LinkDTO(Url.Action(nameof(BuscarPorId), new { id = t.Id, version = "1" }), "self", "GET")
                };
                return dto;
            }));
        }

        // ✏️ Atualizar (PUT → atualiza tudo)
        [HttpPut("{id:int}")]
        [SwaggerOperation(
            Summary = "Atualizar troca",
            Description = "Atualiza completamente os dados de uma troca existente."
        )]
        [SwaggerResponse(200, "Troca atualizada com sucesso", typeof(TrocaDTO))]
        [SwaggerResponse(404, "Troca não encontrada")]
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
            Description = "Altera apenas o status da troca (Ex: Pendente → Confirmada → Finalizada)."
        )]
        [SwaggerResponse(200, "Status atualizado com sucesso", typeof(TrocaDTO))]
        [SwaggerResponse(404, "Troca não encontrada")]
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
            Description = "Remove uma troca do sistema. Somente administradores podem executar essa ação."
        )]
        [SwaggerResponse(204, "Troca removida com sucesso")]
        [SwaggerResponse(404, "Troca não encontrada")]
        public async Task<IActionResult> Deletar(int id)
        {
            var ok = await _service.Deletar(id);
            return ok ? NoContent() : NotFound();
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
