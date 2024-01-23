using ESCPOS.Utils;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ESCPOS
{
    public static partial class Commands
    {
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

        /// <summary>
        /// ESC M n
        /// </summary>
        public static byte[] SelectCharacterFont(Font font)
            => new byte[] { 0x1B, 0x4D, (byte)font };

        /// <summary>
        /// ESC R n
        /// </summary>
        public static byte[] SelectInternationalCharacterSet(CharSet charSet)
            => new byte[] { 0x1B, 0x52, (byte)charSet };

        /// <summary>
        /// ESC T n
        /// </summary>
        public static byte[] SelectPrintDirection(Direction direction)
            => new byte[] { 0x1B, 0x54, (byte)direction };

        /// <summary>
        /// ESC V n
        /// </summary>
        public static byte[] Turn90Degrees(ClockwiseDirection clockwiseDirection)
            => new byte[] { 0x1B, 0x56, (byte)clockwiseDirection };

        /// <summary>
        /// ESC a n
        /// </summary>
        public static byte[] SelectJustification(Justification justification)
            => new byte[] { 0x1B, 0x61, (byte)justification };

        /// <summary>
        /// ESC t n
        /// </summary>
        public static byte[] SelectCodeTable(CodeTable codeTable)
            => new byte[] { 0x1B, 0x74, (byte)codeTable };

        /// <summary>
        /// ESC { n
        /// </summary>
        public static byte[] UpsideDown(OnOff @switch)
            => new byte[] { 0x1B, 0x7B, (byte)@switch };

        /// <summary>
        /// GS ! n
        /// </summary>
        public static byte[] SelectCharSize(CharSizeWidth charSizeWidth, CharSizeHeight charSizeHeight)
        {
            var charSize = (byte)charSizeWidth | (byte)charSizeHeight;
            return new byte[] { 0x1D, 0x21, (byte)charSize };
        }

        /// <summary>
        /// GS ! n
        /// </summary>
        public static byte[] SelectCharSizeHeight(CharSizeHeight charSize)
            => new byte[] { 0x1D, 0x21, (byte)charSize };

        /// <summary>
        /// GS ! n
        /// </summary>
        public static byte[] SelectCharSizeWidth(CharSizeWidth charSize)
            => new byte[] { 0x1D, 0x21, (byte)charSize };

        /// <summary>
        /// GS H n
        /// </summary>
        public static byte[] SelectHRIPosition(HRIPosition hriPosition)
            => new byte[] { 0x1D, 0x21, (byte)hriPosition };

        /// <summary>
        /// GS k m n
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="barCode"/> is <see langword="null"/>.</exception>
        [Obsolete(nameof(PrintBarCode) + " is deprecated, please use " + nameof(Barcode) + " method instead.")]
        public static byte[] PrintBarCode(BarCodeType barcodeType, string barcode, int heightInDots = 162, BarcodeWidth barcodeWidth = BarcodeWidth.Normal)
            => Barcode(barcodeType, barcode, heightInDots, barcodeWidth);

        /// <summary>
        /// GS k m n
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="barcode"/> is <see langword="null"/>.</exception>
        public static byte[] Barcode(BarCodeType barcodeType, string barcode, int heightInDots = 162, BarcodeWidth barcodeWidth = BarcodeWidth.Normal, Encoding encoding = null)
        {
            var height = new byte[] { 0x1D, 0x68, (byte)heightInDots };
            var width = new byte[] { 0x1D, 0x77, (byte)barcodeWidth };
            var length = barcode.Length;
            var bar = (encoding ?? Encoding.UTF8).GetBytes(barcode);
            if (barcodeType == BarCodeType.CODE128)
            {
                length += 2;
                bar = new byte[] { 0x7B, 0x42 }.Add(bar); //Subset CODE B is selected for CODE128 bars
            }
            var settings = new byte[] { 0x1D, 0x6B, (byte)barcodeType, (byte)length };

            return height.Add(width, settings, bar);
        }

        public static byte[] ToBarcode(this string barcode, BarCodeType barCodeType, int heightInDots = 162, BarcodeWidth barcodeWidth = BarcodeWidth.Normal, Encoding encoding = null)
            => Barcode(barCodeType, barcode, heightInDots, barcodeWidth, encoding);

        /// <summary>
        /// GS ( k pL pH cn fn n1 n2
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="content"/> is <see langword="null"/>.</exception>
        [Obsolete(nameof(PrintQRCode) + " is deprecated, please use " + nameof(QRCode) + " method instead.")]
        public static byte[] PrintQRCode(string content, QRCodeModel qrCodeModel = QRCodeModel.Model1, QRCodeCorrection qrCodeCorrection = QRCodeCorrection.Percent7, QRCodeSize qrCodeSize = QRCodeSize.Normal)
            => QRCode(content, qrCodeModel, qrCodeCorrection, qrCodeSize);


        /// <summary>
        /// GS ( k pL pH cn fn n1 n2
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="content"/> is <see langword="null"/>.</exception>
        public static byte[] QRCode(string content, QRCodeModel qrCodeModel = QRCodeModel.Model1, QRCodeCorrection qrCodeCorrection = QRCodeCorrection.Percent7, QRCodeSize qrCodeSize = QRCodeSize.Normal, Encoding encoding = null)
        {
            var model = new byte[] { 0x1D, 0x28, 0x6B, 0x04, 0x00, 0x31, 0x41, (byte)qrCodeModel, 0x00 };
            var size = new byte[] { 0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x43, (byte)qrCodeSize };
            var errorCorrection = new byte[] { 0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x45, (byte)qrCodeCorrection };
            int num = content.Length + 3;
            int pL = num % 256;
            int pH = num / 256;
            var storeData = new byte[] { 0x1D, 0x28, 0x6B, (byte)pL, (byte)pH, 0x31, 0x50, 0x30 };
            var data = (encoding ?? Encoding.UTF8).GetBytes(content);
            var print = new byte[] { 0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x51, 0x30 };
            return model.Add(size, errorCorrection, storeData, data, print);
        }

        public static byte[] ToQRCode(this string content, QRCodeModel qrCodeModel = QRCodeModel.Model1, QRCodeCorrection qrCodeCorrection = QRCodeCorrection.Percent7, QRCodeSize qrCodeSize = QRCodeSize.Normal, Encoding encoding = null)
            => QRCode(content, qrCodeModel, qrCodeCorrection, qrCodeSize, encoding);


        /// <exception cref="ArgumentException"><paramref name="printerAddress"/> is empty, or in an unexpected format.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="printerAddress"/> is <see langword="null"/>.</exception>
        public static void Print(this byte[] data, string printerAddress)
        {
            if (printerAddress == null)
                throw new ArgumentNullException(nameof(printerAddress));

            if (printerAddress.Length == 0)
                throw new ArgumentException("Printer address can't be empty", nameof(printerAddress));

            string[] splittedAddress = printerAddress.Split(':');

            //if printerAddress is a directly connected printer without sharing address, it will be accepted as "{host}:{port}" format
            if (splittedAddress.Length == 2)
            {
                string host = splittedAddress[0];
                string port = splittedAddress[1];
                if (!int.TryParse(port, out var portNumber))
                    throw new ArgumentException($"Print address format should be {{host}}:{{port}}, but instead it is {host}:{portNumber}");

                using (Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    clientSocket.NoDelay = true;
                    clientSocket.Connect(splittedAddress[0], portNumber);
                    clientSocket.Send(data);
                }
                return;
            }

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
}
