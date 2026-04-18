using Lab1_65_Lapko.Services;
using Lab1_65_Lapko.UI.ViewModels;

namespace Lab1_65_Lapko.UI.Pages
{
    public partial class SessionDetailPage : ContentPage
    {
        private readonly SessionDetailViewModel _viewModel;

        public SessionDetailPage(IAcademicService academicService, Guid sessionId)
        {
            InitializeComponent();
            _viewModel = new SessionDetailViewModel(academicService, sessionId);
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadAsync();
        }
    }
}
