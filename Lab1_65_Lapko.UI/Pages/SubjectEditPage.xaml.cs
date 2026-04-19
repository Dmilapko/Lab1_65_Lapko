using Lab1_65_Lapko.Services;
using Lab1_65_Lapko.UI.ViewModels;

namespace Lab1_65_Lapko.UI.Pages
{
    public partial class SubjectEditPage : ContentPage
    {
        private readonly SubjectEditViewModel _viewModel;

        public SubjectEditPage(IAcademicService academicService, Guid? existingId)
        {
            InitializeComponent();
            _viewModel = new SubjectEditViewModel(academicService, Navigation, existingId);
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadAsync();
        }
    }
}
