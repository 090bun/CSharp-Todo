using Microsoft.EntityFrameworkCore;
using todo.Models;
using DotNetEnv;
using todo.Services.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using todo.Services;


var builder = WebApplication.CreateBuilder(args);
//*****************************************************************************************

//測試 user-secrets 儲存敏感資訊
// Console.WriteLine("JWT SECRET => " + builder.Configuration["Jwt:Key"]);
// Console.WriteLine("DB => " + builder.Configuration.GetConnectionString("DefaultConnection"));



//用 env 傳資料
// var Server = Environment.GetEnvironmentVariable("Server");
// var Database = Environment.GetEnvironmentVariable("Database");
// var UserId = Environment.GetEnvironmentVariable("UserId");
// var Password = Environment.GetEnvironmentVariable("Password");
// var connectionString =
//     $"Server={Server};Database={Database};User Id={UserId};Password={Password};TrustServerCertificate=True;";

//*****************************************************************************************

// 自動載入 appsettings.json + appsettings.Development.json 的地方
Env.Load(".env");
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddScoped<IUserIdValidationService, UserValidationService>();
builder.Services.AddScoped<JwtService>();
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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });


//*****************************************************************************************

//匯入資料庫連線設定
builder.Services.AddDbContext<TodoListContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

//Seed 初始資料
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.Run();



record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    
}


