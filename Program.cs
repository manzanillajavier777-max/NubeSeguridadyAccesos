using Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Leer DATABASE_URL desde Render
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

    return $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
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
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Urls.Add($"http://0.0.0.0:{port}");

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