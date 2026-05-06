using Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Obtener DATABASE_URL desde Render
var rawConnection = Environment.GetEnvironmentVariable("DATABASE_URL");

if (string.IsNullOrEmpty(rawConnection))
{
    throw new Exception("DATABASE_URL no configurado");
}

// 🔹 Convertir de formato Render (postgresql://...) a formato EF Core
string ConvertFromRender(string url)
{
    var uri = new Uri(url);
    var userInfo = uri.UserInfo.Split(':');

    // 🔥 FIX: si no hay puerto, usar 5432
    var port = uri.Port == -1 ? 5432 : uri.Port;

    return $"Host={uri.Host};Port={port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
}

var connectionString = ConvertFromRender(rawConnection);

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
var portEnv = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Urls.Add($"http://0.0.0.0:{portEnv}");

// 🔹 Migraciones automáticas
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

// 🔹 Swagger siempre activo
app.UseSwagger();
app.UseSwaggerUI();

// 🔹 Middleware
app.UseAuthorization();

app.MapControllers();

app.Run();