using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QCRunner.Core.Batches;
using QCRunner.Core.Methods;
using QCRunner.Data.Repositories;
using QCRunner.Data.Seeding;

namespace QCRunner.Data;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the SQLite context with lazy loading proxies, the repositories and the seeder.
    /// The context is scoped, so a batch run should happen inside one scope.
    /// </summary>
    public static IServiceCollection AddQCRunnerData(this IServiceCollection services, string connectionString)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        services.AddDbContext<QCRunnerDbContext>(options => options
            .UseSqlite(connectionString)
            .UseLazyLoadingProxies());

        services.AddScoped<IMethodRepository, MethodRepository>();
        services.AddScoped<IBatchRepository, BatchRepository>();
        services.AddScoped<DatabaseInitializer>();

        return services;
    }
}
