using ESCPOS.Utils;
using System;
using System.Text;

namespace ESCPOS
{
    public class Printer
    {
        public Encoding Encoding { get; set; }
        public int Columns { get; set; } = 42;
        public string Address { get; set; }
        public byte[] CommandCache { get; set; }

        public byte[] HorizontalLine => "".PadLeft(Columns, '-').ToBytes(Encoding);

        public byte[] HorizontalDoubleLine => "".PadLeft(Columns, '=').ToBytes(Encoding);

        /// <summary>
        /// Based on printer's column count provides a byte array for 1 full line where both texts fit, first one aligned to the left and the other to the right.
        /// If total length of both texts is greater than printer's column count, Left Aligned text will get truncated to fit both and a whitespace will be added in between
        /// Printer will have Text Alignment set to the Left afterwards
        /// </summary>
        /// <param name="leftAlignedText">Text that will be aligned to the Left</param>
        /// <param name="rightAlignedText">Text that will be aligned to the Right</param>
        /// <returns>Byte array that contains a full line with one text aligned to the left and the other to the right</returns>
        public byte[] SameLineLeftAndRightAlignedText(string leftAlignedText, string rightAlignedText)
        {
            int totalLength = leftAlignedText.Length + rightAlignedText.Length;
            if (rightAlignedText.Length > Columns)
                throw new ArgumentException($"Length of Right-Aligned text ({rightAlignedText.Length}) surpass the printer's column count ({Columns}).");

            if (rightAlignedText.Length >= Columns -1)
                return Commands.AlignToRight.Add(rightAlignedText.ToBytes(Encoding), Commands.AlignToLeft);

            string result;
            if (totalLength >= Columns)
                result = $"{leftAlignedText.Substring(0, Columns - (rightAlignedText.Length + 1))} {rightAlignedText}";
            else
            {
                int padCount = Columns - totalLength;
                result = $"{leftAlignedText}{rightAlignedText.PadLeft(padCount + rightAlignedText.Length, ' ')}";
            }
            return result.ToBytes(Encoding).Add(Commands.AlignToLeft);
        }

        /// <summary>
        /// Clears the CommandCache for this printer instance.
        /// </summary>
        public void ClearCache() 
            => CommandCache = null;

        public void AddToCache(params object[] objects)
            => CommandCache = CommandCache.Add(Encoding, objects);

        /// <summary>
        /// Sends content of CommandCache to the set Address for this printer. Once that's done CommandCache will be cleared.
        /// Printer Address can't be null empty, or in an unexpected format.
        /// </summary>
        /// 
        /// <exception cref="InvalidOperationException"><paramref name="Address"/> is null, empty, or in an unexpected format.</exception>
        public void Print()
        {
            if (CommandCache is null)
                return;

            try
            {
                CommandCache.Print(Address);
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentException)
            {
                throw new InvalidOperationException("Printer Address can't be null empty, or in an unexpected format.", ex);
            }
            ClearCache();
        }
    }
}
