using System.Collections.ObjectModel;
using System.Windows.Input;
using Lab1_65_Lapko.Services;
using Lab1_65_Lapko.Services.DTOs;
using Lab1_65_Lapko.UI.Helpers;

namespace Lab1_65_Lapko.UI.ViewModels
{
    public class SubjectEditViewModel : BaseViewModel
    {
        private static readonly string[] KnowledgeAreas =
            { "Engineering", "Mathematics", "Programming", "Humanities" };

        private readonly IAcademicService _academicService;
        private readonly INavigation _navigation;
        private readonly Guid? _existingId;

        private string _name = string.Empty;
        private int _ectsCredits;
        private string _area = KnowledgeAreas[0];

        public string Title => _existingId.HasValue ? "Edit Subject" : "New Subject";

        public string Name { get => _name; set => SetProperty(ref _name, value); }
        public int EctsCredits { get => _ectsCredits; set => SetProperty(ref _ectsCredits, value); }
        public string Area { get => _area; set => SetProperty(ref _area, value); }

        public ObservableCollection<string> Areas { get; } = new(KnowledgeAreas);

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public SubjectEditViewModel(IAcademicService academicService, INavigation navigation, Guid? existingId)
        {
            _academicService = academicService;
            _navigation = navigation;
            _existingId = existingId;

            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
            CancelCommand = new AsyncRelayCommand(() => _navigation.PopAsync());

            PropertyChanged += (_, e) =>
            {
                if (e.PropertyName is nameof(Name) or nameof(EctsCredits))
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
                var detail = await _academicService.GetSubjectDetailAsync(_existingId.Value);
                if (detail == null) return;

                Name = detail.Name;
                EctsCredits = detail.EctsCredits;
                Area = detail.Area;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanSave() => !string.IsNullOrWhiteSpace(Name) && EctsCredits > 0;

        private async Task SaveAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var input = new SubjectInputDto
                {
                    Name = Name.Trim(),
                    EctsCredits = EctsCredits,
                    Area = Area
                };

                if (_existingId.HasValue)
                {
                    await _academicService.UpdateSubjectAsync(_existingId.Value, input);
                }
                else
                {
                    await _academicService.AddSubjectAsync(input);
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
