using Lab1_65_Lapko.UI.Pages;

namespace Lab1_65_Lapko.UI
{
    public partial class App : Application
    {
        private readonly SubjectsPage _subjectsPage;

        public App(SubjectsPage subjectsPage)
        {
            InitializeComponent();
            _subjectsPage = subjectsPage;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new NavigationPage(_subjectsPage));
        }
    }
}
