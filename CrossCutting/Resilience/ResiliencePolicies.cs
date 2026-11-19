using System;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

namespace DigiTFactory.Libraries.CrossCutting.Resilience
{
    /// <summary>
    /// Готовые Polly-политики отказоустойчивости (реализация паттернов из канона EDM, QG-6xx).
    /// retry + экспоненциальная задержка + jitter, circuit breaker, timeout.
    /// </summary>
    public static class ResiliencePolicies
    {
        /// <summary>Повтор с экспоненциальной задержкой и jitter (избегает «стада» повторных попыток).</summary>
        public static RetryStrategyOptions<TResult> RetryWithJitter<TResult>(int retryCount, TimeSpan medianFirstRetryDelay)
            => new RetryStrategyOptions<TResult>
            {
                MaxRetryAttempts = retryCount,
                BackoffType = DelayBackoffType.Exponential,
                Delay = medianFirstRetryDelay,
                UseJitter = true,
            };

        /// <summary>Circuit breaker: размыкает цепь при превышении доли ошибок и держит её указанное время.</summary>
        public static CircuitBreakerStrategyOptions<TResult> CircuitBreaker<TResult>(
            double failureRatio,
            int minimumThroughput,
            TimeSpan samplingDuration,
            TimeSpan breakDuration)
            => new CircuitBreakerStrategyOptions<TResult>
            {
                FailureRatio = failureRatio,
                MinimumThroughput = minimumThroughput,
                SamplingDuration = samplingDuration,
                BreakDuration = breakDuration,
            };

        /// <summary>Таймаут операции: ресурсы не блокируются бесконечно.</summary>
        public static TimeoutStrategyOptions Timeout(TimeSpan timeout)
            => new TimeoutStrategyOptions { Timeout = timeout };
    }
}

// 0.1.0 — retry+jitter, circuit-breaker, timeout (QG-609..614, публикуется через crosscutting.yml)
