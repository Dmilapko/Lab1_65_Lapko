using System.Collections.ObjectModel;
using Lab1_65_Lapko.Services;
using Lab1_65_Lapko.Services.DTOs;

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

        public SubjectDetailViewModel(IAcademicService academicService, INavigation navigation, Guid subjectId)
        {
            _academicService = academicService;
            _navigation = navigation;
            _subjectId = subjectId;
        }

        public async Task LoadAsync()
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

        private async void NavigateToDetail(SessionListDto session)
        {
            var detailPage = new Pages.SessionDetailPage(_academicService, session.Id);
            await _navigation.PushAsync(detailPage);
        }
    }
}
