using System.IO;
using ESCPOS;
using ESCPOS.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static ESCPOS.Commands;
using static ESCPOSTest.StringDiffHelper;

namespace ESCPOSTest
{
    [TestClass]
    public class UnitTest
    {
        private const string TEST_FILE = "test.txt";
        private const string TEXT_DATA = "Data test with some special characters: $ñáãç*/&#@\"'^{}";

        [TestMethod]
        public void QRCode_NoParameter()
        {
            PrintQRCode(TEXT_DATA).Print(TEST_FILE);
            ShouldEqualWithDiff("\u001D(k\u0004\01A1\0\u001D(k\u0003\01C\u0004\u001D(k\u0003\01E0\u001D(k:\01P0Data test with some special characters: $ñáãç*/&#@\"'^{}\u001D(k\u0003\01Q0", File.ReadAllText(TEST_FILE));
        }

        [TestMethod]
        public void QRCode_FullParameters()
        {
            PrintQRCode(TEXT_DATA, QRCodeModel.Model2, QRCodeCorrection.Percent30, QRCodeSize.Large).Print(TEST_FILE);
            ShouldEqualWithDiff("\u001D(k\u0004\01A2\0\u001D(k\u0003\01C\u0005\u001D(k\u0003\01E3\u001D(k:\01P0Data test with some special characters: $ñáãç*/&#@\"'^{}\u001D(k\u0003\01Q0", File.ReadAllText(TEST_FILE));
        }

        [TestMethod]
        public void Barcode_EAN8()
        {
            PrintBarCode(BarCodeType.EAN8, "90311017", 52).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "\u001Dh4\u001DkD\u000890311017");
        }

        [TestMethod]
        public void Barcode_EAN13()
        {
            PrintBarCode(BarCodeType.EAN13, "9780201379624", 52).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "\u001Dh4\u001DkC\r9780201379624");
        }

        [TestMethod]
        public void Barcode_CODE128()
        {
            PrintBarCode(BarCodeType.CODE128, "ABC123456", 52).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "\u001Dh4\u001DkI\u0009ABC123456");
        }

        [TestMethod]
        public void Barcode_UPC_A()
        {
            PrintBarCode(BarCodeType.UPC_A, "72527273070", 52).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "\u001Dh4\u001DkA\u000B72527273070");
        }

        [TestMethod]
        public void DoubleHeight()
        {
            SelectCharSizeHeight(CharSizeHeight.Double).Add(TEXT_DATA.ToBytes()).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "\u001D!\u0001Data test with some special characters: $ñáãç*/&#@\"'^{}");
        }

        [TestMethod]
        public void DoubleWidth()
        {
            SelectCharSizeWidth(CharSizeWidth.Double).Add(TEXT_DATA.ToBytes()).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "\u001D!\u0010Data test with some special characters: $ñáãç*/&#@\"'^{}");
        }

        [TestMethod]
        public void AlignCenter()
        {
            SelectJustification(Justification.Center).Add(TEXT_DATA.ToBytes()).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "\u001Ba\u0001Data test with some special characters: $ñáãç*/&#@\"'^{}");
        }

        [TestMethod]
        public void AlignRight()
        {
            SelectJustification(Justification.Right).Add(TEXT_DATA.ToBytes()).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "\u001Ba\u0002Data test with some special characters: $ñáãç*/&#@\"'^{}");
        }

        [TestMethod]
        public void AlignLeft()
        {
            SelectJustification(Justification.Left).Add(TEXT_DATA.ToBytes()).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "\u001Ba\0Data test with some special characters: $ñáãç*/&#@\"'^{}");
        }
    }
}
