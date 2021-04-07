using JJS.Application;
using JJS.Application.Interfaces;
using JJS.Infrastructure.Identity;
using JJS.Infrastructure.Persistence;
using JJS.Infrastructure.Shared;
using JJS.WebApi.Extensions;
using JJS.WebApi.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;

namespace JJS.WebApi
{
    public class Startup
    {
        public IConfiguration _config { get; }
        private readonly ILogger<Startup> _logger;

        public Startup(ILogger<Startup> logger, IConfiguration configuration)
        {
            _logger = logger;
            _config = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("ConfigureServices called");
            try
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

                // data
                services.AddDataProtection()
                           .SetDefaultKeyLifetime(TimeSpan.FromDays(14));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
                throw ex;
            }
            finally
            {
                _logger.LogInformation("ConfigureServices end");
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _logger.LogInformation("Configure called");
            try
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
            catch (Exception ex)
            {
                _logger.LogInformation(ex, ex.Message);
            }
            finally
            {
                _logger.LogInformation("Configure end");
            }
        }
    }
}
