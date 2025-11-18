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
    [Route("api/v{version:apiVersion}/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioInterface _service;

        public UsuarioController(IUsuarioInterface service)
        {
            _service = service;
        }

        // ➕ Criar usuário
        [HttpPost]
        [SwaggerOperation(
            Summary = "Criar novo usuário",
            Description = "Cria um novo usuário com nome, e-mail, senha, cargo, bio, avatar, localização e créditos iniciais."
        )]
        [SwaggerResponse(201, "Usuário criado com sucesso", typeof(UsuarioDTO))]
        [SwaggerResponse(400, "Dados inválidos enviados")]
        public async Task<IActionResult> Criar([FromBody] UsuarioDTO dto)
        {
            var entity = new Usuario
            {
                NomeCompleto = dto.NomeCompleto,
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
            var resource = ToResource(criado);

            return CreatedAtAction(nameof(BuscarPorId),
                new { id = criado.Id, version = "1" },
                resource);
        }

        // 🔎 Buscar por ID
        [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Buscar usuário por ID",
            Description = "Retorna os detalhes completos de um usuário específico, incluindo links HATEOAS."
        )]
        [SwaggerResponse(200, "Usuário encontrado", typeof(UsuarioDTO))]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var usuario = await _service.BuscarPorId(id);
            if (usuario == null) return NotFound();

            return Ok(ToResource(usuario));
        }

        // 📄 Listar com paginação + HATEOAS
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar usuários com paginação",
            Description = "Retorna uma lista paginada de usuários, incluindo links HATEOAS e metadados da paginação."
        )]
        [SwaggerResponse(200, "Lista retornada com sucesso")]
        public async Task<IActionResult> Listar([FromQuery] int page = 1, [FromQuery] int tamanho = 10)
        {
            if (page < 1) page = 1;

            var lista = await _service.Listar();
            var total = lista.Count();
            var dados = lista.Skip((page - 1) * tamanho).Take(tamanho).ToList();

            var usuariosComLinks = dados.Select(u => ToDTO(u)).Select(dto =>
            {
                dto.Links = new List<LinkDTO>
                {
                    new LinkDTO(Url.Action(nameof(BuscarPorId), new { id = dto.Id, version = "1" }), "self", "GET"),
                    new LinkDTO(Url.Action(nameof(Atualizar), new { id = dto.Id, version = "1" }), "update", "PUT"),
                    new LinkDTO(Url.Action(nameof(Deletar), new { id = dto.Id, version = "1" }), "delete", "DELETE")
                };

                return dto;
            });

            var rootLinks = new List<LinkDTO>
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
                items = usuariosComLinks,
                _links = rootLinks
            });
        }

        // ✏️ Atualizar
        [HttpPut("{id:int}")]
        [SwaggerOperation(
            Summary = "Atualizar usuário",
            Description = "Atualiza os dados cadastrais de um usuário existente."
        )]
        [SwaggerResponse(200, "Usuário atualizado com sucesso", typeof(UsuarioDTO))]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] UsuarioDTO dto)
        {
            var entity = await _service.BuscarPorId(id);
            if (entity == null) return NotFound();

            entity.NomeCompleto = dto.NomeCompleto;
            entity.Email = dto.Email;
            entity.Password = dto.Password;
            entity.Role = dto.Role;
            entity.Bio = dto.Bio;
            entity.AvatarUrl = dto.AvatarUrl;
            entity.Location = dto.Location;
            entity.Timezone = dto.Timezone;
            entity.LinkedinUrl = dto.LinkedinUrl;

            var atualizado = await _service.Atualizar(id, entity);

            return Ok(ToResource(atualizado));
        }

        // ❌ Deletar
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        [SwaggerOperation(
            Summary = "Excluir usuário",
            Description = "Remove um usuário do sistema. Apenas administradores têm permissão para executar esta ação."
        )]
        [SwaggerResponse(204, "Usuário removido com sucesso")]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<IActionResult> Deletar(int id)
        {
            var removido = await _service.Deletar(id);
            return removido ? NoContent() : NotFound();
        }

        // DTO -> Resource com links
        private ResourceDTO<UsuarioDTO> ToResource(Usuario u)
        {
            var dto = ToDTO(u);
            var resource = new ResourceDTO<UsuarioDTO>(dto);

            resource.AddLink(Url.Action(nameof(BuscarPorId), new { id = u.Id, version = "1" }), "self", "GET");
            resource.AddLink(Url.Action(nameof(Atualizar), new { id = u.Id, version = "1" }), "update", "PUT");
            resource.AddLink(Url.Action(nameof(Deletar), new { id = u.Id, version = "1" }), "delete", "DELETE");

            return resource;
        }

        // 🔄 Entity → DTO
        private UsuarioDTO ToDTO(Usuario u)
        {
            return new UsuarioDTO
            {
                Id = u.Id,
                NomeCompleto = u.NomeCompleto,
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
