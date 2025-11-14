using Application.Interface;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs;
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
            Summary = "Criar troca",
            Description = "Cria uma nova troca entre mentor e aluno, vinculada a uma habilidade."
        )]
        [SwaggerResponse(201, "Troca criada com sucesso", typeof(TrocaDTO))]
        [SwaggerResponse(400, "Dados inválidos")]
        public async Task<IActionResult> Criar([FromBody] TrocaDTO dto)
        {
            var entity = FromDTO(dto);
            var criado = await _service.Criar(entity);

            return CreatedAtAction(nameof(BuscarPorId),
                new { id = criado.Id, version = "1" },
                ToDTO(criado));
        }

        // 🔎 Buscar por ID
        [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Buscar troca por ID",
            Description = "Retorna os detalhes de uma troca específica."
        )]
        [SwaggerResponse(200, "Troca encontrada", typeof(TrocaDTO))]
        [SwaggerResponse(404, "Troca não encontrada")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var troca = await _service.BuscarPorId(id);
            return troca == null ? NotFound() : Ok(ToDTO(troca));
        }

        // 📄 Listar todas
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar trocas",
            Description = "Lista todas as trocas cadastradas no sistema."
        )]
        [SwaggerResponse(200, "Lista retornada com sucesso")]
        public async Task<IActionResult> Listar()
        {
            var lista = await _service.Listar();
            return Ok(lista.Select(ToDTO));
        }

        // 📄 Listar por mentor
        [HttpGet("mentor/{mentorId:int}")]
        [SwaggerOperation(
            Summary = "Listar trocas por mentor",
            Description = "Retorna todas as trocas em que o usuário atua como mentor."
        )]
        [SwaggerResponse(200, "Lista retornada com sucesso")]
        public async Task<IActionResult> ListarPorMentor(int mentorId)
        {
            var lista = await _service.ListarPorMentor(mentorId);
            return Ok(lista.Select(ToDTO));
        }

        // 📄 Listar por aluno
        [HttpGet("aluno/{alunoId:int}")]
        [SwaggerOperation(
            Summary = "Listar trocas por aluno",
            Description = "Retorna todas as trocas em que o usuário atua como aluno."
        )]
        [SwaggerResponse(200, "Lista retornada com sucesso")]
        public async Task<IActionResult> ListarPorAluno(int alunoId)
        {
            var lista = await _service.ListarPorAluno(alunoId);
            return Ok(lista.Select(ToDTO));
        }

        // 🔄 Atualizar COMPLETO
        [HttpPut("{id:int}")]
        [SwaggerOperation(
            Summary = "Atualizar troca",
            Description = "Atualiza os dados de uma troca (exceto status)."
        )]
        [SwaggerResponse(200, "Troca atualizada com sucesso", typeof(TrocaDTO))]
        [SwaggerResponse(404, "Troca não encontrada")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] TrocaDTO dto)
        {
            var entity = FromDTO(dto);
            entity.Id = id;

            var atualizado = await _service.AtualizarStatus(id, dto.Status);
            return atualizado == null ? NotFound() : Ok(ToDTO(atualizado));
        }

        // 🔄 Atualizar status específico (PATCH)
        [HttpPatch("{id:int}/status")]
        [SwaggerOperation(
            Summary = "Atualizar status da troca",
            Description = "Atualiza apenas o status da troca (AGENDADA, CONFIRMADA, CONCLUIDA...)."
        )]
        [SwaggerResponse(200, "Status atualizado com sucesso", typeof(TrocaDTO))]
        [SwaggerResponse(404, "Troca não encontrada")]
        public async Task<IActionResult> AtualizarStatus(int id, [FromBody] string status)
        {
            var atualizado = await _service.AtualizarStatus(id, status);
            return atualizado == null ? NotFound() : Ok(ToDTO(atualizado));
        }

        // ❌ Deletar
        [HttpDelete("{id:int}")]
        [SwaggerOperation(
            Summary = "Excluir troca",
            Description = "Remove permanentemente uma troca do sistema."
        )]
        [SwaggerResponse(204, "Troca removida com sucesso")]
        [SwaggerResponse(404, "Troca não encontrada")]
        public async Task<IActionResult> Deletar(int id)
        {
            var ok = await _service.Deletar(id);
            return ok ? NoContent() : NotFound();
        }

        // ============================================================
        // 🔵 MAPEAMENTO ENTITY → DTO
        // ============================================================
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

        // ============================================================
        // 🔵 MAPEAMENTO DTO → ENTITY
        // ============================================================
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
