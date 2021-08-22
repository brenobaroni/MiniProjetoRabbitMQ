using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Service.Interfaces;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Service.Consumers.Options;
using Service.Consumers;
using Microsoft.Extensions.Configuration;

namespace MiniProjetoRabbitMQ.Configuration
{
    public class DependencyInjection
    {

        #region [ Método Externo ]

        public static void AddDependencies(ref IServiceCollection services, IConfiguration configuration)
        {
            AddGenericCore(ref services);
            AddEspecificCore(ref services);
            AddConfiguration(ref services, configuration);
        }

        #endregion [ Método Externo ]

        #region [ Repositório Genérico ]

        private static void AddGenericCore(ref IServiceCollection services)
        {

        }

        #endregion [ Repositório Genérico ]

        #region [ Repositório Específicos - Core]

        private static void AddEspecificCore(ref IServiceCollection services)
        {
            services.AddScoped(typeof(INotificationService), typeof(NotificationService));
        }

        #endregion [ Repositório Específicos ]

        #region [ Repositório de Configuração ]
        private static void AddConfiguration(ref IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MiniProjetoRabbitMQ", Version = "v1" });
            });

            services.AddHostedService<ProcessMessageConsumer>();
            services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMqConfig"));

        }
        #endregion
    }
}
