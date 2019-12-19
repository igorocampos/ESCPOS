[![Build Status](https://dev.azure.com/igorocampos/PersonalProjects/_apis/build/status/igorocampos.ESCPOS?branchName=master)](https://dev.azure.com/igorocampos/PersonalProjects/_build/latest?definitionId=1&branchName=master)
[![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/igorocampos/ESCPOS)](#)
[![NuGet](https://buildstats.info/nuget/ESCPOS)](http://www.nuget.org/packages/ESCPOS)
[![GitHub](https://img.shields.io/github/license/igorocampos/ESCPOS)](ESCPOS/LICENSE)
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Figorocampos%2FESCPOS.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2Figorocampos%2FESCPOS?ref=badge_shield)

# ESCPOS
A ESC/POS Printer Commands Helper.

![](https://github.com/igorocampos/ESCPOS/blob/master/ESC_POS.png)

# Installing via NuGet Package

The NuGet Package can be found [here](https://www.nuget.org/packages/ESCPOS/) and you can install it with:

```powershell
PM> Install-Package ESCPOS
```

# Usage
All command methods will return a byte array that you should concatenate with the bytes of your data, and then send it all to your printer. 

The `Print` extension method will send a byte array to the informed printer address, which can be something like `COM3`, `LPT1`, `\\127.0.0.1\printer`, etc. or even a path to a text file like `./print.txt`.

There is also an `Add` and `ToBytes` extension methods located in the namespace `ESCPOS.Utils`. 
The first one for byte arrays, you can use it to concatenate 2 or more byte arrays just like this:
```cs
byte[] result = array1.Add(array2, array3, ..., arrayN);
```

The second for strings, you can use it to convert a UTF-8 string to a byte array:
```cs
byte[] result = "Some string".ToBytes();
```
## Examples

All examples will assume the using statements below:
```cs
using static ESCPOS.Commands;
using ESCPOS;
using ESCPOS.Utils;
```

### QRCode
```cs
byte[] qrCodeCommand = PrintQRCode("Some data");
qrCodeCommand.Print("COM2");
```

### Barcode
```cs
byte[] barCodeCommand = PrintBarCode(BarCodeType.EAN13, "9780201379624");
barCodeCommand.Print("LPT1");
```

### Full CFe SAT Receipt
This example will assume that the variable `cfe` is a deserialized object from the [CFe](https://portal.fazenda.sp.gov.br/servicos/sat) XML, and will print the receipt using its fields.
Also this example will print a 32 columns receipt, which is ideal for 56mm paper roll.
```cs
var line = "--------------------------------".ToBytes();
byte[] array = null;
array.Add(LF, SelectCharSizeHeight(CharSizeHeight.Double), SelectJustification(Justification.Center));
if (cfe.infCFe.emit.xFant != null)
    array.Add(cfe.infCFe.emit.xFant.ToBytes());

array.Add(LF, SelectCharSizeHeight(CharSizeHeight.Normal), cfe.infCFe.emit.xNome.ToBytes(),
          LF, $"{cfe.infCFe.emit.enderEmit.xLgr},{cfe.infCFe.emit.enderEmit.nro} {cfe.infCFe.emit.enderEmit.xBairro} - {cfe.infCFe.emit.enderEmit.xMun} {cfe.infCFe.emit.enderEmit.CEP}".ToBytes(),
          LF, $"CNPJ: {cfe.infCFe.emit.CNPJ}".ToBytes(),
          LF, $"IE: {cfe.infCFe.emit.IE}".ToBytes(),
          LF, line, SelectCharSizeHeight(CharSizeHeight.Double), $"Extrato No. {cfe.infCFe.ide.nCFe}".ToBytes(),
          LF, "CUPOM FISCAL ELETRONICO - SAT".ToBytes(), SelectCharSizeHeight(CharSizeHeight.Normal),
          LF, LF);

if (!string.IsNullOrEmpty(cfe.infCFe.dest?.Item))
    array.Add(line, "CPF/CNPJ do Consumidor:".ToBytes(), cfe.infCFe.dest.Item.ToBytes(), LF);

array.Add(line, "#|COD|DESC|QTD|UN|VL UNIT R$|(VL TRIB R$)*|VL ITEM R$".ToBytes(),
          LF, line, SelectJustification(Justification.Left));

int i = 1;
foreach (var det in cfe.infCFe.det)
{
    string prod = $"{det.prod.cProd} {det.prod.xProd} {det.prod.qCom:0.0##} {det.prod.uCom} X {det.prod.vUnCom:0.00#} {((det.imposto?.vItem12741 ?? 0) == 0 ? "" : $"({det.imposto.vItem12741:f2})*")}";
    array.Add($" {i++:D3} ".ToBytes());
    while (prod.Length > 20)
    {
        var wrap = prod.Length >= 20 ? prod.Substring(0,20) : prod;
        array.Add(wrap).ToBytes(), LF, "     ".ToBytes());
        prod = prod.Substring(20);
    }
    array.Add(prod.PadRight(20).ToBytes(), det.prod.vProd.ToString("f2").PadLeft(6).ToBytes(), LF);
}

array.Add(LF);
if (cfe.infCFe.total.ICMSTot.vDesc > 0)
    array.Add($" Desconto R${cfe.infCFe.total.ICMSTot.vDesc.ToString("f2").PadLeft(19)}".ToBytes(), LF);

if (cfe.infCFe.total.ICMSTot.vOutro > 0)
    array.Add($" Acrescimo R${cfe.infCFe.total.ICMSTot.vOutro.ToString("f2").PadLeft(18)}".ToBytes(), LF);

array.Add(SelectCharSizeHeight(CharSizeHeight.Double), $" TOTAL R${cfe.infCFe.total.vCFe.ToString("f2").PadLeft(22)}".ToBytes(), LF,
          SelectCharSizeHeight(CharSizeHeight.Normal), LF);

foreach (var mp in cfe.infCFe.pgto.MP)
{
    string description;
    switch (Convert.ToInt32(mp.cMP ?? "1"))
    {
        case 2:
            description = "Cheque";
            break;
        case 3:
            description = "Cartao de Credito";
            break;
        case 4:
            description = "Cartao de Debito";
            break;
        case 5:
            description = "Credito na Loja";
            break;
        case 10:
            description = "Vale Alimentacao";
            break;
        case 11:
            description = "Vale Refeicao";
            break;
        case 12:
            description = "Vale Presente";
            break;
        case 13:
            description = "Vale Combustivel";
            break;
        case 14:
            description = "Duplicata Mercantil";
            break;
        case 90:
            description = "Sem Pagamento";
            break;
        default:
            description = "Dinheiro";
            break;
    }

    array.Add($" {description.PadRight(18)}{mp.vMP.ToString("f2").PadLeft(12)}".ToBytes(), LF);
}

String accessKey = cfe.infCFe.Id.Substring(3, 44);
array.Add($" Troco{cfe.infCFe.pgto.vTroco.ToString("f2").PadLeft(25)}".ToBytes(), LF);

foreach (var obs in cfe.infCFe.infAdic.obsFisco)
    array.Add($" {obs.xCampo}-{obs.xTexto}".ToBytes(), LF);

array.Add(line, "OBSERVACOES DO CONTRIBUINTE".ToBytes(), LF);
foreach (var item in cfe.infCFe.infAdic.infCpl.Split(';'))
    array.Add(item.ToBytes(), LF);

array.Add(LF, line, SelectCharSizeHeight(CharSizeHeight.Double), SelectJustification(Justification.Center), $"SAT No. {cfe.infCFe.ide.nserieSAT}".ToBytes(),
          LF, SelectCharSizeHeight(CharSizeHeight.Normal), DateTime.ParseExact($"{cfe.infCFe.ide.dEmi} {cfe.infCFe.ide.hEmi}", "yyyyMMdd HHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss").ToBytes(),
          LF, LF, SelectCharSizeHeight(CharSizeHeight.Double), accessKey.ToBytes(),
          LF, LF, PrintBarCode(BarCodeType.CODE128, accessKey.Substring(0, 22), 30), PrintBarCode(BarCodeType.CODE128, accessKey.Substring(22), 30),
          LF, LF,
          PrintQRCode($"{accessKey}|{cfe.infCFe.ide.dEmi}{cfe.infCFe.ide.hEmi}|{cfe.infCFe.total.vCFe}|{cfe.infCFe.dest?.Item ?? ""}|{cfe.infCFe.ide.assinaturaQRCODE}"),
          SelectCharSizeHeight(CharSizeHeight.Normal),
          LF, line, LF, LF, LF);

array.Print(@"\\127.0.0.1\printer");
```

# Considerations
This library will only accept UTF8 Encoding for Barcodes and QRCodes data.

You can see the changelog [here](CHANGELOG.md).


## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Figorocampos%2FESCPOS.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2Figorocampos%2FESCPOS?ref=badge_large)
