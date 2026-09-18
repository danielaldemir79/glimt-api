using Glimt.Api.Data;
using Glimt.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Kopplar AppDbContext till SQLite och anslutningssträngen i appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IMemoryService, MemoryService>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    string xmlPath = Path.Combine(
        AppContext.BaseDirectory,
        "Glimt.Api.xml");

    options.IncludeXmlComments(xmlPath);

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Glimt API",
        Version = "v1",
        Description = "API för fotodagboken Glimt. Hanterar minnen och bilduppladdning."
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Swagger används bara under utveckling
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("Frontend");
app.MapControllers();

app.Run();