using System.Collections.ObjectModel;
using System.Windows.Input;
using Lab1_65_Lapko.Services;
using Lab1_65_Lapko.Services.DTOs;
using Lab1_65_Lapko.UI.Helpers;

namespace Lab1_65_Lapko.UI.ViewModels
{
    public class SubjectDetailViewModel : BaseViewModel
    {
        private readonly IAcademicService _academicService;
        private readonly INavigation _navigation;
        private readonly Guid _subjectId;

        private Guid _id;
        private string _name = string.Empty;
        private int _ectsCredits;
        private string _area = string.Empty;
        private TimeSpan _totalDuration;
        private SessionListDto? _selectedSession;

        public Guid Id { get => _id; private set => SetProperty(ref _id, value); }
        public string Name { get => _name; private set => SetProperty(ref _name, value); }
        public int EctsCredits { get => _ectsCredits; private set => SetProperty(ref _ectsCredits, value); }
        public string Area { get => _area; private set => SetProperty(ref _area, value); }
        public TimeSpan TotalDuration { get => _totalDuration; private set => SetProperty(ref _totalDuration, value); }

        public ObservableCollection<SessionListDto> Sessions { get; } = new();

        public SessionListDto? SelectedSession
        {
            get => _selectedSession;
            set
            {
                if (SetProperty(ref _selectedSession, value) && value != null)
                {
                    NavigateToDetail(value);
                    SelectedSession = null;
                }
            }
        }

        public ICommand AddSessionCommand { get; }
        public ICommand EditSessionCommand { get; }
        public ICommand DeleteSessionCommand { get; }

        public SubjectDetailViewModel(IAcademicService academicService, INavigation navigation, Guid subjectId)
        {
            _academicService = academicService;
            _navigation = navigation;
            _subjectId = subjectId;

            AddSessionCommand = new AsyncRelayCommand(AddSessionAsync);
            EditSessionCommand = new AsyncRelayCommand<SessionListDto>(EditSessionAsync);
            DeleteSessionCommand = new AsyncRelayCommand<SessionListDto>(DeleteSessionAsync);
        }

        public async Task LoadAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var detail = await _academicService.GetSubjectDetailAsync(_subjectId);
                if (detail == null) return;

                Id = detail.Id;
                Name = detail.Name;
                EctsCredits = detail.EctsCredits;
                Area = detail.Area;
                TotalDuration = detail.TotalDuration;

                Sessions.Clear();
                foreach (var session in detail.Sessions)
                {
                    Sessions.Add(session);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Task AddSessionAsync()
        {
            var page = new Pages.SessionEditPage(_academicService, _subjectId, null);
            return _navigation.PushAsync(page);
        }

        private Task EditSessionAsync(SessionListDto? session)
        {
            if (session == null) return Task.CompletedTask;
            var page = new Pages.SessionEditPage(_academicService, _subjectId, session.Id);
            return _navigation.PushAsync(page);
        }

        private async Task DeleteSessionAsync(SessionListDto? session)
        {
            if (session == null) return;

            var page = Application.Current?.Windows[0]?.Page;
            if (page == null) return;

            var confirm = await page.DisplayAlert(
                "Delete Session",
                $"Delete session \"{session.Topic}\"?",
                "Delete", "Cancel");
            if (!confirm) return;

            if (IsBusy) return;
            IsBusy = true;
            try
            {
                await _academicService.DeleteSessionAsync(session.Id);
                Sessions.Remove(session);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void NavigateToDetail(SessionListDto session)
        {
            var detailPage = new Pages.SessionDetailPage(_academicService, session.Id);
            await _navigation.PushAsync(detailPage);
        }
    }
}
