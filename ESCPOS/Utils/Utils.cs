using System;
using System.Text;

namespace ESCPOS.Utils
{
    public static class Utils
    {
        private static byte[] Add(this byte[] array1, byte[] array2)
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

        public static byte[] Add(this byte[] array1, params object[] objects)
        {
            foreach (var obj in objects)
            {
                if (obj is byte[] array2)
                    array1 = array1.Add(array2);
                else if (obj is string str)
                    array1 = array1.Add(str.ToBytes());
            }
            return array1;
        }

        public static byte[] Add(this byte[] array1, Encoding encoding, params object[] objects)
        {
            foreach (var obj in objects)
            {
                if (obj is byte[] array2)
                    array1 = array1.Add(array2);
                else if (obj is string str)
                    array1 = array1.Add(str.ToBytes(encoding));
            }
            return array1;
        }


        public static byte[] Add(this byte[] array, params string[] strings)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string str in strings)
                sb.Append(str);

            return array.Add(sb.ToString().ToBytes());
        }

        public static byte[] Add(this byte[] array, Encoding encoding, params string[] strings)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string str in strings)
                sb.Append(str);

            return array.Add(sb.ToString().ToBytes(encoding));
        }

        public static byte[] Add(this byte[] array1, params byte[][] arrays)
        {
            foreach (byte[] array2 in arrays)
                array1 = array1.Add(array2);

            return array1;
        }

        public static byte[] ToBytes(this string source, Encoding encoding = null)
            => (encoding ?? Encoding.UTF8).GetBytes(source);

        public static string ToUTF8(this byte[] array)
            => Encoding.UTF8.GetString(array);
    }
}
