using Lab1_65_Lapko.Services;

namespace Lab1_65_Lapko.UI.ViewModels
{
    public class SessionDetailViewModel : BaseViewModel
    {
        public Guid Id { get; private set; }
        public string Topic { get; private set; } = string.Empty;
        public string Type { get; private set; } = string.Empty;
        public DateOnly Date { get; private set; }
        public TimeOnly StartTime { get; private set; }
        public TimeOnly EndTime { get; private set; }
        public TimeSpan Duration { get; private set; }

        public SessionDetailViewModel(IAcademicService academicService, Guid sessionId)
        {
            var detail = academicService.GetSessionDetail(sessionId);
            if (detail == null) return;

            Id = detail.Id;
            Topic = detail.Topic;
            Type = detail.Type;
            Date = detail.Date;
            StartTime = detail.StartTime;
            EndTime = detail.EndTime;
            Duration = detail.Duration;
        }
    }
}
