using System;

namespace Hive.SeedWorks.Monads
{
    public static class MonadExtentions
    {
        /// <summary>
        /// Разветвление результата.
        /// Алльтернатива тернарному оператору присваивания,
        /// для написания FluentApi-style code.
        /// </summary>
        /// <param name="o">Объект применения разветвления.</param>
        /// <param name="condition">Условие разветвления.</param>
        /// <param name="ifTrue">Функция при истинности условия.</param>
        /// <param name="ifFalse">Функция при ложности условия.</param>
        /// <typeparam name="TSource">Входящий тип запроса.</typeparam>
        /// <typeparam name="TResult">Тип результирующего значения.</typeparam>
        /// <returns>Результат разветвления.</returns>
        public static TResult Either<TSource, TResult>(
            this TSource o,
            Func<TSource, bool> condition,
            Func<TSource, TResult> ifTrue,
            Func<TSource, TResult> ifFalse)
            => condition(o) ? ifTrue(o) : ifFalse(o);

        /// <summary>
        /// Применение функции к результирующему значению.
        /// Для написания FluentApi-style code.
        /// </summary>
        /// <typeparam name="TSource">Входящий тип запроса.</typeparam>
        /// <typeparam name="TResult">Тип результирующего значения.</typeparam>
        /// <param name="source">Объект над которым нужно использовать функцию.</param>
        /// <param name="func">Функция преобразования объекта.</param>
        /// <returns></returns>
        public static TResult PipeTo<TSource, TResult>(
            this TSource source, Func<TSource, TResult> func)
            => func(source);

        /// <summary>
        /// Выполнить функцию побочного эффекта над объектом.
        /// </summary>
        /// <typeparam name="T">Тип объекта над которым выполняется действие.</typeparam>
        /// <param name="obj">Объект над которым выполняется действие.</param>
        /// <param name="action">Действие над объектом.</param>
        /// <returns>Объект над которым выполнено действие.</returns>
        public static T Do<T>(this T obj, Action<T> action)
        {
            if (obj != null)
            {
                action(obj);
            }

            return obj;
        }
    }
}
