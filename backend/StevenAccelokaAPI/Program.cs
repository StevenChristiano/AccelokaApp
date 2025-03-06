using Microsoft.EntityFrameworkCore;
using Serilog;
using StevenAccelokaAPI.Helpers;
//using StevenAccelokaAPI.Services;
using MediatR;
using FluentValidation;
using System.Reflection;
using Hellang.Middleware.ProblemDetails;


var logsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
Directory.CreateDirectory(logsDirectory); // Pastikan folder logs ada sebelum logging dimulai

var logFilePath = Path.Combine(logsDirectory, "Log-.txt"); // Gunakan template untuk rolling file("Cors:AllowedOrigins").Get<string[]>();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File(logFilePath,
                  rollingInterval: RollingInterval.Day,
                  retainedFileCountLimit: 7,
                  flushToDiskInterval: TimeSpan.FromSeconds(1),
                  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] - {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
// ✅ Add ProblemDetails middleware
Hellang.Middleware.ProblemDetails.ProblemDetailsExtensions.AddProblemDetails(builder.Services);


// ✅ Konfigurasi Serilog
// Konfigurasi Serilog menggunakan builder.Host
builder.Host.UseSerilog();
// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

// Register FluentValidation
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
// ✅ Tambahkan services ke container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddScoped<IBookedTicketService, BookedTicketService>();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:3000") // Allow frontend
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

Log.Information("Aplikasi telah dimulai dan Serilog sudah aktif.");

// ✅ Logging request sebelum middleware lain
app.UseSerilogRequestLogging();

// Use ProblemDetails Middleware
app.UseProblemDetails();
// ✅ Konfigurasi middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(builder =>
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.Run();
