using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities
{
    [Table("TROCAS_GS")]
    public class Troca
    {
        // 🟣 CONSTANTES (Status da troca)
        public const string AGENDADA = "AGENDADA";
        public const string CONFIRMADA = "CONFIRMADA";
        public const string EM_ANDAMENTO = "EM_ANDAMENTO";
        public const string CONCLUIDA = "CONCLUIDA";
        public const string CANCELADA = "CANCELADA";

        // 🔑 Identificação

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // 🧑‍🏫 Mentor (quem ensina)

        [Required]
        [Column("MENTOR_ID")]
        public  int MentorId { get; set; }

        public Usuario Mentor { get; set; } = null!;

        // 👨‍🎓 Aluno (quem recebe)

        [Required]
        [Column("ALUNO_ID")]
        public int AlunoId { get; set; }

        public Usuario Aluno { get; set; } = null!;

        // 🎓 Habilidade relacionada

        [Required]
        [Column("HABILIDADE_ID")]
        public int HabilidadeId { get; set; }

        public Habilidade Habilidade { get; set; } = null!;

        [Column("NOME_HABILIDADE", TypeName = "VARCHAR(150)")]
        public string? SkillName { get; set; }

        // 🗓️ Agendamento

        [Required]
        [Column("DATA_AGENDADA")]
        public DateTime ScheduledDate { get; set; }

        [Required]
        [Column("DURACAO_HORAS")]
        public double DurationHours { get; set; } = 1.0;

        // ♻️ STATUS (string com constantes)

        [Required]
        [Column("STATUS_TROCA", TypeName = "VARCHAR(50)")]
        public string Status { get; set; } = AGENDADA;

        // 📞 Informações da reunião

        [Column("MEETING_LINK", TypeName = "VARCHAR(500)")]
        public string? MeetingLink { get; set; }

        [Column("NOTAS", TypeName = "VARCHAR(2000)")]
        public string? Notes { get; set; }


        // 💰 Créditos

        [Column("CREDITOS")]
        public double? CreditsValue { get; set; }

        // 🕒 Auditoria

        [Column("DATA_CRIACAO")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // 🔗 Relacionamentos reversos

        public ICollection<Transacao>? Transacoes { get; set; }
        public ICollection<Avaliacao>? Avaliacoes { get; set; }
    }
}
