using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ATBS.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Adds the key-value pairs from the source dictionary to the target dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionaries.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionaries.</typeparam>
        /// <param name="target">The target dictionary to add key-value pairs to.</param>
        /// <param name="source">The source dictionary to add key-value pairs from.</param>
        public static void Add<TKey, TValue>(this IDictionary<TKey, TValue> target, IDictionary<TKey, TValue> source)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (target == null) throw new ArgumentNullException("target");

            foreach (var keyValuePair in source)
            {
                target.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }
    }
}