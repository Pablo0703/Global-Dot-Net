using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities
{
    [Table("HABILIDADES_GS")]
    public class Habilidade
    {
        // 🔑 Identificação

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // 📝 Nome da habilidade

        [Required]
        [Column("NOME", TypeName = "VARCHAR(150)")]
        public string Nome { get; set; } = string.Empty;

        // 🏷️ Categoria (string + constantes)

        public const string TECNOLOGIA = "TECNOLOGIA";
        public const string DESIGN = "DESIGN";
        public const string NEGOCIOS = "NEGOCIOS";
        public const string IDIOMAS = "IDIOMAS";
        public const string MARKETING = "MARKETING";
        public const string DADOS = "DADOS";
        public const string CRIATIVIDADE = "CRIATIVIDADE";
        public const string SOFT_SKILLS = "SOFT_SKILLS";
        public const string OUTROS = "OUTROS";

        [Required]
        [Column("CATEGORIA", TypeName = "VARCHAR(50)")]
        public string Categoria { get; set; } = OUTROS;

        // 📘 Descrição

        [Column("DESCRICAO", TypeName = "VARCHAR(1000)")]
        public string? Descricao { get; set; }

        // 🎚️ Nível (string + constantes)

        public const string INICIANTE = "INICIANTE";
        public const string INTERMEDIARIO = "INTERMEDIARIO";
        public const string AVANCADO = "AVANCADO";
        public const string EXPERT = "EXPERT";

        [Required]
        [Column("NIVEL", TypeName = "VARCHAR(50)")]
        public string Nivel { get; set; } = INICIANTE;

        // 🎁 Indicadores de oferta/solicitação

        [Required]
        [Column("OFERECE")]
        public bool IsOffering { get; set; } = true;

        [Required]
        [Column("PROCURA")]
        public bool IsSeeking { get; set; } = false;

        // 💰 Valor opcional por hora

        [Column("VALOR_HORA")]
        public double? ValorPorHora { get; set; }

        // 🔗 Usuário dono da habilidade

        [Required]
        [Column("USUARIO_ID")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        // 🕒 Auditoria

        [Column("DATA_CRIACAO")]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}
