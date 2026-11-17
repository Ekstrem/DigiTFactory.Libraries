using System;

namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Помечает CommandHandler, который осознанно НЕ работает с агрегатом и потому
    /// не инжектит IAggregateProvider в конструктор. Анализатор QG-210 подсвечивает такой
    /// хендлер как warning (осознанное исключение), а не как ошибку.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class CommandHandlerNoAggregateAttribute : Attribute
    {
    }
}
