using System.Collections.ObjectModel;
using System.Windows.Input;
using Lab1_65_Lapko.Services;
using Lab1_65_Lapko.Services.DTOs;
using Lab1_65_Lapko.UI.Helpers;

namespace Lab1_65_Lapko.UI.ViewModels
{
    public class SessionEditViewModel : BaseViewModel
    {
        private static readonly string[] SessionTypes =
            { "Lecture", "Seminar", "Laboratory", "Exam" };

        private readonly IAcademicService _academicService;
        private readonly INavigation _navigation;
        private readonly Guid _subjectId;
        private readonly Guid? _existingId;

        private string _topic = string.Empty;
        private string _type = SessionTypes[0];
        private DateTime _date = DateTime.Today;
        private TimeSpan _startTime = new(10, 0, 0);
        private TimeSpan _endTime = new(11, 30, 0);

        public string Title => _existingId.HasValue ? "Edit Session" : "New Session";

        public string Topic { get => _topic; set => SetProperty(ref _topic, value); }
        public string Type { get => _type; set => SetProperty(ref _type, value); }
        public DateTime Date { get => _date; set => SetProperty(ref _date, value); }
        public TimeSpan StartTime { get => _startTime; set => SetProperty(ref _startTime, value); }
        public TimeSpan EndTime { get => _endTime; set => SetProperty(ref _endTime, value); }

        public ObservableCollection<string> Types { get; } = new(SessionTypes);

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public SessionEditViewModel(
            IAcademicService academicService,
            INavigation navigation,
            Guid subjectId,
            Guid? existingId)
        {
            _academicService = academicService;
            _navigation = navigation;
            _subjectId = subjectId;
            _existingId = existingId;

            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
            CancelCommand = new AsyncRelayCommand(() => _navigation.PopAsync());

            PropertyChanged += (_, e) =>
            {
                if (e.PropertyName is nameof(Topic) or nameof(StartTime) or nameof(EndTime))
                    ((AsyncRelayCommand)SaveCommand).RaiseCanExecuteChanged();
            };
        }

        public async Task LoadAsync()
        {
            if (!_existingId.HasValue) return;
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var detail = await _academicService.GetSessionDetailAsync(_existingId.Value);
                if (detail == null) return;

                Topic = detail.Topic;
                Type = detail.Type;
                Date = detail.Date.ToDateTime(TimeOnly.MinValue);
                StartTime = detail.StartTime.ToTimeSpan();
                EndTime = detail.EndTime.ToTimeSpan();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanSave() => !string.IsNullOrWhiteSpace(Topic) && EndTime > StartTime;

        private async Task SaveAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var input = new SessionInputDto
                {
                    SubjectId = _subjectId,
                    Topic = Topic.Trim(),
                    Type = Type,
                    Date = DateOnly.FromDateTime(Date),
                    StartTime = TimeOnly.FromTimeSpan(StartTime),
                    EndTime = TimeOnly.FromTimeSpan(EndTime)
                };

                if (_existingId.HasValue)
                {
                    await _academicService.UpdateSessionAsync(_existingId.Value, input);
                }
                else
                {
                    await _academicService.AddSessionAsync(input);
                }

                await _navigation.PopAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
