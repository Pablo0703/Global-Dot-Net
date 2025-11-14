using Application.Interface;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(
            Summary = "Criar novo usuário",
            Description = "Endpoint responsável pela criação de um novo usuário."
        )]
        [SwaggerResponse(201, "Usuário criado com sucesso", typeof(UsuarioDTO))]
        [SwaggerResponse(400, "Dados inválidos")]
        public async Task<IActionResult> Criar([FromBody] UsuarioDTO dto)
        {
            var entity = new Usuario
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = dto.Password,
                Role = dto.Role,
                Bio = dto.Bio,
                AvatarUrl = dto.AvatarUrl,
                Location = dto.Location,
                Timezone = dto.Timezone,
                LinkedinUrl = dto.LinkedinUrl,
                TimeCredits = dto.TimeCredits
            };

            var criado = await _service.Criar(entity);

            return CreatedAtAction(nameof(BuscarPorId),
                new { id = criado.Id, version = "1" },
                ToDTO(criado));
        }

        // 🔎 Buscar por ID
        [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Buscar usuário por ID",
            Description = "Retorna os dados de um usuário específico."
        )]
        [SwaggerResponse(200, "Usuário encontrado", typeof(UsuarioDTO))]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var usuario = await _service.BuscarPorId(id);
            if (usuario == null) return NotFound();

            return Ok(ToDTO(usuario));
        }

        // 📄 Listar com paginação
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar usuários",
            Description = "Retorna usuários com suporte a paginação."
        )]
        [SwaggerResponse(200, "Lista retornada", typeof(IEnumerable<UsuarioDTO>))]
        public async Task<IActionResult> Listar([FromQuery] int page = 1, [FromQuery] int tamanho = 10)
        {
            if (page < 1) page = 1;

            var lista = await _service.Listar();
            var total = lista.Count();
            var dados = lista.Skip((page - 1) * tamanho).Take(tamanho).Select(ToDTO);

            return Ok(new
            {
                total,
                page,
                tamanho,
                items = dados
            });
        }

        // 🔗 Listar habilidades
        [HttpGet("{id:guid}/habilidades")]
        [SwaggerOperation(
            Summary = "Listar habilidades de um usuário",
            Description = "Retorna todas as habilidades vinculadas ao usuário."
        )]
        [SwaggerResponse(200, "Habilidades encontradas")]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<IActionResult> ListarHabilidades(int id)
        {
            var usuario = await _service.BuscarPorId(id);
            if (usuario == null) return NotFound();

            return Ok(usuario.Habilidades);
        }

        // ✏️ Atualizar usuário
        [HttpPut("{id:int}")]
        [SwaggerOperation(
            Summary = "Atualizar usuário",
            Description = "Atualiza os dados de um usuário existente."
        )]
        [SwaggerResponse(200, "Usuário atualizado", typeof(UsuarioDTO))]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] UsuarioDTO dto)
        {
            var entity = await _service.BuscarPorId(id);
            if (entity == null) return NotFound();

            entity.FullName = dto.FullName;
            entity.Email = dto.Email;
            entity.Password = dto.Password;
            entity.Role = dto.Role;
            entity.Bio = dto.Bio;
            entity.AvatarUrl = dto.AvatarUrl;
            entity.Location = dto.Location;
            entity.Timezone = dto.Timezone;
            entity.LinkedinUrl = dto.LinkedinUrl;

            var atualizado = await _service.Atualizar(id, entity);

            return Ok(ToDTO(atualizado));
        }

        // ❌ Deletar usuário
        [HttpDelete("{id:int}")]
        [SwaggerOperation(
            Summary = "Excluir usuário",
            Description = "Remove um usuário da plataforma."
        )]
        [SwaggerResponse(204, "Usuário removido com sucesso")]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<IActionResult> Deletar(int id)
        {
            var removido = await _service.Deletar(id);
            return removido ? NoContent() : NotFound();
        }

        // 🔄 Entity → DTO
        private UsuarioDTO ToDTO(Usuario u)
        {
            return new UsuarioDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Password = u.Password,
                Role = u.Role,
                Bio = u.Bio,
                AvatarUrl = u.AvatarUrl,
                Location = u.Location,
                Timezone = u.Timezone,
                LinkedinUrl = u.LinkedinUrl,
                TimeCredits = u.TimeCredits,
                TotalSessionsGiven = u.TotalSessionsGiven,
                TotalSessionsTaken = u.TotalSessionsTaken,
                AverageRating = u.AverageRating,
                CreatedDate = u.CreatedDate
            };
        }
    }
}
