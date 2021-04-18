
using FluentValidation;
using JJS.Application.Behaviours;
using JJS.Application.FileUpload;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace JJS.Application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));               
            services.AddTransient<IDropboxHelper, DropboxHelper>();
        }
    }
}
