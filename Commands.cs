using System;

namespace ESCPOS
{
    public static class Commands
    {
        #region Simple Commands
        /// <summary>
        /// Moves the print position to the next tab position.
        /// </summary>
        /// <remarks>
        /// ·This command is ignored unless the next tab position has been set.
        /// ·If the next horizontal tab position exceeds the printing area, the printer sets the printing position to[Printing area width + 1].        /// ·If this command is received when the printing position is at [printing area width +1], the printer executes print buffer-full printing of the current line and horizontal tab processing from the beginning of the next line.
        /// ·The default setting of the horizontal tab position for the paper roll is font A (12 x 24) every 8th character(9th, 17th, 25th, … column).        /// </remarks>
        public static byte[] HorizontalTab => new byte[] { 0x09 };

        /// <summary>
        ///Prints the data in the print buffer and feeds one line based on the current line spacing.        /// </summary>
        /// <remarks>
        /// ·This command sets the print position to the beginning of the line.        /// </remarks>
        public static byte[] LineFeed => new byte[] { 0x0A };

        /// <summary>
        ///When automatic line feed is enabled, this command functions the same as LF, when automatic line feed is disabled, this command is ignored        /// </summary>
        /// <remarks>
        /// ·Sets the print starting position to the beginning of the line.
        /// ·The automatic line feed is ignored.        /// </remarks>
        public static byte[] CarriageReturn => new byte[] { 0x0D };

        /// <summary>
        ///Prints the data in the print buffer and returns to standard mode.        /// </summary>
        /// <remarks>
        ///·The buffer data is deleted after being printed.
        ///·The printer does not execute paper cutting.
        ///·This command sets the print position to the beginning of the line.
        ///·This command is enabled only in page mode.        /// </remarks>
        public static byte[] PrintAndReturnToStandardMode => new byte[] { 0x0C };

        /// <summary>
        ///In page mode, delete all the print data in the current printable area.        /// </summary>
        /// <remarks>
        ///·This command is enabled only in page mode.
        ///·If data that existed in the previously specified printable area also exists in the currently specified printable area, it is deleted.        /// </remarks>
        public static byte[] CancelPrint => new byte[] { 0x18 };

        //Alias
        public static byte[] HT => HorizontalTab;
        public static byte[] LF => LineFeed;
        public static byte[] CR => CarriageReturn;
        public static byte[] FF => PrintAndReturnToStandardMode;
        public static byte[] CAN => CancelPrint;
        #endregion Simple Commands

        /// <summary>
        /// ESC @
        /// </summary>
        public static byte[] InitializePrinter => new byte[] { 0x1B, 0x40 };

        /// <summary>
        /// ESC ! n
        /// </summary>
        public static byte[] SelectPrintMode(PrintMode printMode)
            => new byte[] { 0x1B, 0x21, (byte)printMode };

        /// <summary>
        /// ESC - n
        /// </summary>
        public static byte[] SelectUnderlineMode(UnderlineMode underlineMode)
            => new byte[] { 0x1B, 0x2D, (byte)underlineMode };

        /// <summary>
        /// ESC 2 / ESC 3 n
        /// </summary>
        public static byte[] SelectLineSpacing(LineSpacing lineSpacing)
            => lineSpacing == LineSpacing.Default ? new byte[] { 0x1B, (byte)lineSpacing } : new byte[] { 0x1B, 0x33, (byte)lineSpacing };

        /// <summary>
        /// ESC G n
        /// </summary>
        public static byte[] DoubleStrike(OnOff @switch)
            => new byte[] { 0x1B, 0x47, (byte)@switch };
    }

    public enum OnOff
    {
        Off,
        On
    }
    public enum PrintMode
    {
        Reset = 0,
        FontB,
        EmphasizedOn = 8,
        DoubleHeight = 16,
        DoubleWidth = 32,
        UnderlineOn = 128
    }

    public enum UnderlineMode
    {
        Off = 0,
        OneDotThick,
        TwoDotsThick
    }

    public enum LineSpacing
    {
        Default = 2,
        Double = 4,
        Triple = 6
    }

}
