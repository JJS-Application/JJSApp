using AutoMapper;

using FluentValidation;

using JJS.Application.Behaviours;
using JJS.Application.Features.Products.Commands.CreateProduct;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

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

        }
    }
}
