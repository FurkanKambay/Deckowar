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
    }
}
