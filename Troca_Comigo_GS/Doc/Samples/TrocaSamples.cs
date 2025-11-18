using Domain.Entities;

namespace Presentation.Doc.Samples
{
    public static class TrocaSamples
    {
        // 📥 REQUEST SAMPLE
        public static readonly Troca TrocaRequest = new Troca
        {
            MentorId = 1,
            AlunoId = 2,
            HabilidadeId = 10,
            SkillName = "Python para iniciantes",
            ScheduledDate = DateTime.UtcNow.AddDays(2),
            DurationHours = 1.5,
            Status = "AGENDADA",
            MeetingLink = "https://meet.google.com/abc-123"
        };

        // 📤 RESPONSE SAMPLE
        public static readonly object TrocaResponse = new
        {
            Id = 100,
            MentorId = 1,
            AlunoId = 2,
            HabilidadeId = 10,
            SkillName = "Python para iniciantes",
            ScheduledDate = DateTime.UtcNow.AddDays(2),
            DurationHours = 1.5,
            Status = "AGENDADA",
            MeetingLink = "https://meet.google.com/abc-123"
        };
    }
}
