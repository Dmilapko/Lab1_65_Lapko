using Lab1_65_Lapko.UI.ViewModels;
using Lab1_65_Lapko.Services;

namespace Lab1_65_Lapko.UI.Pages
{
    public partial class SubjectDetailPage : ContentPage
    {
        public SubjectDetailPage(IAcademicService academicService, Guid subjectId)
        {
            InitializeComponent();
            BindingContext = new SubjectDetailViewModel(academicService, Navigation, subjectId);
        }
    }
}
