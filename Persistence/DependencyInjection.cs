using Application.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Persistence.Repositories;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IEmailVerificationRepository, EmailVerificationRepository>();
            services.AddScoped<IFlashCardRepository, FlashCardRepository>();
            services.AddScoped<IDeckRepository, DeckRepository>();
            services.AddScoped<IFolderRepository, FolderRepository>();
            services.AddScoped<IUserFlashcardProgressRepository, UserFlashcardProgressRepository>();
            return services;
        }

        public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        string connectionString)
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

            dataSourceBuilder.EnableDynamicJson();

            var dataSource = dataSourceBuilder.Build();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(dataSource);
            });

            return services;
        }

    }
}
