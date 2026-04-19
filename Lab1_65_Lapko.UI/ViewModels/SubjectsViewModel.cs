using System.Collections.ObjectModel;
using System.Windows.Input;
using Lab1_65_Lapko.Services;
using Lab1_65_Lapko.Services.DTOs;
using Lab1_65_Lapko.UI.Helpers;

namespace Lab1_65_Lapko.UI.ViewModels
{
    public class SubjectsViewModel : BaseViewModel
    {
        private readonly IAcademicService _academicService;
        private readonly INavigation _navigation;
        private SubjectListDto? _selectedSubject;

        public ObservableCollection<SubjectListDto> Subjects { get; } = new();

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
                Subjects.Clear();
                var items = await _academicService.GetAllSubjectsAsync();
                foreach (var subject in items)
                {
                    Subjects.Add(subject);
                }
            }
            finally
            {
                IsBusy = false;
            }
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
