using Application.Interface;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs;
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
            Summary = "Criar transação",
            Description = "Cria uma transação de créditos entre dois usuários, podendo ser pagamento, bônus ou ajuste."
        )]
        [SwaggerResponse(201, "Transação criada com sucesso", typeof(TransacaoDTO))]
        [SwaggerResponse(400, "Dados inválidos")]
        public async Task<IActionResult> Criar([FromBody] TransacaoDTO dto)
        {
            var entidade = FromDTO(dto);
            var criado = await _service.Criar(entidade);

            return CreatedAtAction(nameof(BuscarPorId),
                new { id = criado.Id, version = "1" },
                ToDTO(criado));
        }

        // 🔎 Buscar por ID
        [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Buscar transação por ID",
            Description = "Obtém os detalhes completos de uma transação específica."
        )]
        [SwaggerResponse(200, "Transação encontrada", typeof(TransacaoDTO))]
        [SwaggerResponse(404, "Transação não encontrada")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var t = await _service.BuscarPorId(id);
            return t == null ? NotFound() : Ok(ToDTO(t));
        }

        // 📄 Listar todas
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar transações",
            Description = "Retorna todas as transações registradas no sistema."
        )]
        [SwaggerResponse(200, "Lista retornada com sucesso")]
        public async Task<IActionResult> Listar()
        {
            var lista = await _service.Listar();
            return Ok(lista.Select(ToDTO));
        }

        // 📄 Listar por usuário
        [HttpGet("usuario/{id:int}")]
        [SwaggerOperation(
            Summary = "Listar transações de um usuário",
            Description = "Retorna todas as transações em que o usuário aparece como remetente ou destinatário."
        )]
        [SwaggerResponse(200, "Lista retornada com sucesso")]
        public async Task<IActionResult> ListarPorUsuario(int id)
        {
            var lista = await _service.ListarPorUsuario(id);
            return Ok(lista.Select(ToDTO));
        }

        // 🔄 Atualizar (CRUD completo)
        [HttpPut("{id:int}")]
        [SwaggerOperation(
            Summary = "Atualizar transação",
            Description = "Atualiza completamente os dados de uma transação existente."
        )]
        [SwaggerResponse(200, "Transação atualizada com sucesso", typeof(TransacaoDTO))]
        [SwaggerResponse(404, "Transação não encontrada")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] TransacaoDTO dto)
        {
            var atualizado = await _service.Atualizar(id, FromDTO(dto));
            return atualizado == null ? NotFound() : Ok(ToDTO(atualizado));
        }

        // 🛑 Deletar (CRUD completo)
        [HttpDelete("{id:int}")]
        [SwaggerOperation(
            Summary = "Excluir transação",
            Description = "Remove permanentemente uma transação do sistema."
        )]
        [SwaggerResponse(204, "Transação removida com sucesso")]
        [SwaggerResponse(404, "Transação não encontrada")]
        public async Task<IActionResult> Deletar(int id)
        {
            var removido = await _service.Deletar(id);
            return removido ? NoContent() : NotFound();
        }

        // 🔄 Concluir
        [HttpPatch("{id:int}/concluir")]
        [SwaggerOperation(
            Summary = "Concluir transação",
            Description = "Marca a transação como CONCLUÍDA."
        )]
        [SwaggerResponse(200, "Transação concluída com sucesso", typeof(TransacaoDTO))]
        [SwaggerResponse(404, "Transação não encontrada")]
        public async Task<IActionResult> Concluir(int id)
        {
            var t = await _service.Concluir(id);
            return t == null ? NotFound() : Ok(ToDTO(t));
        }

        // 🔄 Estornar
        [HttpPatch("{id:int}/estornar")]
        [SwaggerOperation(
            Summary = "Estornar transação",
            Description = "Altera o status da transação para ESTORNADA."
        )]
        [SwaggerResponse(200, "Transação estornada com sucesso", typeof(TransacaoDTO))]
        [SwaggerResponse(404, "Transação não encontrada")]
        public async Task<IActionResult> Estornar(int id)
        {
            var t = await _service.Estornar(id);
            return t == null ? NotFound() : Ok(ToDTO(t));
        }

        // 🟩 DTO → Entity
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

        // 🟩 Entity → DTO
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
    }
}
