using System;
using System.Collections.Generic;
using System.Linq;

namespace Nigel.Basic
{
    public static class ListExtension
    {
        /// <summary>
        ///     Intersects the specified t source list.
        /// </summary>
        /// <typeparam name="TSource">The type of the t source.</typeparam>
        /// <param name="first">The first.</param>
        /// <param name="tSourceList">The t source list.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>IEnumerable&lt;TSource&gt;.</returns>
        /// <exception cref="ArgumentNullException">
        ///     first
        ///     or
        ///     tSourceList
        /// </exception>
        public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first,
            IEnumerable<IEnumerable<TSource>> tSourceList, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (tSourceList == null)
                throw new ArgumentNullException(nameof(tSourceList));
            var intersectResult = first;
            foreach (var sourceItem in tSourceList)
            {
                if (sourceItem == null)
                    throw new ArgumentNullException(nameof(sourceItem));
                intersectResult = intersectResult.Intersect(sourceItem, comparer);
            }

            return intersectResult;
        }


        /// <summary>
        ///     Intersects the specified t source list.
        /// </summary>
        /// <typeparam name="TSource">The type of the t source.</typeparam>
        /// <param name="first">The first.</param>
        /// <param name="tSourceList">The t source list.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>IEnumerable&lt;TSource&gt;.</returns>
        /// <exception cref="ArgumentNullException">
        ///     first
        ///     or
        ///     tSourceList
        /// </exception>
        public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<IEnumerable<TSource>> tSourceList,
            IEqualityComparer<TSource> comparer)
        {
            if (tSourceList == null)
                throw new ArgumentNullException(nameof(tSourceList));
            IEnumerable<TSource> intersectResult = null;

            foreach (var sourceItem in tSourceList)
            {
                if (sourceItem == null)
                    throw new ArgumentNullException(nameof(sourceItem));
                var enumerable = sourceItem as TSource[] ?? sourceItem.ToArray();
                if (intersectResult == null) intersectResult = enumerable;

                intersectResult = intersectResult.Intersect(enumerable, comparer);
            }

            return intersectResult;
        }


        /// <summary>
        ///     Excepts the specified t source list.
        /// </summary>
        /// <typeparam name="TSource">The type of the t source.</typeparam>
        /// <param name="first">The first.</param>
        /// <param name="tSourceList">The t source list.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>IEnumerable&lt;TSource&gt;.</returns>
        /// <exception cref="ArgumentNullException">
        ///     first
        ///     or
        ///     tSourceList
        ///     or
        ///     sourceItem
        /// </exception>
        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first,
            IEnumerable<IEnumerable<TSource>> tSourceList, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (tSourceList == null)
                throw new ArgumentNullException(nameof(tSourceList));

            var intersectResult = first;

            foreach (var sourceItem in tSourceList)
            {
                if (sourceItem == null)
                    throw new ArgumentNullException(nameof(sourceItem));

                intersectResult = intersectResult.Except(sourceItem, comparer);
            }

            return intersectResult;
        }
    }
}