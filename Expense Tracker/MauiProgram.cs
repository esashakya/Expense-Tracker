using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using Expense_Tracker.Services;

namespace Expense_Tracker
{
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
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IInflowService, InflowService>();
            builder.Services.AddSingleton<IOutflowService, OutflowService>();
            builder.Services.AddSingleton<IDebtService, DebtService>();
            builder.Services.AddSingleton<AuthenticationStateService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
