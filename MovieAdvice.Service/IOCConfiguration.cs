using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MovieAdvice.Infrastructure;
using MovieAdvice.Repository.Repositories;
using MovieAdvice.Service.Services;
using MovieAdvice.Service.Validations;

namespace MovieAdvice.Service
{
    public static class IOCConfiguration
    {
        public static void Initialize(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtHelper, JwtHelper>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IValidatorInterceptor, ValidationHandling>();
            services.AddScoped<IObjectConvertFormat, ObjectConvertFormat>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<ISendMailService, SendMailService>();
            services.AddScoped<IConsumerService, ConsumerService>();
            services.AddScoped<IPublisherService, PublisherService>();
            services.AddScoped<IRabbitMQService, RabbitMQService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IMailTemplateRepository, MailTemplateRepository>();
            services.AddScoped<IUserMovieNoteRepository, UserMovieNoteRepository>();
            services.AddScoped<IUserMovieRateRepository, UserMovieRateRepository>();
            services.AddScoped<IWebsiteParameterRepository, WebsiteParameterRepository>();
        }
    }
}
