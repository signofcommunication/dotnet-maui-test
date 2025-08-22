using Microsoft.Extensions.Logging;
using TodoApp_Maui.Services;
using TodoApp_Maui.ViewModels;
using TodoApp_Maui.Views;

namespace TodoApp_Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register Services
        builder.Services.AddSingleton<ITodoService, TodoService>();

        // Register ViewModels
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<AddTodoViewModel>();
        builder.Services.AddTransient<EditTodoViewModel>();

        // Register Views
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<AddTodoPage>();
        builder.Services.AddTransient<EditTodoPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
