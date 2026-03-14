using System;
using System.Text.Json;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Events;

namespace DigiTFactory.Libraries.EventBus.Kafka
{
    /// <summary>
    /// Сериализация IDomainEvent в JSON через DomainEventEnvelope.
    /// </summary>
    internal static class DomainEventSerializer
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public static string Serialize<TBoundedContext>(IDomainEvent<TBoundedContext> domainEvent)
            where TBoundedContext : IBoundedContext
        {
            var envelope = new DomainEventEnvelope
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

            return JsonSerializer.Serialize(envelope, JsonOptions);
        }

        public static DomainEventEnvelope Deserialize(string json)
        {
            return JsonSerializer.Deserialize<DomainEventEnvelope>(json, JsonOptions)
                   ?? throw new InvalidOperationException("Failed to deserialize DomainEventEnvelope");
        }
    }
}
