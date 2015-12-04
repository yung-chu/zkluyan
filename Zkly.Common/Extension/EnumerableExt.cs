using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.Common.Extension
{
    public static class EnumerableExt
    {
        #region IEnumerable Methods
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.DistinctBy(keySelector, null);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            var knownKeys = new HashSet<TKey>(comparer);
            return source.Where(element => knownKeys.Add(keySelector(element)));
        }

        /// <summary>
        /// a better way to cast unknown IEnumerable to typed list
        /// </summary>
        public static List<T> CastToList<T>(this IEnumerable source)
        {
            var list = new List<T>();
            if (source != null)
            {
                foreach (T item in source)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        /// <summary>
        /// a better way to cast unknown IEnumerable to typed Collection
        /// </summary>
        public static Collection<T> CastToCollection<T>(this IEnumerable source)
        {
            var list = new Collection<T>();
            if (source != null)
            {
                foreach (T item in source)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        /// <summary>
        /// Check if the sequence is null, if yes, return an empty sequence instead.
        /// </summary>
        /// <typeparam name="T">the element type of the sequence</typeparam>
        /// <param name="theSource">the souce sequence</param>
        /// <returns>a sequence</returns>
        public static IEnumerable<T> TrimNull<T>(this IEnumerable<T> theSource)
        {
            return theSource ?? Enumerable.Empty<T>();
        }

        /// <summary>
        /// Check if the sequence is null or empty
        /// </summary>
        /// <typeparam name="T">the element type of the sequence</typeparam>
        /// <param name="source">the souce sequence</param>
        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        /// <summary>
        /// add range of items, null or empty items will be ignored.
        /// </summary>
        /// <typeparam name="T">the element type of the sequence</typeparam>
        /// <param name="source">the souce sequence</param>
        /// <param name="items">items to be added into the collection</param>
        public static void AddMany<T>(this ICollection<T> source, IEnumerable<T> items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    source.Add(item);
                }
            }
        }

        /// <summary>
        /// Returns if the sequence that satisfies a condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1"/> to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <returns>
        /// if exists any element that satisfies the condition.
        /// </returns>
        public static bool Contains<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Any(predicate);
        }

        /// <summary>
        /// concatenates a sequence by seperator
        /// </summary>
        /// <param name="theSource">the souce string</param>
        /// <param name="theSelector">the element selector</param>
        /// <param name="theSeperator">seperator</param>
        /// <returns>a string</returns>
        public static string JoinToString<T>(this IEnumerable<T> theSource, Func<T, string> theSelector, string theSeperator)
        {
            return theSource.TrimNull().Select(theSelector).Join(theSeperator);
        }

        /// <summary>
        /// concatenates a sequence by seperator
        /// </summary>
        /// <param name="theSource">the souce string</param>
        /// <param name="theSeperator">seperator</param>
        /// <returns>a string</returns>
        public static string Join(this IEnumerable<string> theSource, string theSeperator)
        {
            return string.Join(theSeperator, theSource.TrimNull().Where(s => !string.IsNullOrEmpty(s)).ToArray());
        }

        /// <summary>
        /// concatenates a sequence by seperator
        /// </summary>
        /// <param name="theSource">the souce string</param>
        /// <param name="formatter">seperate item formatter</param>
        /// <returns>a string</returns>
        public static string Join(this IEnumerable<string> theSource, Func<string, string> formatter)
        {
            var builder = new StringBuilder();
            foreach (var s in theSource.TrimNull())
            {
                builder.Append(formatter(s));
            }

            return builder.ToString();
        }

        #endregion
    }
}
