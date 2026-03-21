using System.Collections.ObjectModel;
using System.Windows.Input;
using Lab1_65_Lapko.Services;
using Lab1_65_Lapko.Services.DTOs;

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

        public SubjectsViewModel(IAcademicService academicService, INavigation navigation)
        {
            _academicService = academicService;
            _navigation = navigation;
            LoadSubjects();
        }

        public void LoadSubjects()
        {
            Subjects.Clear();
            foreach (var subject in _academicService.GetAllSubjects())
            {
                Subjects.Add(subject);
            }
        }

        private async void NavigateToDetail(SubjectListDto subject)
        {
            var detailPage = new Pages.SubjectDetailPage(_academicService, subject.Id);
            await _navigation.PushAsync(detailPage);
        }
    }
}
