using Microsoft.Extensions.DependencyInjection;

namespace Cepdi.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        #region Implementacion de cors
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {

                options.AddPolicy("CorsPolicy",
                    policy =>
                    {
                        policy.WithOrigins("https://localhost:44351", "http://localhost:4200")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            
                            .AllowCredentials();
                    });
            });

        }
        #endregion
    }
}
