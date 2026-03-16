namespace DigiTFactory.Libraries.AbstractAggregate.Metadata
{
    /// <summary>
    /// Метаданные перехода состояний (секция 3 Aggregate Design Canvas — State Transitions).
    /// </summary>
    public sealed class StateTransitionMetadata
    {
        /// <summary>
        /// Исходное состояние.
        /// </summary>
        public string FromState { get; set; } = string.Empty;

        /// <summary>
        /// Целевое состояние.
        /// </summary>
        public string ToState { get; set; } = string.Empty;

        /// <summary>
        /// Имя операции, вызывающей переход.
        /// </summary>
        public string TriggerOperation { get; set; } = string.Empty;
    }
}
