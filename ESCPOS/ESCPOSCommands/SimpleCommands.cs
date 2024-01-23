using ESCPOS.Utils;

namespace ESCPOS
{
    public static partial class Commands
    {
        /// <summary>
        /// Moves the print position to the next tab position.
        /// </summary>
        /// <remarks>
        /// ·This command is ignored unless the next tab position has been set.
        /// ·If the next horizontal tab position exceeds the printing area, the printer sets the printing position to[Printing area width + 1].
        /// ·If this command is received when the printing position is at [printing area width +1], the printer executes print buffer-full printing of the current line and horizontal tab processing from the beginning of the next line.
        /// ·The default setting of the horizontal tab position for the paper roll is font A (12 x 24) every 8th character(9th, 17th, 25th, … column).
        /// </remarks>
        public static byte[] HorizontalTab => new byte[] { 0x09 };

        /// <summary>
        /// Prints the data in the print buffer and feeds one line based on the current line spacing.
        /// </summary>
        /// <remarks>
        /// ·This command sets the print position to the beginning of the line.
        /// </remarks>
        public static byte[] LineFeed => new byte[] { 0x0A };

        /// <summary>
        /// When automatic line feed is enabled, this command functions the same as LF, when automatic line feed is disabled, this command is ignored
        /// </summary>
        /// <remarks>
        /// ·Sets the print starting position to the beginning of the line.
        /// ·The automatic line feed is ignored.
        /// </remarks>
        public static byte[] CarriageReturn => new byte[] { 0x0D };

        /// <summary>
        /// Prints the data in the print buffer and returns to standard mode.
        /// </summary>
        /// <remarks>
        /// ·The buffer data is deleted after being printed.
        /// ·The printer does not execute paper cutting.
        /// ·This command sets the print position to the beginning of the line.
        /// ·This command is enabled only in page mode.
        /// </remarks>
        public static byte[] PrintAndReturnToStandardMode => new byte[] { 0x0C };

        /// <summary>
        /// In page mode, delete all the print data in the current printable area.
        /// </summary>
        /// <remarks>
        /// ·This command is enabled only in page mode.
        /// ·If data that existed in the previously specified printable area also exists in the currently specified printable area, it is deleted.
        /// </remarks>
        public static byte[] CancelPrint => new byte[] { 0x18 };

        /// <summary>
        /// ESC @
        /// </summary>
        public static byte[] InitializePrinter => new byte[] { 0x1B, 0x40 };

        /// <summary>
        /// ESC p m t1 t2
        /// </summary>
        public static byte[] OpenDrawer => new byte[] { 0x1B, 0x70, 0x00, 0x3C, 0x78 };

        /// <summary>
        /// ESC m
        /// </summary>
        public static byte[] PaperCut => new byte[] { 0x1B, 0x6D };

        /// <summary>
        /// ESC i
        /// </summary>
        public static byte[] FullPaperCut => new byte[] { 0x1B, 0x69 };

        //Alias
        public static byte[] HT => HorizontalTab;
        public static byte[] LF => LineFeed;
        public static byte[] CR => CarriageReturn;
        public static byte[] FF => PrintAndReturnToStandardMode;
        public static byte[] CAN => CancelPrint;

        public static byte[] PrintModeReset => SelectPrintMode(PrintMode.Reset);
        public static byte[] PrintModeFontB => SelectPrintMode(PrintMode.FontB);
        public static byte[] PrintModeEmphasis => SelectPrintMode(PrintMode.EmphasizedOn);
        
        public static byte[] UnderlineModeOff => SelectUnderlineMode(UnderlineMode.Off);
        public static byte[] UnderlineModeOn => SelectUnderlineMode(UnderlineMode.OneDotThick);
        
        public static byte[] DoubleStrikeOn => DoubleStrike(OnOff.On);
        public static byte[] DoubleStrikeOff => DoubleStrike(OnOff.Off);
        
        public static byte[] UseFontA => SelectCharacterFont(Font.A);
        public static byte[] UseFontB => SelectCharacterFont(Font.B);

        public static byte[] AlignToLeft => SelectJustification(Justification.Left);
        public static byte[] AlignToRight => SelectJustification(Justification.Right);
        public static byte[] AlignToCenter => SelectJustification(Justification.Center);

        public static byte[] CharSizeDoubleHeight => SelectCharSize(CharSizeWidth.Normal, CharSizeHeight.Double);
        public static byte[] CharSizeDoubleWidth => SelectCharSize(CharSizeWidth.Double, CharSizeHeight.Normal);
        public static byte[] CharSizeDoubleHeightAndWidth => SelectCharSize(CharSizeWidth.Double, CharSizeHeight.Double);
        public static byte[] CharSizeReset => SelectCharSize(CharSizeWidth.Normal, CharSizeHeight.Normal);

        public static byte[] HRIAboveBarcode => SelectHRIPosition(HRIPosition.AboveBarcode);
        public static byte[] HRIBelowBarcode => SelectHRIPosition(HRIPosition.BelowBarcode);
        public static byte[] HRINotPrinted => SelectHRIPosition(HRIPosition.NotPrinted);
    }
}
