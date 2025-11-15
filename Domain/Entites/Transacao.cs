using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities
{
    [Table("TRANSACOES_GS")]
    public class Transacao
    {
        // 🟣 CONSTANTES — Tipo de transação

        public const string PAGAMENTO_SESSAO = "PAGAMENTO_SESSAO";
        public const string AJUSTE = "AJUSTE";
        public const string BONUS_INICIAL = "BONUS_INICIAL";
        public const string BONUS_REFERENCIA = "BONUS_REFERENCIA";

        // 🟣 CONSTANTES — Status da transação

        public const string PENDENTE = "PENDENTE";
        public const string CONCLUIDA = "CONCLUIDA";
        public const string ESTORNADA = "ESTORNADA";

        // 🔑 Identificação

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // 🔗 Sessão/Troca vinculada (opcional)

        [Column("TROCA_ID")]
        public int? TrocaId { get; set; }
        public Troca? Troca { get; set; }

        // 👤 Usuário que envia créditos

        [Required]
        [Column("REMETENTE_ID")]
        public int RemetenteId { get; set; }
        public Usuario Remetente { get; set; } = null!;

        // 👤 Usuário que recebe créditos

        [Required]
        [Column("DESTINATARIO_ID")]
        public int DestinatarioId { get; set; }
        public Usuario Destinatario { get; set; } = null!;

        // 💰 Valor

        [Required]
        [Column("CREDITOS")]
        public double Creditos { get; set; }

        // 🏷️ Tipo de Transação (string + constantes)

        [Required]
        [Column("TIPO", TypeName = "VARCHAR(50)")]
        public string Tipo { get; set; } = PAGAMENTO_SESSAO;

        // 📝 Descrição

        [Column("DESCRICAO", TypeName = "VARCHAR(500)")]
        public string? Descricao { get; set; }

        // ♻️ Status da transação

        [Required]
        [Column("STATUS", TypeName = "VARCHAR(50)")]
        public string Status { get; set; } = PENDENTE;

        // 🕒 Auditoria

        [Column("DATA_CRIACAO")]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}
