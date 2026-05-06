using Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Obtener connection string (appsettings o Render)
var connectionString = builder.Configuration.GetConnectionString("Connection")
    ?? Environment.GetEnvironmentVariable("ConnectionStrings__ConnectionSeguridadyAccesos");

// 🔹 DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// 🔹 Servicios
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔹 Puerto dinámico (Render)
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Urls.Add($"http://0.0.0.0:{port}");

// 🔹 Base de datos (elige UNA opción)

// ✅ OPCIÓN SEGURA (PRODUCCIÓN)

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureDeleted();   // 💣 BORRA TODO
    context.Database.EnsureCreated();   // 🧱 CREA BIEN
}

/*
// ❌ OPCIÓN SOLO PRUEBAS (BORRA TODO)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    context.Database.EnsureDeleted();   // 💣 BORRA TODO
    context.Database.EnsureCreated();   // 🧱 CREA BIEN
}
*/

// 🔹 Swagger SIEMPRE activo (Render no usa Development)
app.UseSwagger();
app.UseSwaggerUI();

// 🔹 Middleware
// app.UseHttpsRedirection(); // Mejor desactivado en Render
app.UseAuthorization();

app.MapControllers();

app.Run();