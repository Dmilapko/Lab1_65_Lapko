using Lab1_65_Lapko.Services;
using Lab1_65_Lapko.UI.Pages;

namespace Lab1_65_Lapko.UI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>();

            // Register services (DI / IoC)
            builder.Services.AddSingleton<IAcademicService, AcademicService>();

            // Register pages for DI
            builder.Services.AddTransient<SubjectsPage>();
            builder.Services.AddTransient<SubjectDetailPage>();
            builder.Services.AddTransient<SessionDetailPage>();

            return builder.Build();
        }
    }
}
