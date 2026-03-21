using Lab1_65_Lapko.Repositories;
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

            // Register repositories (DI / IoC)
            builder.Services.AddSingleton<ISubjectRepository, SubjectRepository>();
            builder.Services.AddSingleton<ISessionRepository, SessionRepository>();

            // Register services
            builder.Services.AddSingleton<IAcademicService, AcademicService>();

            // Register pages for DI
            builder.Services.AddTransient<SubjectsPage>();

            return builder.Build();
        }
    }
}
