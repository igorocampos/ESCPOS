using System;
using System.Collections.Generic;
using System.Text;

namespace ESCPOS
{
    public static class Utils
    {
        public static byte[] Add(this byte[] array1, byte[] array2)
        {
            byte[] result = new byte[array1.Length + array2.Length];
            Array.Copy(array1, result, array1.Length);
            Array.Copy(array2, 0, result, array1.Length, array2.Length);
            return result;
        }
    }

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
        LevelL = 48,
        LevelM,
        LevelQ,
        LevelH
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

    public enum CharSizeWidth
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

    public enum PrintMode
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

    public enum LineSpacing
    {
        Default = 2,
        Double = 4,
        Triple = 6
    }
}
