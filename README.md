# ESCPOS
A ESC/POS Printer Commands Helper

# Installing via the NuGet Package
```PM> Install-Package ESCPOS -Version 1.0.0```

# Usage
All command methods will return a byte array that you should concatenate with the bytes of your data, and then send it all to your printer. 

## Examples
### QRCode
```cs
using static ESCPOS.Commands;
using ESCPOS;
.
.
.

byte[] qrCodeCommand = PrintQRCode("Some data", QRCodeModel.Model1, QRCodeCorrection.LevelM, QRCodeSize.Normal);
```

## Considerations
This library will only accept UTF8 Encoding for Barcodes and QRCodes data.


# Future features
- A method that sends a byte array to a printer.
- A extension method to help user concatenate multiple byte arrays.
