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

// ����������� ��������� ����������
builder.RootComponents.Add<App>("#app");

// ��������� HTTP-������� ��� ������� � zxcAPI
builder.Services.AddHttpClient("zxcAPI", client =>
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// ����������� HTTP-������� ��� �������
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("zxcAPI"));

// ��������� �������������� ����� JWT
builder.Services.AddApiAuthorization();

// ����������� ���������������� ��������
builder.Services.AddScoped<RestaurantService>();
builder.Services.AddScoped<NotificationService>();

// ��������� ����� �� ������
if (builder.HostEnvironment.IsDevelopment())
{
    // � ������ ���������� ��������� ���-������
    var webAppBuilder = WebApplication.CreateBuilder(args);
    webAppBuilder.Services.AddDirectoryBrowser();
    var webApp = webAppBuilder.Build();

    webApp.UseFileServer(new FileServerOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
        RequestPath = "",
        EnableDirectoryBrowsing = false
    });

    // ��������� ������ �� localhost:5015
    webApp.Urls.Add("http://localhost:5015");
    await webApp.StartAsync();

    Console.WriteLine("Blazor WebAssembly ������� �� http://localhost:5000");
    // ��������� ������� �������������
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
        Console.WriteLine($"�� ������� ������� �������: {ex.Message}");
    }
}

var app = builder.Build();
await app.RunAsync();