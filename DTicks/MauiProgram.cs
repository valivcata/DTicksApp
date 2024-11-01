using CommunityToolkit.Maui;
using DTicks.Services;
using DTicks.ViewModels;
using DTicks.Views;
using Microsoft.Extensions.Logging;
using mvvmdemo.ViewModels;

namespace DTicks
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<DbService>();

            // register our pages
            builder.Services.AddSingleton<TasksPage>();
            builder.Services.AddTransient<NewTaskPage>();
            builder.Services.AddTransient<TaskModifyPage>();
            builder.Services.AddTransient<TaskDetailPage>();
            builder.Services.AddTransient<CompletedTasksPage>();

            // register viewmodels
            builder.Services.AddSingleton<TasksViewModel>();
            builder.Services.AddTransient<NewTaskViewModel>();
            builder.Services.AddTransient<TaskModifyViewModel>();
            builder.Services.AddTransient<TaskDetailViewModel>();
            builder.Services.AddTransient<CompletedTasksViewModel>();

            var app = builder.Build();

            // Initialize our service helper before calling it
            ServiceHelper.Initialize(app.Services);

            return app;
        }
    }
}
