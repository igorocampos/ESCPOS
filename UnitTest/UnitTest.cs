using Microsoft.VisualStudio.TestTools.UnitTesting;
using static ESCPOS.Commands;
using static UnitTest.StringDiffHelper;
using ESCPOS.Utils;
using System.IO;
using ESCPOS;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        const string TEST_FILE = "test.txt";
        const string TEXT_DATA = "Data test with some special characters: $ñáãç*/&#@\"'^{}";

        [TestMethod]
        public void QRCode_NoParameter()
        {
            PrintQRCode(TEXT_DATA).Print(TEST_FILE);
            ShouldEqualWithDiff("(k\01A1\0(k\01C(k\01E0(kE\01P0Data test with some special characters: $ñáãç*/&#@\"'^{}(k\01Q0", File.ReadAllText(TEST_FILE));
        }

        [TestMethod]
        public void QRCode_FullParameters()
        {
            PrintQRCode(TEXT_DATA, QRCodeModel.Model2, QRCodeCorrection.Percent30, QRCodeSize.Large).Print(TEST_FILE);
            ShouldEqualWithDiff("(k\01A2\0(k\01C(k\01E3(kE\01P0Data test with some special characters: $ñáãç*/&#@\"'^{}(k\01Q0", File.ReadAllText(TEST_FILE));
        }

        [TestMethod]
        public void Barcode_EAN8()
        {
            PrintBarCode(BarCodeType.EAN8, "90311017", 52).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "h4kD90311017");
        }

        [TestMethod]
        public void Barcode_EAN13()
        {
            PrintBarCode(BarCodeType.EAN13, "9780201379624", 52).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "h4kC\r9780201379624");
        }

        [TestMethod]
        public void Barcode_CODE128()
        {
            PrintBarCode(BarCodeType.CODE128, "ABC123456", 52).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "h4kI	ABC123456");
        }

        [TestMethod]
        public void Barcode_UPC_A()
        {
            PrintBarCode(BarCodeType.UPC_A, "72527273070", 52).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "h4kA72527273070");
        }

        [TestMethod]
        public void DoubleHeight()
        {
            SelectCharSizeHeight(CharSizeHeight.Double).Add(TEXT_DATA.ToBytes()).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "!Data test with some special characters: $ñáãç*/&#@\"'^{}");
        }

        [TestMethod]
        public void DoubleWidth()
        {
            SelectCharSizeWidth(CharSizeWidth.Double).Add(TEXT_DATA.ToBytes()).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "!Data test with some special characters: $ñáãç*/&#@\"'^{}");
        }

        [TestMethod]
        public void AlignCenter()
        {
            SelectJustification(Justification.Center).Add(TEXT_DATA.ToBytes()).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "aData test with some special characters: $ñáãç*/&#@\"'^{}");
        }

        [TestMethod]
        public void AlignRight()
        {
            SelectJustification(Justification.Right).Add(TEXT_DATA.ToBytes()).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "aData test with some special characters: $ñáãç*/&#@\"'^{}");
        }

        [TestMethod]
        public void AlignLeft()
        {
            SelectJustification(Justification.Left).Add(TEXT_DATA.ToBytes()).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "a\0Data test with some special characters: $ñáãç*/&#@\"'^{}");
        }

    }
}
