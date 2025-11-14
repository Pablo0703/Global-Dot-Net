using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Troca_Comigo_GS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "RM556834");

            migrationBuilder.CreateSequence(
                name: "SEQ_AVALIACOES",
                schema: "RM556834");

            migrationBuilder.CreateSequence(
                name: "SEQ_HABILIDADES",
                schema: "RM556834");

            migrationBuilder.CreateSequence(
                name: "SEQ_TRANSACOES",
                schema: "RM556834");

            migrationBuilder.CreateSequence(
                name: "SEQ_TROCAS",
                schema: "RM556834");

            migrationBuilder.CreateSequence(
                name: "SEQ_USUARIOS",
                schema: "RM556834");

            migrationBuilder.CreateTable(
                name: "USUARIOS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    EMAIL = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    NOME_COMPLETO = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    SENHA = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    ROLE = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    BIO = table.Column<string>(type: "VARCHAR(500)", nullable: true),
                    AVATAR_URL = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    CREDITOS_TEMPO = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    TOTAL_SESSOES_DADAS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TOTAL_SESSOES_RECEBIDAS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    MEDIA_AVALIACOES = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    LOCALIZACAO = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    TIMEZONE = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    LINKEDIN_URL = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    DATA_CRIACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIOS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HABILIDADES",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    NOME = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    CATEGORIA = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    DESCRICAO = table.Column<string>(type: "VARCHAR(1000)", nullable: true),
                    NIVEL = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    OFERECE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    PROCURA = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    VALOR_HORA = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    USUARIO_ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    DATA_CRIACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HABILIDADES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HABILIDADES_USUARIOS_USUARIO_ID",
                        column: x => x.USUARIO_ID,
                        principalTable: "USUARIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioUsuario",
                columns: table => new
                {
                    Usuario1Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioUsuario", x => new { x.Usuario1Id, x.UsuarioId });
                    table.ForeignKey(
                        name: "FK_UsuarioUsuario_USUARIOS_Usuario1Id",
                        column: x => x.Usuario1Id,
                        principalTable: "USUARIOS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UsuarioUsuario_USUARIOS_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "USUARIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TROCAS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    MENTOR_ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    ALUNO_ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    HABILIDADE_ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    NOME_HABILIDADE = table.Column<string>(type: "VARCHAR(150)", nullable: true),
                    DATA_AGENDADA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DURACAO_HORAS = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    STATUS_TROCA = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    MEETING_LINK = table.Column<string>(type: "VARCHAR(500)", nullable: true),
                    NOTAS = table.Column<string>(type: "VARCHAR(2000)", nullable: true),
                    CREDITOS = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    DATA_CRIACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TROCAS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TROCAS_HABILIDADES_HABILIDADE_ID",
                        column: x => x.HABILIDADE_ID,
                        principalTable: "HABILIDADES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TROCAS_USUARIOS_ALUNO_ID",
                        column: x => x.ALUNO_ID,
                        principalTable: "USUARIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TROCAS_USUARIOS_MENTOR_ID",
                        column: x => x.MENTOR_ID,
                        principalTable: "USUARIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AVALIACOES",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    TROCA_ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    AVALIADOR_ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    AVALIADO_ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    NOTA = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    COMENTARIO = table.Column<string>(type: "VARCHAR(2000)", nullable: true),
                    DATA_CRIACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AVALIACOES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AVALIACOES_TROCAS_TROCA_ID",
                        column: x => x.TROCA_ID,
                        principalTable: "TROCAS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AVALIACOES_USUARIOS_AVALIADOR_ID",
                        column: x => x.AVALIADOR_ID,
                        principalTable: "USUARIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AVALIACOES_USUARIOS_AVALIADO_ID",
                        column: x => x.AVALIADO_ID,
                        principalTable: "USUARIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TRANSACOES",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    TROCA_ID = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    REMETENTE_ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    DESTINATARIO_ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CREDITOS = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    TIPO = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    DESCRICAO = table.Column<string>(type: "VARCHAR(500)", nullable: true),
                    STATUS = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    DATA_CRIACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRANSACOES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TRANSACOES_TROCAS_TROCA_ID",
                        column: x => x.TROCA_ID,
                        principalTable: "TROCAS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TRANSACOES_USUARIOS_DESTINATARIO_ID",
                        column: x => x.DESTINATARIO_ID,
                        principalTable: "USUARIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TRANSACOES_USUARIOS_REMETENTE_ID",
                        column: x => x.REMETENTE_ID,
                        principalTable: "USUARIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AVALIACOES_AVALIADO_ID",
                table: "AVALIACOES",
                column: "AVALIADO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_AVALIACOES_AVALIADOR_ID",
                table: "AVALIACOES",
                column: "AVALIADOR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_AVALIACOES_TROCA_ID",
                table: "AVALIACOES",
                column: "TROCA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HABILIDADES_USUARIO_ID",
                table: "HABILIDADES",
                column: "USUARIO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TRANSACOES_DESTINATARIO_ID",
                table: "TRANSACOES",
                column: "DESTINATARIO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TRANSACOES_REMETENTE_ID",
                table: "TRANSACOES",
                column: "REMETENTE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TRANSACOES_TROCA_ID",
                table: "TRANSACOES",
                column: "TROCA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TROCAS_ALUNO_ID",
                table: "TROCAS",
                column: "ALUNO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TROCAS_HABILIDADE_ID",
                table: "TROCAS",
                column: "HABILIDADE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TROCAS_MENTOR_ID",
                table: "TROCAS",
                column: "MENTOR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioUsuario_UsuarioId",
                table: "UsuarioUsuario",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AVALIACOES");

            migrationBuilder.DropTable(
                name: "TRANSACOES");

            migrationBuilder.DropTable(
                name: "UsuarioUsuario");

            migrationBuilder.DropTable(
                name: "TROCAS");

            migrationBuilder.DropTable(
                name: "HABILIDADES");

            migrationBuilder.DropTable(
                name: "USUARIOS");

            migrationBuilder.DropSequence(
                name: "SEQ_AVALIACOES",
                schema: "RM556834");

            migrationBuilder.DropSequence(
                name: "SEQ_HABILIDADES",
                schema: "RM556834");

            migrationBuilder.DropSequence(
                name: "SEQ_TRANSACOES",
                schema: "RM556834");

            migrationBuilder.DropSequence(
                name: "SEQ_TROCAS",
                schema: "RM556834");

            migrationBuilder.DropSequence(
                name: "SEQ_USUARIOS",
                schema: "RM556834");
        }
    }
}
