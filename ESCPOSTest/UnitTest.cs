using ESCPOS;
using ESCPOS.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
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
        public void Add_TwoByteArrays_FirstNull()
        {
            byte[] array1 = null;
            array1 = array1.Add(LF);
            ShouldEqualWithDiff(array1.ToUTF8(), LF.ToUTF8());
        }

        [TestMethod]
        public void Add_TwoByteArrays_SecondNull()
        {
            byte[] array2 = null;
            array2 = LF.Add(array2);
            ShouldEqualWithDiff(array2.ToUTF8(), LF.ToUTF8());
        }

        [TestMethod]
        public void Add_TwoByteArrays()
        {
            byte[] array = LF.Add(HT);
            ShouldEqualWithDiff(array.ToUTF8(), $"{LF.ToUTF8()}{HT.ToUTF8()}");
        }

        [TestMethod]
        public void Add_ThreeStrings()
        {
            byte[] array = null;
            array = array.Add(TEXT_DATA, TEXT_DATA, TEXT_DATA);
            ShouldEqualWithDiff(array.ToUTF8(), $"{TEXT_DATA}{TEXT_DATA}{TEXT_DATA}");
        }

        [TestMethod]
        public void Add_FourByteArrays()
        {
            byte[] array = LF.Add(HT, CR, FF);
            ShouldEqualWithDiff(array.ToUTF8(), $"{LF.ToUTF8()}{HT.ToUTF8()}{CR.ToUTF8()}{FF.ToUTF8()}");
        }

        [TestMethod]
        public void Add_ThreeByteArraysAndTwoStrings()
        {
            byte[] array = LF.Add(TEXT_DATA, CR, TEXT_DATA, FF);
            ShouldEqualWithDiff(array.ToUTF8(), $"{LF.ToUTF8()}{TEXT_DATA}{CR.ToUTF8()}{TEXT_DATA}{FF.ToUTF8()}");
        }

        [TestMethod]
        public void Add_IgnoringNonStringNonByteArray()
        {
            byte[] array = LF.Add(TEXT_DATA, 1, CR, DateTime.Now, 10.5, 785m, new UnitTest(), FF);
            ShouldEqualWithDiff(array.ToUTF8(), $"{LF.ToUTF8()}{TEXT_DATA}{CR.ToUTF8()}{FF.ToUTF8()}");
        }

        [TestMethod]
        public void QRCode_NoParameter()
        {
            QRCode(TEXT_DATA).Print(TEST_FILE);
            ShouldEqualWithDiff("\u001D(k\u0004\01A1\0\u001D(k\u0003\01C\u0004\u001D(k\u0003\01E0\u001D(k:\01P0Data test with some special characters: $ñáãç*/&#@\"'^{}\u001D(k\u0003\01Q0", File.ReadAllText(TEST_FILE));
        }

        [TestMethod]
        public void QRCode_FullParameters()
        {
            QRCode(TEXT_DATA, QRCodeModel.Model2, QRCodeCorrection.Percent30, QRCodeSize.Large).Print(TEST_FILE);
            ShouldEqualWithDiff("\u001D(k\u0004\01A2\0\u001D(k\u0003\01C\u0005\u001D(k\u0003\01E3\u001D(k:\01P0Data test with some special characters: $ñáãç*/&#@\"'^{}\u001D(k\u0003\01Q0", File.ReadAllText(TEST_FILE));
        }

        [TestMethod]
        public void Barcode_EAN8()
        {
            Barcode(BarCodeType.EAN8, "90311017", 52).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "\u001Dh4\u001dw\u0003\u001DkD\u000890311017");
        }

        [TestMethod]
        public void Barcode_EAN13()
        {
            Barcode(BarCodeType.EAN13, "9780201379624", 52).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "\u001Dh4\u001dw\u0003\u001DkC\r9780201379624");
        }

        [TestMethod]
        public void Barcode_CODE128()
        {
            Barcode(BarCodeType.CODE128, "ABC1234", 52).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "\u001Dh4\u001dw\u0003\u001DkI\u0009{BABC1234");
        }

        [TestMethod]
        public void Barcode_UPC_A()
        {
            Barcode(BarCodeType.UPC_A, "72527273070", 52).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "\u001Dh4\u001dw\u0003\u001DkA\u000B72527273070");
        }

        [TestMethod]
        public void DoubleHeight()
        {
            SelectCharSizeHeight(CharSizeHeight.Double).Add(TEXT_DATA).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "\u001D!\u0001Data test with some special characters: $ñáãç*/&#@\"'^{}");
        }

        [TestMethod]
        public void DoubleWidthAndHeight()
        {
            SelectCharSize(CharSizeWidth.Double, CharSizeHeight.Double).Add(TEXT_DATA).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "\u001D!\u0011Data test with some special characters: $ñáãç*/&#@\"'^{}");
        }

        [TestMethod]
        public void DoubleWidth()
        {
            SelectCharSizeWidth(CharSizeWidth.Double).Add(TEXT_DATA).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "\u001D!\u0010Data test with some special characters: $ñáãç*/&#@\"'^{}");
        }

        [TestMethod]
        public void AlignCenter()
        {
            SelectJustification(Justification.Center).Add(TEXT_DATA).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "\u001Ba\u0001Data test with some special characters: $ñáãç*/&#@\"'^{}");
        }

        [TestMethod]
        public void AlignRight()
        {
            SelectJustification(Justification.Right).Add(TEXT_DATA).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "\u001Ba\u0002Data test with some special characters: $ñáãç*/&#@\"'^{}");
        }

        [TestMethod]
        public void AlignLeft()
        {
            SelectJustification(Justification.Left).Add(TEXT_DATA).Print(TEST_FILE);
            ShouldEqualWithDiff(File.ReadAllText(TEST_FILE), "\u001Ba\0Data test with some special characters: $ñáãç*/&#@\"'^{}");
        }
    }
}
