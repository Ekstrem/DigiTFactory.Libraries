using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Hive.Dal.RawSql
{
    /// <summary>
    /// Компонент построения функции чтения записи из результата выборки SQL
    /// </summary>
    public class RecordConverterBuilder
    {
        /// <summary>
        /// Свойство интерфейса IDataRecord: object this[string name] { get; }
        /// </summary>
        private static readonly PropertyInfo DataReaderIndexer;

        /// <summary>
        /// Метод текущего класс: object FixReferenceValue(object value)
        /// </summary>
        private static readonly MethodInfo FixReferenceValueMethod;

        /// <summary>
        /// Метод текущего класс: object ChangeNullableValueType(object value, Type resultType)
        /// </summary>
        private static readonly MethodInfo ChangeNullableValueTypeMethod;

        /// <summary>
        /// Метод текущего класс: object ChangeValueType(object value, Type resultType)
        /// </summary>
        private static readonly MethodInfo ChangeValueTypeMethod;

        static RecordConverterBuilder()
        {
            DataReaderIndexer = typeof(IDataRecord).GetProperties()
                .Single(p =>
                {
                    var parameters = p.GetIndexParameters();
                    return p.Name == "Item" &&
                           parameters.Length == 1 &&
                           parameters[0].ParameterType == typeof(string);
                });

            var staticMethods = typeof(RecordConverterBuilder).GetMethods(BindingFlags.Static | BindingFlags.NonPublic);
            FixReferenceValueMethod = staticMethods.Single(m => m.Name == nameof(FixReferenceValue));
            ChangeValueTypeMethod = staticMethods.Single(m => m.Name == nameof(ChangeValueType));
            ChangeNullableValueTypeMethod = staticMethods.Single(m => m.Name == nameof(ChangeNullableValueType));
        }

        /// <summary>
        /// Построить функцию
        /// </summary>
        /// <typeparam name="TResult">Тип результата</typeparam>
        public Func<IDataRecord, TResult> BuildFunc<TResult>()
            where TResult : class
        {
            return CreateLambda<TResult>().Compile();
        }

        private Expression<Func<IDataRecord, TResult>> CreateLambda<TResult>()
        {
            var resultType = typeof(TResult);

            var parameterExp = Expression.Parameter(typeof(IDataRecord));
            var bindings = resultType.GetProperties().Select(p => CreatePropertyBinding(parameterExp, p));

            var resultTypeCtor = GetDefaultCtor(resultType);
            var bodyExp = Expression.MemberInit(Expression.New(resultTypeCtor), bindings);

            return Expression.Lambda<Func<IDataRecord, TResult>>(bodyExp, parameterExp);
        }

        private MemberAssignment CreatePropertyBinding(ParameterExpression parameter, PropertyInfo resultProperty)
        {
            // Example: dataRecord["key"]
            var keyExp = Expression.Constant(resultProperty.Name, typeof(string));
            var indexerExp = Expression.Property(parameter, DataReaderIndexer, keyExp);

            // Example: (int) SafeChangeType(dataRecord["key"], typeof(int))
            var convertExp = CreateConvertExpression(indexerExp, resultProperty.PropertyType);

            return Expression.Bind(resultProperty, convertExp);
        }

        private UnaryExpression CreateConvertExpression(IndexExpression indexerExp, Type resultPropertyType)
        {
            MethodCallExpression methodCallExp;
            if (resultPropertyType.IsValueType)
            {
                if (resultPropertyType.IsGenericType && resultPropertyType.Name.StartsWith("Nullable"))
                {
                    var resultNotNullableType = resultPropertyType.GenericTypeArguments[0];
                    methodCallExp = Expression.Call(
                        ChangeNullableValueTypeMethod, indexerExp, Expression.Constant(resultNotNullableType));
                }
                else
                {
                    methodCallExp = Expression.Call(
                        ChangeValueTypeMethod, indexerExp, Expression.Constant(resultPropertyType));
                }
            }
            else
            {
                methodCallExp = Expression.Call(FixReferenceValueMethod, indexerExp);
            }

            return Expression.Convert(methodCallExp, resultPropertyType);
        }

        private ConstructorInfo GetDefaultCtor(Type type)
        {
            return type.GetConstructors().Single(c => c.GetParameters().Length == 0);
        }

        private static object FixReferenceValue(object value)
        {
            return !IsNull(value) ? value : null;
        }

        private static object ChangeNullableValueType(object value, Type resultType)
        {
            return !IsNull(value)
                ? Convert.ChangeType(value, resultType)
                : null;
        }

        private static object ChangeValueType(object value, Type resultType)
        {
            return !IsNull(value)
                ? Convert.ChangeType(value, resultType)
                : Activator.CreateInstance(resultType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsNull(object value) => value == null || Convert.IsDBNull(value);
    }
}