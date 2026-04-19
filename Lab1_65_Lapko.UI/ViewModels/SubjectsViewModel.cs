using System.Collections.ObjectModel;
using System.Windows.Input;
using Lab1_65_Lapko.Services;
using Lab1_65_Lapko.Services.DTOs;
using Lab1_65_Lapko.UI.Helpers;

namespace Lab1_65_Lapko.UI.ViewModels
{
    public class SubjectsViewModel : BaseViewModel
    {
        private static readonly string[] SortOptionsList =
        {
            "Name (A-Z)", "Name (Z-A)",
            "ECTS (Low-High)", "ECTS (High-Low)",
            "Area (A-Z)", "Area (Z-A)"
        };

        private readonly IAcademicService _academicService;
        private readonly INavigation _navigation;

        private List<SubjectListDto> _allSubjects = new();
        private SubjectListDto? _selectedSubject;
        private string _searchText = string.Empty;
        private string _sortOption = SortOptionsList[0];

        public ObservableCollection<SubjectListDto> Subjects { get; } = new();
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

        public SubjectListDto? SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                if (SetProperty(ref _selectedSubject, value) && value != null)
                {
                    NavigateToDetail(value);
                    SelectedSubject = null;
                }
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public SubjectsViewModel(IAcademicService academicService, INavigation navigation)
        {
            _academicService = academicService;
            _navigation = navigation;

            AddCommand = new AsyncRelayCommand(AddAsync);
            EditCommand = new AsyncRelayCommand<SubjectListDto>(EditAsync);
            DeleteCommand = new AsyncRelayCommand<SubjectListDto>(DeleteAsync);
        }

        public async Task LoadAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                _allSubjects = await _academicService.GetAllSubjectsAsync();
                ApplyFilterAndSort();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ApplyFilterAndSort()
        {
            IEnumerable<SubjectListDto> query = _allSubjects;

            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                var term = _searchText.Trim();
                query = query.Where(s =>
                    s.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    s.Area.Contains(term, StringComparison.OrdinalIgnoreCase));
            }

            query = _sortOption switch
            {
                "Name (A-Z)" => query.OrderBy(s => s.Name),
                "Name (Z-A)" => query.OrderByDescending(s => s.Name),
                "ECTS (Low-High)" => query.OrderBy(s => s.EctsCredits),
                "ECTS (High-Low)" => query.OrderByDescending(s => s.EctsCredits),
                "Area (A-Z)" => query.OrderBy(s => s.Area),
                "Area (Z-A)" => query.OrderByDescending(s => s.Area),
                _ => query
            };

            Subjects.Clear();
            foreach (var s in query)
                Subjects.Add(s);
        }

        private Task AddAsync()
        {
            var page = new Pages.SubjectEditPage(_academicService, null);
            return _navigation.PushAsync(page);
        }

        private Task EditAsync(SubjectListDto? subject)
        {
            if (subject == null) return Task.CompletedTask;
            var page = new Pages.SubjectEditPage(_academicService, subject.Id);
            return _navigation.PushAsync(page);
        }

        private async Task DeleteAsync(SubjectListDto? subject)
        {
            if (subject == null) return;

            var page = Application.Current?.Windows[0]?.Page;
            if (page == null) return;

            var confirm = await page.DisplayAlert(
                "Delete Subject",
                $"Delete \"{subject.Name}\" and all its sessions?",
                "Delete", "Cancel");
            if (!confirm) return;

            if (IsBusy) return;
            IsBusy = true;
            try
            {
                await _academicService.DeleteSubjectAsync(subject.Id);
                _allSubjects.RemoveAll(s => s.Id == subject.Id);
                Subjects.Remove(subject);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void NavigateToDetail(SubjectListDto subject)
        {
            var detailPage = new Pages.SubjectDetailPage(_academicService, subject.Id);
            await _navigation.PushAsync(detailPage);
        }
    }
}
