namespace Presentation.DTOs;

public class TrocaDTO
{
    public int Id { get; set; }

    public required int MentorId { get; set; }

    public required int AlunoId { get; set; }

    public required int HabilidadeId { get; set; }

    public string? SkillName { get; set; }

    public required DateTime ScheduledDate { get; set; }

    public required double DurationHours { get; set; }

    public required string Status { get; set; }

    public string? MeetingLink { get; set; }

    public string? Notes { get; set; }

    public double? CreditsValue { get; set; }

    public DateTime CreatedDate { get; set; }
}
