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
    }
}
