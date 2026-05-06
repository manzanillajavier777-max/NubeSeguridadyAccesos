using Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =======================
// 🔹 CORS CONFIGURADO
// =======================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// =======================
// 🔹 DATABASE (RENDER POSTGRES)
// =======================
var rawConnection = Environment.GetEnvironmentVariable("DATABASE_URL");

if (string.IsNullOrEmpty(rawConnection))
{
    throw new Exception("DATABASE_URL no configurado");
}

string ConvertFromRender(string url)
{
    var uri = new Uri(url);
    var userInfo = uri.UserInfo.Split(':');

    var port = uri.Port == -1 ? 5432 : uri.Port;

    return $"Host={uri.Host};Port={port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
}

var connectionString = ConvertFromRender(rawConnection);

// =======================
// 🔹 DB CONTEXT
// =======================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// =======================
// 🔹 SERVICES
// =======================
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// =======================
// 🔹 PORT (RENDER)
// =======================
var portEnv = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Urls.Add($"http://0.0.0.0:{portEnv}");

// =======================
// 🔹 MIDDLEWARE PIPELINE
// =======================

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// 🔥 IMPORTANTE: orden correcto
app.UseRouting();

// 🔥 CORS ACTIVADO AQUÍ
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// =======================
// 🔹 MIGRACIONES AUTOMÁTICAS
// =======================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

app.Run();