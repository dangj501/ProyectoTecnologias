using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TuProyecto
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Este método configura los servicios de la aplicación.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configura CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3001") // Cambia esto con tus orígenes permitidos
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            // Otros servicios que tu aplicación pueda necesitar
            services.AddControllers();
        }

        // Este método configura la canalización de solicitud HTTP.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Configura manejo de errores en producción
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // Agrega CORS a la canalización de solicitud HTTP
            app.UseCors("AllowSpecificOrigin");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
