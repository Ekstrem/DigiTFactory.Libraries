using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DigiTFactory.Libraries.CrossCutting.Health
{
    /// <summary>Регистрация health-checks (QG-614): AddHealthChecks + проверка зависимости.</summary>
    public static class HealthCheckExtensions
    {
        /// <summary>
        /// Регистрирует проверку работоспособности зависимости по произвольному пробнику.
        /// Пробник возвращает true, если зависимость доступна.
        /// </summary>
        public static IHealthChecksBuilder AddDependencyHealthCheck(
            this IServiceCollection services,
            string name,
            Func<CancellationToken, Task<bool>> probe,
            params string[] tags)
            => services
                .AddHealthChecks()
                .AddCheck(name, new ProbeHealthCheck(probe), tags: tags);

        private sealed class ProbeHealthCheck : IHealthCheck
        {
            private readonly Func<CancellationToken, Task<bool>> _probe;

            public ProbeHealthCheck(Func<CancellationToken, Task<bool>> probe) => _probe = probe;

            public async Task<HealthCheckResult> CheckHealthAsync(
                HealthCheckContext context,
                CancellationToken cancellationToken = default)
            {
                try
                {
                    return await _probe(cancellationToken).ConfigureAwait(false)
                        ? HealthCheckResult.Healthy()
                        : HealthCheckResult.Unhealthy();
                }
                catch (Exception ex)
                {
                    return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
                }
            }
        }
    }
}
