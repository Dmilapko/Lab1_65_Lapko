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

            // Storage
            var dataPath = Path.Combine(FileSystem.Current.AppDataDirectory, "academic.json");
            builder.Services.AddSingleton(sp => new JsonDataStore(dataPath));

            // Repositories
            builder.Services.AddSingleton<ISubjectRepository, SubjectRepository>();
            builder.Services.AddSingleton<ISessionRepository, SessionRepository>();

            // Services
            builder.Services.AddSingleton<IAcademicService, AcademicService>();

            // Pages
            builder.Services.AddTransient<SubjectsPage>();

            return builder.Build();
        }
    }
}
