using System.Collections.ObjectModel;
using System.Windows.Input;
using Lab1_65_Lapko.Services;
using Lab1_65_Lapko.Services.DTOs;
using Lab1_65_Lapko.UI.Helpers;

namespace Lab1_65_Lapko.UI.ViewModels
{
    public class SubjectDetailViewModel : BaseViewModel
    {
        private static readonly string[] SortOptionsList =
        {
            "Topic (A-Z)", "Topic (Z-A)",
            "Type (A-Z)", "Type (Z-A)",
            "Date (Earliest)", "Date (Latest)"
        };

        private readonly IAcademicService _academicService;
        private readonly INavigation _navigation;
        private readonly Guid _subjectId;

        private Guid _id;
        private string _name = string.Empty;
        private int _ectsCredits;
        private string _area = string.Empty;
        private TimeSpan _totalDuration;
        private SessionListDto? _selectedSession;

        private List<SessionListDto> _allSessions = new();
        private string _searchText = string.Empty;
        private string _sortOption = SortOptionsList[4];

        public Guid Id { get => _id; private set => SetProperty(ref _id, value); }
        public string Name { get => _name; private set => SetProperty(ref _name, value); }
        public int EctsCredits { get => _ectsCredits; private set => SetProperty(ref _ectsCredits, value); }
        public string Area { get => _area; private set => SetProperty(ref _area, value); }
        public TimeSpan TotalDuration { get => _totalDuration; private set => SetProperty(ref _totalDuration, value); }

        public ObservableCollection<SessionListDto> Sessions { get; } = new();
        public ObservableCollection<string> SortOptions { get; } = new(SortOptionsList);

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                    ApplyFilterAndSort();
            }
        }

        public string SortOption
        {
            get => _sortOption;
            set
            {
                if (SetProperty(ref _sortOption, value))
                    ApplyFilterAndSort();
            }
        }

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

                _allSessions = detail.Sessions.ToList();
                ApplyFilterAndSort();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ApplyFilterAndSort()
        {
            IEnumerable<SessionListDto> query = _allSessions;

            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                var term = _searchText.Trim();
                query = query.Where(s =>
                    s.Topic.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    s.Type.Contains(term, StringComparison.OrdinalIgnoreCase));
            }

            query = _sortOption switch
            {
                "Topic (A-Z)" => query.OrderBy(s => s.Topic),
                "Topic (Z-A)" => query.OrderByDescending(s => s.Topic),
                "Type (A-Z)" => query.OrderBy(s => s.Type),
                "Type (Z-A)" => query.OrderByDescending(s => s.Type),
                "Date (Earliest)" => query.OrderBy(s => s.Date).ThenBy(s => s.StartTime),
                "Date (Latest)" => query.OrderByDescending(s => s.Date).ThenByDescending(s => s.StartTime),
                _ => query
            };

            Sessions.Clear();
            foreach (var s in query)
                Sessions.Add(s);
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
                _allSessions.RemoveAll(s => s.Id == session.Id);
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
