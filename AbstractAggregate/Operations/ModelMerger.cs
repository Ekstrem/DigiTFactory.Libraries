using System;
using System.Collections.Generic;
using System.Linq;
using DigiTFactory.Libraries.AbstractAggregate.DynamicTypes;
using DigiTFactory.Libraries.AbstractAggregate.Metadata;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Events;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.AbstractAggregate.Operations
{
    /// <summary>
    /// Строит новую анемичную модель, объединяя текущее состояние агрегата
    /// с входящими Value Object-ами согласно стратегии слияния из метаданных.
    /// </summary>
    public static class ModelMerger
    {
        /// <summary>
        /// Слияние текущей модели с входящими Value Object-ами.
        /// </summary>
        /// <param name="current">Текущее состояние агрегата.</param>
        /// <param name="incomingValueObjects">Новые Value Object-ы от команды.</param>
        /// <param name="command">Команда к агрегату.</param>
        /// <param name="operationMetadata">Метаданные операции (стратегия, затронутые VO).</param>
        public static DynamicAnemicModel<TBoundedContext> Merge<TBoundedContext>(
            IAnemicModel<TBoundedContext> current,
            IDictionary<string, IValueObject> incomingValueObjects,
            CommandToAggregate command,
            OperationMetadata operationMetadata)
            where TBoundedContext : IBoundedContext
        {
            var currentVOs = current.GetValueObjects();
            var resultVOs = new Dictionary<string, IValueObject>();

            switch (operationMetadata.MergeStrategy)
            {
                case OperationMergeStrategy.FullReplace:
                    // Полная замена: берём все incoming VO
                    foreach (var (key, value) in incomingValueObjects)
                        resultVOs[key] = value;
                    // Сохраняем VO, которых нет в incoming
                    foreach (var (key, value) in currentVOs)
                    {
                        if (!resultVOs.ContainsKey(key))
                            resultVOs[key] = value;
                    }
                    break;

                case OperationMergeStrategy.ReplaceAffected:
                    // Копируем все текущие VO
                    foreach (var (key, value) in currentVOs)
                        resultVOs[key] = value;
                    // Заменяем только затронутые
                    var affected = new HashSet<string>(
                        operationMetadata.AffectedValueObjects,
                        StringComparer.OrdinalIgnoreCase);
                    foreach (var (key, value) in incomingValueObjects)
                    {
                        if (affected.Count == 0 || affected.Contains(key))
                            resultVOs[key] = value;
                    }
                    break;

                case OperationMergeStrategy.AppendCollections:
                    // Копируем все текущие VO
                    foreach (var (key, value) in currentVOs)
                        resultVOs[key] = value;
                    // Для коллекций — добавляем, для скаляров — заменяем
                    foreach (var (key, value) in incomingValueObjects)
                    {
                        if (resultVOs.TryGetValue(key, out var existing) &&
                            existing is DynamicCollectionValueObject existingCollection &&
                            value is DynamicCollectionValueObject incomingCollection)
                        {
                            resultVOs[key] = existingCollection.Append(incomingCollection);
                        }
                        else
                        {
                            resultVOs[key] = value;
                        }
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(operationMetadata.MergeStrategy),
                        operationMetadata.MergeStrategy,
                        "Unknown merge strategy");
            }

            return DynamicAnemicModel<TBoundedContext>.Create(
                current.Id,
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                command.CorrelationToken,
                command.CommandName,
                command.SubjectName,
                resultVOs);
        }
    }
}
