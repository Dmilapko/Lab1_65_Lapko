using Lab1_65_Lapko.UI.ViewModels;
using Lab1_65_Lapko.Services;

namespace Lab1_65_Lapko.UI.Pages
{
    public partial class SessionDetailPage : ContentPage
    {
        public SessionDetailPage(IAcademicService academicService, Guid sessionId)
        {
            InitializeComponent();
            BindingContext = new SessionDetailViewModel(academicService, sessionId);
        }
    }
}
