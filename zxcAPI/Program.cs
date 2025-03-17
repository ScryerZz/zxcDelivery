using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using zxcAPI.Data;
using zxcAPI.Hubs;
using zxcAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// ��������� ����������� � ���� ������ (��������� ������ ����� SSMS)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ��������� �������������� � �������������� JWT
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("JWT Key is missing in configuration.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// ��������� SignalR ��� real-time �����������
builder.Services.AddSignalR();

// ����������� ������������ ��� REST API
builder.Services.AddControllers();

// ��������� Swagger ��� ������������ API
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "zxcAPI", Version = "v1" });
});

// ����������� ���������������� ��������
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IRestaurantService, RestaurantService>();

var app = builder.Build();

// ������������ middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "zxcAPI v1"));
}

// �������� pipeline ��������� ��������
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ��������� �������� �����
app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");

app.Run();