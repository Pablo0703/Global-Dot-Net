using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("USUARIOS_GS")]
    public class Usuario
    {
        // 🟣 CONSTANTES — Tipos de usuário

        public const string ADMIN = "ADMIN";
        public const string USER = "USER";

        // 🔑 Identificação

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // 📧 E-mail

        [Required]
        [EmailAddress]
        [Column("EMAIL", TypeName = "VARCHAR(100)")]
        public string Email { get; set; } = string.Empty;

        // 👤 Nome completo

        [Required]
        [Column("NOME_COMPLETO", TypeName = "VARCHAR(150)")]
        public string NomeCompleto { get; set; } = string.Empty;

        // 🔐 Senha (hash)

        [Required]
        [Column("SENHA", TypeName = "VARCHAR(255)")]
        public string Password { get; set; } = string.Empty;

        // 🏷️ Papel do usuário

        [Required]
        [Column("ROLE", TypeName = "VARCHAR(50)")]
        public string Role { get; set; } = USER;

        // 📝 Informações adicionais

        [Column("BIO", TypeName = "VARCHAR(500)")]
        public string? Bio { get; set; }

        [Column("AVATAR_URL", TypeName = "VARCHAR(255)")]
        public string? AvatarUrl { get; set; }

        // 💳 Créditos de tempo

        [Required]
        [Column("CREDITOS_TEMPO")]
        public double TimeCredits { get; set; } = 10.0;

        // 📊 Estatísticas

        [Column("TOTAL_SESSOES_DADAS")]
        public int TotalSessionsGiven { get; set; } = 0;

        [Column("TOTAL_SESSOES_RECEBIDAS")]
        public int TotalSessionsTaken { get; set; } = 0;

        [Column("MEDIA_AVALIACOES")]
        public double AverageRating { get; set; } = 0.0;

        // 🌍 Localização

        [Column("LOCALIZACAO", TypeName = "VARCHAR(100)")]
        public string? Location { get; set; }

        [Column("TIMEZONE", TypeName = "VARCHAR(100)")]
        public string? Timezone { get; set; }

        [Column("LINKEDIN_URL", TypeName = "VARCHAR(255)")]
        public string? LinkedinUrl { get; set; }

        // 🕒 Auditoria

        [Column("DATA_CRIACAO")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("DATA_ATUALIZACAO")]
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        // 🔗 Relacionamentos

        public ICollection<Habilidade>? Habilidades { get; set; }
        public ICollection<Troca>? TrocasComoMentor { get; set; }
        public ICollection<Troca>? TrocasComoAluno { get; set; }
        public ICollection<Avaliacao>? AvaliacoesFeitas { get; set; }
        public ICollection<Avaliacao>? AvaliacoesRecebidas { get; set; }
    }
}
