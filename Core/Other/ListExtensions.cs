using System;
using System.Collections.Generic;
namespace ATBS.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Adds the value from the source list to the target list.
        /// </summary>
        /// <typeparam name="TValue">The type of the values in the lists.</typeparam>
        /// <param name="target">The target list to add value to.</param>
        /// <param name="source">The source list to add value to.</param>
        public static void Add<TValue>(this IList<TValue> target, IList<TValue> source)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (target == null) throw new ArgumentNullException("target");

            foreach (var value in source)
            {
                target.Add(value);
            }
        }

    }
}