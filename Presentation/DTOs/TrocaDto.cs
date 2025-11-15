using Presentation.Hateoas;

namespace Presentation.DTOs
{
    public class TrocaDTO
    {
        public int Id { get; set; }

        public int MentorId { get; set; }
        public int AlunoId { get; set; }
        public int HabilidadeId { get; set; }

        public string? SkillName { get; set; }
        public DateTime ScheduledDate { get; set; }
        public double DurationHours { get; set; }  // ALTERADO PARA DOUBLE

        public string Status { get; set; } = "";
        public string? MeetingLink { get; set; }
        public string? Notes { get; set; }

        public double? CreditsValue { get; set; }  // AGORA NULLABLE COMO NA ENTITY

        public DateTime CreatedDate { get; set; }

        // 🔗 HATEOAS
        public List<LinkDTO>? Links { get; set; }
    }
}
