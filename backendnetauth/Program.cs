using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using backendnet.Data;
using backendnet.Models;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// Agrega el soporte para MySQL
var connectionString = builder.Configuration.GetConnectionString("DataContext");
builder.Services.AddDbContext<IdentityContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddIdentity<CustomIdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    // Cambie aqui como quiere se manejen sus contraseñas
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<IdentityContext>();

// Agrega la funcionalidad de controladores 
builder.Services.AddControllers();

// Agrega la documentación de la API
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Si queremos mostrar la documentación de la API en la raíz 
if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "backend v1"); 
        options.RoutePrefix = string.Empty;
    });
}

// Redirige a HTTPS
app.UseHttpsRedirection();

// Utiliza rutas para los endpoints de los controladores
app.UseRouting();

// Agrega el middleware de autorización
app.UseAuthorization();

// Establece el uso de rutas sin especificar una por default 
app.MapControllers();

app.Run();
