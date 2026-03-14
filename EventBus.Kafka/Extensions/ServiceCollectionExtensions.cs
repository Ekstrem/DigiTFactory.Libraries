using System;
using DigiTFactory.Libraries.EventBus.Kafka.Configuration;
using DigiTFactory.Libraries.SeedWorks.Events;
using Microsoft.Extensions.DependencyInjection;

namespace DigiTFactory.Libraries.EventBus.Kafka.Extensions
{
    /// <summary>
    /// Расширения для регистрации Kafka EventBus в DI контейнере.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Регистрирует Kafka реализацию IEventBus.
        /// </summary>
        public static IServiceCollection AddEventBusKafka(
            this IServiceCollection services,
            Action<KafkaEventBusOptions> configureOptions)
        {
            var options = new KafkaEventBusOptions();
            configureOptions(options);

            services.AddSingleton(options);
            services.AddSingleton<KafkaEventBusProducer>();
            services.AddSingleton<KafkaEventBusConsumer>();

            services.AddSingleton<KafkaEventBus>(sp => new KafkaEventBus(
                sp.GetRequiredService<KafkaEventBusProducer>(),
                sp.GetRequiredService<KafkaEventBusConsumer>()));

            services.AddSingleton<IEventBus>(sp => sp.GetRequiredService<KafkaEventBus>());
            services.AddSingleton<IEventBusProducer>(sp => sp.GetRequiredService<KafkaEventBus>());
            services.AddSingleton<IEventBusConsumer>(sp => sp.GetRequiredService<KafkaEventBus>());

            services.AddHostedService<KafkaConsumerHostedService>();

            return services;
        }
    }
}
