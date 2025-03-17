using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.FileProviders;
using zxcBLAZOR.Services;
using zxcBLAZOR.Components;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using zxcBLAZOR;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Регистрация корневого компонента
builder.RootComponents.Add<App>("#app");

// Настройка HTTP-клиента для общения с zxcAPI
builder.Services.AddHttpClient("zxcAPI", client =>
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Подключение HTTP-клиента как сервиса
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("zxcAPI"));

// Поддержка аутентификации через JWT
builder.Services.AddApiAuthorization();

// Регистрация пользовательских сервисов
builder.Services.AddScoped<RestaurantService>();
builder.Services.AddScoped<NotificationService>();

// Проверяем среду до сборки
if (builder.HostEnvironment.IsDevelopment())
{
    // В режиме разработки запускаем веб-сервер
    var webAppBuilder = WebApplication.CreateBuilder(args);
    webAppBuilder.Services.AddDirectoryBrowser();
    var webApp = webAppBuilder.Build();

    webApp.UseFileServer(new FileServerOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
        RequestPath = "",
        EnableDirectoryBrowsing = false
    });

    // Запускаем сервер на localhost:5015
    webApp.Urls.Add("http://localhost:5015");
    await webApp.StartAsync();

    Console.WriteLine("Blazor WebAssembly запущен на http://localhost:5000");
    // Открываем браузер автоматически
    try
    {
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = "http://localhost:5000",
            UseShellExecute = true
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Не удалось открыть браузер: {ex.Message}");
    }
}

var app = builder.Build();
await app.RunAsync();