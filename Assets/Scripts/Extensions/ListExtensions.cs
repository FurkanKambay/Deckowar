using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace FurkanKambay.Extensions
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            int n        = list.Count;
            var provider = new RNGCryptoServiceProvider();

            while (n > 1)
            {
                byte[] box = new byte[1];

                do
                    provider.GetBytes(box);
                while (!(box[0] < n * (byte.MaxValue / n)));

                int k = box[0] % n;
                n--;

                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        public static void DumpInto<T>(this ICollection<T> source, List<T> target)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (source.Count == 0)
                return;

            target.AddRange(source);
            source.Clear();
        }

        public static T TakeItemFrom<T>(this ICollection<T> target, IList<T> source, int sourceIndex)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (sourceIndex < 0 || sourceIndex >= source.Count)
                throw new ArgumentOutOfRangeException(nameof(sourceIndex));

            T movedItem = source[sourceIndex];

            source.RemoveAt(sourceIndex);
            target.Add(movedItem);

            return movedItem;
        }
    }
}
