using System;
using System.Collections.Generic;

namespace CellEngine.Utils
{
    public static class Extensions
    {
        public static T RandomElement<T>(this IEnumerable<T> source, Random rng)
        {
            T current = default(T);
            int count = 0;
            foreach (T element in source)
            {
                count++;
                if (rng.Next(count) == 0)
                {
                    current = element;
                }
            }
            return current;
        }
    }
}