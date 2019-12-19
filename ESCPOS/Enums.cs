namespace ESCPOS
{
    public enum OnOff
    {
        Off,
        On
    }

    public enum QRCodeModel
    {
        Model1 = 49,
        Model2
    }

    public enum QRCodeCorrection
    {
        Percent7 = 48,
        Percent15,
        Percent25,
        Percent30
    }

    public enum QRCodeSize
    {
        Tiny = 2,
        Small,
        Normal,
        Large
    }

    public enum Justification
    {
        Left,
        Center,
        Right
    }

    public enum BarCodeType
    {
        UPC_A = 65,
        UPC_E,
        EAN13,
        EAN8,
        CODE39,
        ITF,
        CODABAR,
        CODE93,
        CODE128
    }

    public enum HRIPosition
    {
        NotPrinted,
        AboveBarcode,
        BelowBarcode,
        BothAboveAndBelow
    }

    public enum CharSizeHeight
    {
        Normal,
        Double,
        Triple,
        Quadruple,
        Quintuple,
        Sextuple,
        Septuple,
        Octuple
    }

#pragma warning disable CA1027 // Mark enums with FlagsAttribute
    public enum CharSizeWidth
#pragma warning restore CA1027 // Mark enums with FlagsAttribute
    {
        Normal,
        Double = 16,
        Triple = 32,
        Quadruple = 48,
        Quintuple = 64,
        Sextuple = 80,
        Septuple = 96,
        Octuple = 112
    }

    public enum ClockwiseDirection
    {
        Counterclockwise,
        Clockwise
    }

    public enum Direction
    {
        LeftToRight,
        BottomToTop,
        RightToLeft,
        TopToBottom
    }

    public enum CharSet
    {
        USA,
        France,
        Germany,
        UK,
        DenmarkI,
        Sweden,
        Italy,
        SpainI,
        Japan,
        Norway,
        DenmarkII,
        SpainII,
        LatinAmarerica
    }

    public enum CodeTable
    {
        USA,
        Katakana,
        Multilingual,
        Portuguese,
        CanadianFrench,
        Nordic,
        Windows1252 = 16,
        Cyrillic,
        Latin2,
        OEM858,
        SpacePage = 255
    }

    public enum Font
    {
        A,
        B
    }

#pragma warning disable CA1027 // Mark enums with FlagsAttribute
    public enum PrintMode
#pragma warning restore CA1027 // Mark enums with FlagsAttribute
    {
        Reset,
        FontB,
        EmphasizedOn = 8,
        DoubleHeight = 16,
        DoubleWidth = 32,
        UnderlineOn = 128
    }

    public enum UnderlineMode
    {
        Off,
        OneDotThick,
        TwoDotsThick
    }

#pragma warning disable CA1027 // Mark enums with FlagsAttribute
    public enum LineSpacing
#pragma warning restore CA1027 // Mark enums with FlagsAttribute
    {
        Default = 2,
        Double = 4,
        Triple = 6
    }
}
