using Domain.Entities;
using System.Diagnostics;

namespace Presentation.Doc.Samples
{
    public static class TrocaSamples
    {
        // 📥 Request
        public static readonly Troca TrocaRequest = new Troca
        {
            MentorId = Guid.NewGuid(),
            AlunoId = Guid.NewGuid(),
            HabilidadeId = Guid.NewGuid(),
            SkillName = "Python para iniciantes",
            ScheduledDate = DateTime.UtcNow.AddDays(2),
            DurationHours = 1.5,
            Status = Troca.AGENDADA,
            MeetingLink = "https://meet.google.com/abc-123"
        };

        // 📤 Response
        public static readonly object TrocaResponse = new
        {
            Id = Guid.NewGuid(),
            MentorId = Guid.NewGuid(),
            AlunoId = Guid.NewGuid(),
            SkillName = "Python para iniciantes",
            Status = "AGENDADA",
            ScheduledDate = DateTime.UtcNow.AddDays(2),
            DurationHours = 1.5
        };
    }
}
