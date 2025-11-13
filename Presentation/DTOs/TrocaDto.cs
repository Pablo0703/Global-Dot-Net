public class TrocaDTO
{
    public Guid Id { get; set; }

    public required Guid MentorId { get; set; }

    public required Guid AlunoId { get; set; }

    public required Guid HabilidadeId { get; set; }

    public string? SkillName { get; set; }

    public required DateTime ScheduledDate { get; set; }

    public required double DurationHours { get; set; }

    public required string Status { get; set; }

    public string? MeetingLink { get; set; }

    public string? Notes { get; set; }

    public double? CreditsValue { get; set; }

    public DateTime CreatedDate { get; set; }
}
