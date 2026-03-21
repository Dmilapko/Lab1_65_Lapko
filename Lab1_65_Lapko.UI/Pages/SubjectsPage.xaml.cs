using Lab1_65_Lapko.UI.ViewModels;
using Lab1_65_Lapko.Services;

namespace Lab1_65_Lapko.UI.Pages
{
    public partial class SubjectsPage : ContentPage
    {
        public SubjectsPage(IAcademicService academicService)
        {
            InitializeComponent();
            BindingContext = new SubjectsViewModel(academicService, Navigation);
        }
    }
}
