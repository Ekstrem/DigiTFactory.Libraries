using DigiTFactory.Libraries.SeedWorks.Events;
using Microsoft.Extensions.DependencyInjection;

namespace DigiTFactory.Libraries.EventBus.InMemory.Extensions
{
    /// <summary>
    /// Расширения для регистрации In-Memory EventBus в DI контейнере.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Регистрирует In-Memory реализацию IEventBus (Singleton).
        /// </summary>
        public static IServiceCollection AddEventBusInMemory(this IServiceCollection services)
        {
            services.AddSingleton<InMemoryEventBus>();
            services.AddSingleton<IEventBus>(sp => sp.GetRequiredService<InMemoryEventBus>());
            services.AddSingleton<IEventBusProducer>(sp => sp.GetRequiredService<InMemoryEventBus>());
            services.AddSingleton<IEventBusConsumer>(sp => sp.GetRequiredService<InMemoryEventBus>());

            return services;
        }
    }
}
