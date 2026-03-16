using System;
using DigiTFactory.Libraries.AbstractAggregate.Invariants.Rules;
using DigiTFactory.Libraries.AbstractAggregate.Metadata;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.AbstractAggregate.Invariants
{
    /// <summary>
    /// Парсер DSL-выражений правил валидации.
    /// Формат: "RuleType[:Param1[:Param2[:Param3]]]"
    /// </summary>
    public static class RuleExpressionParser
    {
        /// <summary>
        /// Разобрать выражение правила и создать соответствующую спецификацию.
        /// </summary>
        /// <remarks>
        /// Поддерживаемые выражения:
        /// - IsNewEntity — агрегат новый (Version == 0)
        /// - IsNotNewEntity — агрегат существует (Version > 0)
        /// - VOExists:Name — Value Object существует и не null
        /// - PropertyEquals:VO.Prop:Value — свойство VO равно значению
        /// - PropertyNotNull:VO.Prop — свойство VO не null
        /// - CollectionNotEmpty:Name — коллекция VO не пуста
        /// - StateIs:State — текущее состояние агрегата
        /// </remarks>
        public static IBusinessOperationSpecification<TBoundedContext, IAnemicModel<TBoundedContext>>
            Parse<TBoundedContext>(InvariantMetadata metadata)
            where TBoundedContext : IBoundedContext
        {
            var expression = metadata.RuleExpression;
            var parts = expression.Split(':');
            var ruleType = parts[0];

            return (ruleType, metadata.Type) switch
            {
                ("IsNewEntity", InvariantType.Assertion) =>
                    new IsNewEntityAssertion<TBoundedContext>(metadata),
                ("IsNewEntity", InvariantType.Validator) =>
                    new IsNewEntityValidator<TBoundedContext>(metadata),

                ("IsNotNewEntity", InvariantType.Assertion) =>
                    new IsNotNewEntityAssertion<TBoundedContext>(metadata),
                ("IsNotNewEntity", InvariantType.Validator) =>
                    new IsNotNewEntityValidator<TBoundedContext>(metadata),

                ("VOExists", InvariantType.Assertion) =>
                    new VOExistsAssertion<TBoundedContext>(metadata, GetParam(parts, 1)),
                ("VOExists", InvariantType.Validator) =>
                    new VOExistsValidator<TBoundedContext>(metadata, GetParam(parts, 1)),

                ("PropertyEquals", InvariantType.Assertion) =>
                    ParsePropertyEquals<TBoundedContext>(metadata, parts, isAssertion: true),
                ("PropertyEquals", InvariantType.Validator) =>
                    ParsePropertyEquals<TBoundedContext>(metadata, parts, isAssertion: false),

                ("PropertyNotNull", InvariantType.Assertion) =>
                    ParsePropertyNotNull<TBoundedContext>(metadata, parts, isAssertion: true),
                ("PropertyNotNull", InvariantType.Validator) =>
                    ParsePropertyNotNull<TBoundedContext>(metadata, parts, isAssertion: false),

                ("CollectionNotEmpty", InvariantType.Assertion) =>
                    new CollectionNotEmptyAssertion<TBoundedContext>(metadata, GetParam(parts, 1)),
                ("CollectionNotEmpty", InvariantType.Validator) =>
                    new CollectionNotEmptyValidator<TBoundedContext>(metadata, GetParam(parts, 1)),

                ("StateIs", InvariantType.Assertion) =>
                    new StateIsAssertion<TBoundedContext>(metadata, GetParam(parts, 1)),
                ("StateIs", InvariantType.Validator) =>
                    new StateIsValidator<TBoundedContext>(metadata, GetParam(parts, 1)),

                _ => throw new ArgumentException(
                    $"Unknown rule expression: '{expression}'. " +
                    $"Supported rules: IsNewEntity, IsNotNewEntity, VOExists, PropertyEquals, " +
                    $"PropertyNotNull, CollectionNotEmpty, StateIs")
            };
        }

        /// <summary>
        /// Разбор "PropertyEquals:VO.Prop:Value"
        /// </summary>
        private static IBusinessOperationSpecification<TBoundedContext, IAnemicModel<TBoundedContext>>
            ParsePropertyEquals<TBoundedContext>(InvariantMetadata metadata, string[] parts, bool isAssertion)
            where TBoundedContext : IBoundedContext
        {
            var (voName, propName) = ParseVoProp(GetParam(parts, 1));
            var expectedValue = GetParam(parts, 2);

            return isAssertion
                ? new PropertyEqualsAssertion<TBoundedContext>(metadata, voName, propName, expectedValue)
                : new PropertyEqualsValidator<TBoundedContext>(metadata, voName, propName, expectedValue);
        }

        /// <summary>
        /// Разбор "PropertyNotNull:VO.Prop"
        /// </summary>
        private static IBusinessOperationSpecification<TBoundedContext, IAnemicModel<TBoundedContext>>
            ParsePropertyNotNull<TBoundedContext>(InvariantMetadata metadata, string[] parts, bool isAssertion)
            where TBoundedContext : IBoundedContext
        {
            var (voName, propName) = ParseVoProp(GetParam(parts, 1));

            return isAssertion
                ? new PropertyNotNullAssertion<TBoundedContext>(metadata, voName, propName)
                : new PropertyNotNullValidator<TBoundedContext>(metadata, voName, propName);
        }

        /// <summary>
        /// Разбор "VoName.PropertyName" → (voName, propName)
        /// </summary>
        private static (string voName, string propName) ParseVoProp(string voProp)
        {
            var dotIndex = voProp.IndexOf('.');
            if (dotIndex < 0)
                throw new ArgumentException(
                    $"Expected 'VoName.PropertyName' format, got: '{voProp}'");

            return (voProp[..dotIndex], voProp[(dotIndex + 1)..]);
        }

        private static string GetParam(string[] parts, int index)
        {
            if (index >= parts.Length)
                throw new ArgumentException(
                    $"Rule expression requires parameter at position {index}, " +
                    $"but expression has only {parts.Length} parts: '{string.Join(":", parts)}'");
            return parts[index];
        }
    }
}
