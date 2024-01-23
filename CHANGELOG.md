# v1.3.0
- Methods PrintBarcode and PrintQRCode are obsolete and will thow error if used (see #29)
- Updated Tests to .NET 8

# v1.2.4
- Renamed methods PrintBarcode and PrintQRCode and marked old ones as obsolete (see #29)
- Added support for any Encoding instead of just UTF-8
- Added Simple Commands as alias of existing ones (see #30)
- Created Extension Methods for Barcode and QRCode (see #31)

# v1.2.3
- Added new optional parameter for Barcode command (see [#26](https://github.com/igorocampos/ESCPOS/issues/26)).
- Added support for GitHub Codespaces
- Added Dockerfile and docker-compose to the project
- Updated Tests to .NET 6

# v1.2.2
- Fixed SourceLink.
- Added new unit tests for extension method `Add`.

# v1.2.1
- Enabled SourceLink.

# v1.2.0
- New Utility overload for extension method `Add`, accepting string parameters instead of byte arrays. This will prevent a lot of `.ToBytes()` in the code.
- New `SelectCharSize` method that combines Width and Height size of characters (see [#10](https://github.com/igorocampos/ESCPOS/issues/10)).

# v1.1.2
- Bug fixed in `PrintBarCode` method when using CODE128 barcodes (see [#9](https://github.com/igorocampos/ESCPOS/issues/9)).

# v1.1.1
- `Print` method now allows a network address (host:port) as a printer address.

# v1.1.0
- Moved `Print` method to `Commands.cs`.
- Moved all enums to `Enums.cs`.

# v1.0.3
- No relevant changed.
- `AssemblyVersion` was corrected.

# v1.0.2
- Renamed `QRCodeCorrection` enum.
- Created `Add` extension method for multiple strings as parameters.
- `PrintQRCode` method now has default values in all parameters but content.
- Created `Print` extension method for byte array that sends data to a printer address.
- Created `ToBytes` extension method to convert UTF-8 strings into a byte array.
- Add Unit Tests project.

# v1.0.1
- First functional version.
- No Test Project is available yet.
