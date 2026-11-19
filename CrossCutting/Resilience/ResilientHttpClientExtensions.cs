using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace DigiTFactory.Libraries.CrossCutting.Resilience
{
    /// <summary>
    /// Регистрирует типизированный HttpClient через IHttpClientFactory с устойчивыми политиками
    /// (retry+jitter → circuit breaker → timeout). Единственный разрешённый способ создавать HttpClient
    /// в микросервисе DTF (анти-паттерн `new HttpClient()` запрещён анализатором QG-609).
    /// </summary>
    public static class ResilientHttpClientExtensions
    {
        public static IHttpClientBuilder AddResilientHttpClient<TClient, TImplementation>(
            this IServiceCollection services,
            Action<HttpClient>? configureClient = null,
            int retryCount = 6,
            TimeSpan? breakDuration = null,
            TimeSpan? timeout = null)
            where TClient : class
            where TImplementation : class, TClient
        {
            var builder = services.AddHttpClient<TClient, TImplementation>();
            if (configureClient is not null)
            {
                builder.ConfigureHttpClient(configureClient);
            }

            builder.AddResilienceHandler("dtf-resilience", (pipeline, _) =>
            {
                pipeline
                    .AddRetry(ResiliencePolicies.RetryWithJitter<HttpResponseMessage>(retryCount, TimeSpan.FromSeconds(1)))
                    .AddCircuitBreaker(ResiliencePolicies.CircuitBreaker<HttpResponseMessage>(
                        0.5,
                        10,
                        TimeSpan.FromSeconds(10),
                        breakDuration ?? TimeSpan.FromSeconds(30)))
                    .AddTimeout(ResiliencePolicies.Timeout(timeout ?? TimeSpan.FromSeconds(100)));
            });

            return builder;
        }
    }
}
