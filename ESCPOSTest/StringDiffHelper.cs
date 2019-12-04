using System;
using System.Globalization;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ESCPOSTest
{
    //For more information, please see https://haacked.com/archive/2012/01/14/comparing-strings-in-unit-tests.aspx/
    public static class StringDiffHelper
    {
        public static void ShouldEqualWithDiff(string actualValue, string expectedValue, DiffStyle diffStyle = DiffStyle.Full)
            => ShouldEqualWithDiff(actualValue, expectedValue, diffStyle, Console.Out);

        public static void ShouldEqualWithDiff(string actualValue, string expectedValue, DiffStyle diffStyle, TextWriter output)
        {
            if (actualValue is null || expectedValue is null)
            {
                Assert.AreEqual(expectedValue, actualValue);
                return;
            }

            if (actualValue.Equals(expectedValue, StringComparison.Ordinal))
                return;

            output.WriteLine("  Idx Expected  Actual");
            output.WriteLine("-------------------------");
            int maxLen = Math.Max(actualValue.Length, expectedValue.Length);
            int minLen = Math.Min(actualValue.Length, expectedValue.Length);
            for (int i = 0; i < maxLen; i++)
                if (diffStyle != DiffStyle.Minimal || i >= minLen || actualValue[i] != expectedValue[i])
                    //                                  put a mark beside a differing row                 index                         character decimal value                                             character safe string                                                    character decimal value                                                character safe string
                    output.WriteLine($"{(i < minLen && actualValue[i] == expectedValue[i] ? " " : " * ")} {i,-3} {(i < expectedValue.Length ? ((int)expectedValue[i]).ToString() : ""),-4} {(i < expectedValue.Length ? expectedValue[i].ToSafeString() : ""),-3}  {(i < actualValue.Length ? ((int)actualValue[i]).ToString() : ""),-4} {(i < actualValue.Length ? actualValue[i].ToSafeString() : ""),-3}");

            output.WriteLine();
            Assert.AreEqual(expectedValue, actualValue);
        }

        private static string ToSafeString(this char c)
        {
            if (char.IsControl(c) || char.IsWhiteSpace(c))
                return c switch
                {
                    '\r' => @"\r",
                    '\n' => @"\n",
                    '\t' => @"\t",
                    '\a' => @"\a",
                    '\v' => @"\v",
                    '\f' => @"\f",
                    _ => $"\\u{(int) c:X};"
                };

            return c.ToString(CultureInfo.InvariantCulture);
        }
    }

    public enum DiffStyle
    {
        Full,
        Minimal
    }
}
