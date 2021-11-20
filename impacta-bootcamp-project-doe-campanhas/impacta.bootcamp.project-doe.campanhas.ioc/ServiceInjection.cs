using System;
using impacta.bootcamp.project_doe.campanhas.infra.data.Data.Context;
using impacta.bootcamp.project_doe.campanhas.infra.data.ExternalServices.DoeAuth.Interfaces;
using impacta.bootcamp.project_doe.campanhas.infra.data.ExternalServices.DoeAuth.Services;
using Microsoft.Extensions.DependencyInjection;

namespace impacta.bootcamp.project_doe.campanhas.ioc
{
    public class ServiceInjection
    {
        private static ServiceProvider _provider;

        public static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<SqlContext>();
            services.AddScoped<impacta.bootcamp.project_doe.campanhas.core.Interfaces.UseCases.Create.ICreateUseCase, impacta.bootcamp.project.doe.campanhas.application.UseCases.Create.CreateUseCase>();
            services.AddScoped<impacta.bootcamp.project_doe.campanhas.core.Interfaces.Repositories.ICreateRepository, impacta.bootcamp.project_doe.campanhas.infra.data.Data.Repositories.CreateRepository>();
            services.AddScoped<IAuthServices, AuthServices>();
            _provider = services.BuildServiceProvider();
        }

        public static ServiceProvider GetServiceProvider()
        {
            return _provider;
        }
    }
}
