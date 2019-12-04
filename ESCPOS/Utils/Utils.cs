using System;
#if NETCOREAPP
using System.Diagnostics.CodeAnalysis;
#endif
using System.Linq;
using System.Text;

namespace ESCPOS.Utils
{
    public static class Utils
    {
#if NETCOREAPP
         
        [return: NotNullIfNotNull("array1")]
        [return: NotNullIfNotNull("array2")]
#endif
        private static byte[]? Add(this byte[]? array1, byte[]? array2)
        {
            if (array1 is null)
                return array2;

            if (array2 is null)
                return array1;

            byte[] result = new byte[array1.Length + array2.Length];
            Array.Copy(array1, result, array1.Length);
            Array.Copy(array2, 0, result, array1.Length, array2.Length);
            return result;
        }
#if NETCOREAPP
        [return: NotNullIfNotNull("array1")]
#endif
        public static byte[]? Add(this byte[]? array1, params byte[]?[] arrays) 
            => arrays.Aggregate(array1, (current, array2) => current.Add(array2));

        public static byte[] ToBytes(this string source)
            => Encoding.UTF8.GetBytes(source);
    }
}
