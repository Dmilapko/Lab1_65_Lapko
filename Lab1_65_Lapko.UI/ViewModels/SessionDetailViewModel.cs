using Lab1_65_Lapko.Services;

namespace Lab1_65_Lapko.UI.ViewModels
{
    public class SessionDetailViewModel : BaseViewModel
    {
        private readonly IAcademicService _academicService;
        private readonly Guid _sessionId;

        private Guid _id;
        private string _topic = string.Empty;
        private string _type = string.Empty;
        private DateOnly _date;
        private TimeOnly _startTime;
        private TimeOnly _endTime;
        private TimeSpan _duration;

        public Guid Id { get => _id; private set => SetProperty(ref _id, value); }
        public string Topic { get => _topic; private set => SetProperty(ref _topic, value); }
        public string Type { get => _type; private set => SetProperty(ref _type, value); }
        public DateOnly Date { get => _date; private set => SetProperty(ref _date, value); }
        public TimeOnly StartTime { get => _startTime; private set => SetProperty(ref _startTime, value); }
        public TimeOnly EndTime { get => _endTime; private set => SetProperty(ref _endTime, value); }
        public TimeSpan Duration { get => _duration; private set => SetProperty(ref _duration, value); }

        public SessionDetailViewModel(IAcademicService academicService, Guid sessionId)
        {
            _academicService = academicService;
            _sessionId = sessionId;
        }

        public async Task LoadAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var detail = await _academicService.GetSessionDetailAsync(_sessionId);
                if (detail == null) return;

                Id = detail.Id;
                Topic = detail.Topic;
                Type = detail.Type;
                Date = detail.Date;
                StartTime = detail.StartTime;
                EndTime = detail.EndTime;
                Duration = detail.Duration;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
