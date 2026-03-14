using System;

namespace DigiTFactory.Libraries.EventBus.Postgres
{
    /// <summary>
    /// Плоский DTO для сериализации доменного события в JSON.
    /// </summary>
    internal sealed class DomainEventEnvelope
    {
        public Guid AggregateId { get; set; }
        public long Version { get; set; }
        public Guid CorrelationToken { get; set; }
        public string BoundedContext { get; set; } = string.Empty;
        public string CommandName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public long CommandVersion { get; set; }
        public string ChangedValueObjectsJson { get; set; } = "{}";
        public string Result { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
