using Lab1_65_Lapko.Services;
using Lab1_65_Lapko.UI.ViewModels;

namespace Lab1_65_Lapko.UI.Pages
{
    public partial class SubjectsPage : ContentPage
    {
        private readonly SubjectsViewModel _viewModel;

        public SubjectsPage(IAcademicService academicService)
        {
            InitializeComponent();
            _viewModel = new SubjectsViewModel(academicService, Navigation);
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadAsync();
        }
    }
}
