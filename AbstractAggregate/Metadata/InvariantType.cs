namespace DigiTFactory.Libraries.AbstractAggregate.Metadata
{
    /// <summary>
    /// Тип инварианта бизнес-операции.
    /// </summary>
    public enum InvariantType
    {
        /// <summary>
        /// Assertion — при нарушении операция завершается с Exception.
        /// </summary>
        Assertion = 0,

        /// <summary>
        /// Validator — при нарушении операция завершается с Warning.
        /// </summary>
        Validator = 1
    }
}
