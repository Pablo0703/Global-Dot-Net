using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities
{
    [Table("AVALIACOES_GS")]
    public class Avaliacao
    {
        // 🔑 Identificação

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // 🔗 Troca associada

        [Required]
        [Column("TROCA_ID")]
        public int TrocaId { get; set; }
        public Troca Troca { get; set; } = null!;

        // 👤 Usuário que fez a avaliação

        [Required]
        [Column("AVALIADOR_ID")]
        public int AvaliadorId { get; set; }
        public Usuario Avaliador { get; set; } = null!;

        // 👤 Usuário avaliado

        [Required]
        [Column("AVALIADO_ID")]
        public int AvaliadoId { get; set; }
        public Usuario Avaliado { get; set; } = null!;

        // ⭐ Nota (1 a 5)

        [Required]
        [Range(1, 5)]
        [Column("NOTA")]
        public int Nota { get; set; }

        // 📝 Comentário

        [Column("COMENTARIO", TypeName = "VARCHAR(2000)")]
        public string? Comentario { get; set; }

        // 🕒 Auditoria

        [Column("DATA_CRIACAO")]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}
