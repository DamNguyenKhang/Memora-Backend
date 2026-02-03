using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Exceptions;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddProblemDetails(configure =>
                configure.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
                }
            );
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFolderService, FolderService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IDeckService, DeckService>();
            services.AddScoped<IStudyService, StudyService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICloudService, CloudService>();
            services.AddScoped<IFolderService, FolderService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;
        }

        public static IServiceCollection AddExceptionHandler(this IServiceCollection services)
        {
            services.AddExceptionHandler<ValidationExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            return services;
        }
    }
}
