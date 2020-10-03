using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Winch
{
    public static class Extensions 
    {
        /// <summary>
        /// Samples every Nth value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        public static IEnumerable<T> Sample<T>(this IEnumerable<T> list, int ordinal)
        {
            return list.Where((element, index) => index % ordinal == 0);
        }

        /// <returns>The sampled list with the first and last elements</returns>
        public static IEnumerable<T> SampleBookended<T>(this IEnumerable<T> list, int ordinal)
        {
            return list.Sample(ordinal).Union(new T[]
            {
                list.First(),
                list.Last()
            });
        }

        /// <summary>
        /// Samples N evenly spaced values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="cardinal"></param>
        /// <returns></returns>
        public static IEnumerable<T> SampleCardinal<T>(this IEnumerable<T> list, int cardinal)
        {
            int ordinal = list.Count() / (cardinal - 1);
            return list.Sample(ordinal).Concat(new T[] { list.Last() });
        }
    }
}