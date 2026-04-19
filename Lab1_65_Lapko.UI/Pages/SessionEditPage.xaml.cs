using Lab1_65_Lapko.Services;
using Lab1_65_Lapko.UI.ViewModels;

namespace Lab1_65_Lapko.UI.Pages
{
    public partial class SessionEditPage : ContentPage
    {
        private readonly SessionEditViewModel _viewModel;

        public SessionEditPage(IAcademicService academicService, Guid subjectId, Guid? existingId)
        {
            InitializeComponent();
            _viewModel = new SessionEditViewModel(academicService, Navigation, subjectId, existingId);
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadAsync();
        }
    }
}
