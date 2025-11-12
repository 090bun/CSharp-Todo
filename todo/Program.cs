using Microsoft.EntityFrameworkCore;
using todo.Models;
using DotNetEnv;
using todo.Services.Validation;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);
// 自動載入 appsettings.json + appsettings.Development.json 的地方
Env.Load(".env");
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddScoped<IUserIdValidationService, UserValidationService>();
//用 env 傳資料
var Server = Environment.GetEnvironmentVariable("Server");
var Database = Environment.GetEnvironmentVariable("Database");
var UserId = Environment.GetEnvironmentVariable("UserId");
var Password = Environment.GetEnvironmentVariable("Password");

var connectionString =
    $"Server={Server};Database={Database};User Id={UserId};Password={Password};TrustServerCertificate=True;";

//匯入資料庫連線設定
builder.Services.AddDbContext<TodoListContext>(options =>
    options.UseSqlServer(connectionString));

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
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
