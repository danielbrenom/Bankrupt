using System.Collections.Generic;

namespace Bankrupt.Architecture
{
    public static class ListShuffler
    {
        public static void Shuffle<T> (this IList<T> list)
        {
            var n = list.Count;
            while (n > 1) {
                n--;
                var k = ThreadSafeRandom.ThisThreadsRandom.Next (n + 1);
                var value = list [k];
                list [k] = list [n];
                list [n] = value;
            }
        }
    }
}