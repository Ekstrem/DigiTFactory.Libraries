using System;
using System.Collections.Generic;
using System.Data;

namespace Hive.Dal.RawSql
{
    /// <summary>
    /// Компонент маппинга результата SQL выборки на сложный объект
    /// NOTE:
    /// - количество, наименования полей и типы значений в результате выборки и свойств в классе результата должны совпадать
    /// - учитываются только публичные свойства в классе результата (они должны именть getter и setter)
    /// - регистр в результате и названии свойства не важен
    /// </summary>
    /// <typeparam name="TResult">Тип объекта результата</typeparam>
    internal class SqlResultMapper<TResult>
        where TResult : class
    {
        // ReSharper disable StaticMemberInGenericType
        private static readonly int PropertiesNumber;
        private static readonly Func<IDataRecord, TResult> ConvertFunction;
        // ReSharper restore StaticMemberInGenericType

        static SqlResultMapper()
        {
            PropertiesNumber = typeof(TResult).GetProperties().Length;

            var converterBuilder = new RecordConverterBuilder();
            ConvertFunction = converterBuilder.BuildFunc<TResult>();
        }

        /// <summary>
        /// Читает запить из SQL выборки и заполняет коллекцию сложных объектов
        /// </summary>
        /// <param name="reader"><see cref="IDataReader"/></param>
        public IReadOnlyList<TResult> MapList(IDataReader reader)
        {
            ThrowIfIncorrectFieldsNumber(reader);

            var list = new List<TResult>();
            while (reader.Read())
            {
                list.Add(ConvertFunction(reader));
            }

            return list;
        }

        /// <summary>
        /// Читает запить из SQL выборки и заполняет сложный объект
        /// </summary>
        /// <param name="reader"><see cref="IDataReader"/></param>
        public TResult MapSingle(IDataReader reader)
        {
            ThrowIfIncorrectFieldsNumber(reader);

            return reader.Read() ? ConvertFunction(reader) : null;
        }

        private void ThrowIfIncorrectFieldsNumber(IDataReader reader)
        {
            if (reader.FieldCount != PropertiesNumber)
            {
                throw new InvalidOperationException(
                    "Number of properties in a result type have to match to number of fields query");
            }
        }
    }
}