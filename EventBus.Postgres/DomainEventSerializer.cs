using System;
using System.Text.Json;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Events;

namespace DigiTFactory.Libraries.EventBus.Postgres
{
    /// <summary>
    /// Сериализация IDomainEvent в DomainEventEnvelope.
    /// </summary>
    internal static class DomainEventSerializer
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public static DomainEventEnvelope ToEnvelope<TBoundedContext>(IDomainEvent<TBoundedContext> domainEvent)
            where TBoundedContext : IBoundedContext
        {
            return new DomainEventEnvelope
            {
                AggregateId = domainEvent.Id,
                Version = domainEvent.Version,
                CorrelationToken = domainEvent.CorrelationToken,
                BoundedContext = domainEvent.ContextName,
                CommandName = domainEvent.Command.CommandName,
                SubjectName = domainEvent.Command.SubjectName,
                CommandVersion = domainEvent.Command.Version,
                ChangedValueObjectsJson = JsonSerializer.Serialize(domainEvent.ChangedValueObjects, JsonOptions),
                Result = domainEvent.Result.ToString(),
                Reason = domainEvent.Reason ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
