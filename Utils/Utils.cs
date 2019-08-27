using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ESCPOS.Utils
{
    public static class Utils
    {
        public static byte[] Add(this byte[] array1, byte[] array2)
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
        public static byte[] Add(this byte[] array1, params byte[][] arrays)
        {
            foreach (byte[] array2 in arrays)
                array1 = array1.Add(array2);

            return array1;
        }

        public static byte[] ToBytes(this String source)
            => Encoding.UTF8.GetBytes(source);

        public static void Print(this byte[] data, string printerAddress)
        {
            if (string.IsNullOrEmpty(printerAddress))
                throw new ArgumentException($"Printer address can't be null or empty", printerAddress);
            string tempFile = "esc_pos.temp";
            if (File.Exists(tempFile))
            {
                try
                {
                    File.Delete(tempFile);
                }
                catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
                {
                    tempFile = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_{tempFile}";
                }
            }

            File.WriteAllBytes(tempFile, data);
            File.Copy(tempFile, printerAddress, true);
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
