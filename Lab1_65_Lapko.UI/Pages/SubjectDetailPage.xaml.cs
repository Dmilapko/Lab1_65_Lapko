using Lab1_65_Lapko.Services;
using Lab1_65_Lapko.UI.ViewModels;

namespace Lab1_65_Lapko.UI.Pages
{
    public partial class SubjectDetailPage : ContentPage
    {
        private readonly SubjectDetailViewModel _viewModel;

        public SubjectDetailPage(IAcademicService academicService, Guid subjectId)
        {
            InitializeComponent();
            _viewModel = new SubjectDetailViewModel(academicService, Navigation, subjectId);
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadAsync();
        }
    }
}
