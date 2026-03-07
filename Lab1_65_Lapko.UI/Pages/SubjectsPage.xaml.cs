using Lab1_65_Lapko.Core;

namespace Lab1_65_Lapko.UI.Pages
{
    public partial class SubjectsPage : ContentPage
    {
        private readonly IAcademicService _academicService;

        public SubjectsPage(IAcademicService academicService)
        {
            InitializeComponent();
            _academicService = academicService;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SubjectsCollection.ItemsSource = _academicService.GetAllSubjects();
        }

        private async void OnSubjectSelected(object? sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Subject subject)
            {
                SubjectsCollection.SelectedItem = null;
                await Navigation.PushAsync(new SubjectDetailPage(subject));
            }
        }
    }
}
