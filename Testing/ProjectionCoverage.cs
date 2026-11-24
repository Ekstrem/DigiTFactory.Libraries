using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigiTFactory.Libraries.Testing
{
    /// <summary>
    /// Поведенческие проверки проекций (QG-707): rebuild == incremental и идемпотентность
    /// повторной доставки. Выполняются в IntegrationTests на реальном хранилище.
    /// </summary>
    public static class ProjectionCoverage
    {
        /// <summary>Rebuild должен давать то же состояние, что incremental-проекция.</summary>
        public static async Task VerifyRebuildEquivalence<T>(
            Func<Task<T>> incremental, Func<Task<T>> rebuild, Func<T, T, bool> comparer = null)
        {
            var a = await incremental();
            var b = await rebuild();
            if (!AreEqual(a, b, comparer))
            {
                throw new InvalidOperationException("Rebuild result differs from incremental projection (QG-707).");
            }
        }

        /// <summary>Повторная доставка того же события не должна менять проекцию.</summary>
        public static async Task VerifyIdempotency<T>(
            Func<Task<T>> projectOnce, Func<Task<T>> projectAgain, Func<T, T, bool> comparer = null)
        {
            var first = await projectOnce();
            var second = await projectAgain();
            if (!AreEqual(first, second, comparer))
            {
                throw new InvalidOperationException("Projection is not idempotent: repeated event changed the state (QG-707).");
            }
        }

        private static bool AreEqual<T>(T a, T b, Func<T, T, bool> comparer)
            => comparer != null ? comparer(a, b) : EqualityComparer<T>.Default.Equals(a, b);
    }
}
