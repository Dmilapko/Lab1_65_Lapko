using Lab1_65_Lapko.Models;

namespace Lab1_65_Lapko.UI.Pages
{
    public partial class SubjectDetailPage : ContentPage
    {
        public SubjectDetailPage(Subject subject)
        {
            InitializeComponent();
            BindingContext = subject;
            SessionsCollection.ItemsSource = subject.Sessions;
        }

        private async void OnSessionSelected(object? sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Session session)
            {
                SessionsCollection.SelectedItem = null;
                await Navigation.PushAsync(new SessionDetailPage(session));
            }
        }
    }
}
