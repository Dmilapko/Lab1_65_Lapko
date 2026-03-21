using System.Collections.ObjectModel;
using Lab1_65_Lapko.Services;
using Lab1_65_Lapko.Services.DTOs;

namespace Lab1_65_Lapko.UI.ViewModels
{
    public class SubjectDetailViewModel : BaseViewModel
    {
        private readonly IAcademicService _academicService;
        private readonly INavigation _navigation;
        private SessionListDto? _selectedSession;

        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public int EctsCredits { get; private set; }
        public string Area { get; private set; } = string.Empty;
        public TimeSpan TotalDuration { get; private set; }
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
            LoadSubject(subjectId);
        }

        private void LoadSubject(Guid subjectId)
        {
            var detail = _academicService.GetSubjectDetail(subjectId);
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
