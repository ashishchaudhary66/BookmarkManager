using BookmarkManager.Data;
using BookmarkManager.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure HttpClient for PageTitleService
builder.Services.AddHttpClient(
    "PageTitle",
    client =>
    {
        client.Timeout = TimeSpan.FromSeconds(5);
        client.DefaultRequestHeaders.UserAgent.ParseAdd(
            "BookmarkManager/1.0");
    });

// Built-in OpenAPI
builder.Services.AddOpenApi();

// Swagger UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // OpenAPI JSON document
    app.MapOpenApi();

    // Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global exception handling middleware - register early so it wraps all downstream middleware
app.UseMiddleware<BookmarkManager.Middleware.ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();