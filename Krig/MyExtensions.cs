using System.Collections.Generic;
using System;

namespace MyExtensions
{
    static class ListExtensions
    {
        static readonly Random rnd = new Random();


        public static void Shuffle<T>(this IList<T> list)
        {
            // Algorithm from stack overflow
            // https://stackoverflow.com/questions/273313/randomize-a-listt
            // algorithm called "Fisher–Yates shuffle"
            for (var i = list.Count; i > 0; i--)
                list.Swap(i-1, rnd.Next(0, i));
        }

        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}

