using Application.Interface;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Presentation.DTOs;
using System.Diagnostics;

namespace Troca_Comigo_GS.Presentation.Controller
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/avaliacoes")]
    public class AvaliacaoController : ControllerBase
    {
        private readonly IAvaliacaoInterface _service;
        private static readonly ActivitySource Activity = new("TrocaComigo.Avaliacao");

        public AvaliacaoController(IAvaliacaoInterface service)
        {
            _service = service;
        }

        // =====================================================
        // 🔵 1. CRIAR AVALIAÇÃO
        // =====================================================
        [HttpPost]
        [SwaggerOperation(
            Summary = "Criar avaliação",
            Description = "Cria uma nova avaliação entre usuários."
        )]
        [SwaggerResponse(201, "Avaliação criada com sucesso", typeof(AvaliacaoDTO))]
        [SwaggerResponse(400, "Dados inválidos")]
        public async Task<IActionResult> Criar([FromBody] AvaliacaoDTO dto)
        {
            using var activity = Activity.StartActivity("Criar Avaliação");

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
                new { id = criado.Id },
                ToDTO(criado));
        }

        // =====================================================
        // 🔵 2. BUSCAR POR ID
        // =====================================================
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Buscar avaliação por ID")]
        [SwaggerResponse(200, "Avaliação localizada", typeof(AvaliacaoDTO))]
        [SwaggerResponse(404, "Avaliação não encontrada")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            using var activity = Activity.StartActivity("Buscar Avaliação Por ID");

            var avaliacao = await _service.BuscarPorId(id);

            if (avaliacao == null)
                return NotFound(new { Mensagem = "Avaliação não encontrada" });

            return Ok(ToDTO(avaliacao));
        }

        // =====================================================
        // 🔵 3. LISTAR POR AVALIADO
        // =====================================================
        [HttpGet("avaliado/{usuarioId}")]
        [SwaggerOperation(Summary = "Listar avaliações recebidas por um usuário")]
        [SwaggerResponse(200, "Lista retornada com sucesso", typeof(IEnumerable<AvaliacaoDTO>))]
        public async Task<IActionResult> ListarPorAvaliado(int usuarioId)
        {
            using var activity = Activity.StartActivity("Listar Avaliações Por Avaliado");

            var lista = await _service.ListarPorAvaliado(usuarioId);

            return Ok(lista.Select(ToDTO).ToList());
        }

        // =====================================================
        // 🔵 4. LISTAR POR TROCA
        // =====================================================
        [HttpGet("troca/{trocaId}")]
        [SwaggerOperation(Summary = "Listar avaliações vinculadas a uma troca")]
        [SwaggerResponse(200, "Lista retornada com sucesso", typeof(IEnumerable<AvaliacaoDTO>))]
        public async Task<IActionResult> ListarPorTroca(int trocaId)
        {
            using var activity = Activity.StartActivity("Listar Avaliações Por Troca");

            var lista = await _service.ListarPorTroca(trocaId);

            return Ok(lista.Select(ToDTO).ToList());
        }

        // =====================================================
        // 🔵 5. ATUALIZAR
        // =====================================================
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualizar avaliação")]
        [SwaggerResponse(200, "Avaliação atualizada", typeof(AvaliacaoDTO))]
        [SwaggerResponse(404, "Avaliação não encontrada")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AvaliacaoDTO dto)
        {
            using var activity = Activity.StartActivity("Atualizar Avaliação");

            var avaliacao = await _service.BuscarPorId(id);

            if (avaliacao == null)
                return NotFound(new { Mensagem = "Avaliação não encontrada" });

            avaliacao.Nota = dto.Nota;
            avaliacao.Comentario = dto.Comentario;

            // ❗ NÃO RECRIA DATA DE CRIAÇÃO
            // avaliacao.DataCriacao permanece a mesma

            await _service.Atualizar(avaliacao);

            return Ok(ToDTO(avaliacao));
        }

        // =====================================================
        // 🔵 6. DELETE
        // =====================================================
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Excluir avaliação")]
        [SwaggerResponse(204, "Avaliação removida")]
        [SwaggerResponse(404, "Avaliação não encontrada")]
        public async Task<IActionResult> Deletar(int id)
        {
            using var activity = Activity.StartActivity("Deletar Avaliação");

            var avaliacao = await _service.BuscarPorId(id);

            if (avaliacao == null)
                return NotFound(new { Mensagem = "Avaliação não encontrada" });

            await _service.Remover(id);

            return NoContent();
        }

        // =====================================================
        // 🔵 MAPEAMENTO DTO
        // =====================================================
        private static AvaliacaoDTO ToDTO(Avaliacao a)
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
