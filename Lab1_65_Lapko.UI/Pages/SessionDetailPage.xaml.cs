using Lab1_65_Lapko.Models;

namespace Lab1_65_Lapko.UI.Pages
{
    public partial class SessionDetailPage : ContentPage
    {
        public SessionDetailPage(Session session)
        {
            InitializeComponent();
            BindingContext = session;
        }
    }
}
