using JJS.Application;
using JJS.Application.Interfaces;
using JJS.Infrastructure.Identity;
using JJS.Infrastructure.Persistence;
using JJS.Infrastructure.Shared;
using JJS.WebApi.Extensions;
using JJS.WebApi.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JJS.WebApi
{
    public class Startup
    {
        public IConfiguration _config { get; }
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationLayer();
            services.AddIdentityInfrastructure(_config);
            services.AddPersistenceInfrastructure(_config);
            services.AddSharedInfrastructure(_config);
            services.AddSwaggerExtension();
            services.AddControllers();
            services.AddApiVersioningExtension();
            services.AddHealthChecks();
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwaggerExtension();
            app.UseErrorHandlingMiddleware();
            app.UseHealthChecks("/health");

            app.UseEndpoints(endpoints =>
             {
                 endpoints.MapControllers();
             });
        }
    }
}
