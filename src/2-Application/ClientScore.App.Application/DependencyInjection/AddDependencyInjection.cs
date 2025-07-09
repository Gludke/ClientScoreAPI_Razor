using ClientScore.App.Application.Services;
using ClientScore.App.Data.Repositories;
using ClientScore.App.Domain.Factorys;
using ClientScore.App.Domain.Interfaces.Factorys;
using ClientScore.App.Domain.Interfaces.Repositories;
using ClientScore.App.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ClientScore.App.Application.DependencyInjection;

public static class AddDependencyInjection
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IClienteService, ClienteService>();
    }

    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IClienteRepository, ClienteRepository>();
    }

    public static void RegisterFactorys(this IServiceCollection services)
    {
        services.AddScoped<IScoreCalculatorFactory, ScoreCalculatorFactory>();
    }
}
