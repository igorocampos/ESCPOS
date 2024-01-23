[![SourceLink](https://img.shields.io/badge/SourceLink-enabled-brightgreen)](https://github.com/dotnet/sourcelink)
[![Build Status](https://dev.azure.com/igorocampos/PersonalProjects/_apis/build/status/igorocampos.ESCPOS?branchName=master)](https://dev.azure.com/igorocampos/PersonalProjects/_build?definitionId=1&_a=summary)
[![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/igorocampos/ESCPOS)](#)
[![NuGet](https://buildstats.info/nuget/ESCPOS)](http://www.nuget.org/packages/ESCPOS)
[![GitHub](https://img.shields.io/badge/license-MIT-green)](ESCPOS/LICENSE)
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
All command methods will return a **byte array** that you should concatenate with the bytes of your data, and then send it all to your printer using the `Print` extension method, which will send a byte array to the informed printer address. It can be something like `COM3`, `LPT1`, `\\127.0.0.1\printer`, `192.168.0.100:9100`, etc. or even a path to a text file like `./print.txt`.

There is also 2 extension methods, `Add` and `ToBytes`, located in the namespace `ESCPOS.Utils`. 
The first one can be used in byte arrays, so you can concatenate 2 or more byte arrays just like this:
```cs
byte[] result = array1.Add(array2, array3, ..., arrayN);
```
In addition there's an overload to the mentioned `Add` method that will accept string parameters instead of byte arrays. It appends all strings into one new string and then converts it to a byte array.
And yet another overload to accept the mix of byte arrays and strings parameters, but since it's accepting an object type parameter, this will ignore any parameter that is not `string` or `byte[]` (e.g. `int`).

With `ToBytes` method you can convert a UTF-8 string to a byte array:
```cs
byte[] result = "Some string".ToBytes();
```

Alternatively you can choose whatever Encoding you wish to use for that:
```cs
byte[] result = "汉字".ToBytes(Encoding.GetEncoding("GBK"));
```
*Just make sure your Printer has a corresponding CodePage for that Encoding!

## Examples

All examples will assume the using statements below:
```cs
using static ESCPOS.Commands;
using ESCPOS;
using ESCPOS.Utils;
```

### QRCode
```cs
byte[] qrCodeCommand = QRCode("Some data");
qrCodeCommand.Print("COM2");
```

Or using the Extension Method

```cs
string data = "Some data";
data.ToQRCode().Print("COM2");
```


### Barcode
```cs
byte[] barCodeCommand = Barcode(BarCodeType.EAN13, "9780201379624");
barCodeCommand.Print("192.168.0.100:9100");
```

Or using the Extension Method

```cs
string code = "9780201379624";
code.ToBarcode(BarCodeType.EAN13).Print("192.168.0.100:9100");
```


### Formatted Text
```cs
byte[] cmd = SelectCharSizeHeight(CharSizeHeight.Double).Add(SelectJustification(Justification.Center), "Fancy Title");
cmd.Print("\\127.0.0.1\printer");
```

### Full CFe SAT Receipt
This example will assume that the variable `cfe` is a deserialized object from the [CFe](https://portal.fazenda.sp.gov.br/servicos/sat) XML, and will print the receipt using its fields.
Also this example will print a 32 columns receipt, which is ideal for 56mm paper roll.
```cs
var line = "--------------------------------";

byte[] array = LF;
array = array.Add(SelectCharSizeHeight(CharSizeHeight.Double), SelectJustification(Justification.Center));

if (cfe.infCFe.emit.xFant != null)
    array.Add(cfe.infCFe.emit.xFant);

array.Add(LF, SelectCharSizeHeight(CharSizeHeight.Normal), cfe.infCFe.emit.xNome,
          LF, $"{cfe.infCFe.emit.enderEmit.xLgr},{cfe.infCFe.emit.enderEmit.nro} {cfe.infCFe.emit.enderEmit.xBairro} - {cfe.infCFe.emit.enderEmit.xMun} {cfe.infCFe.emit.enderEmit.CEP}",
          LF, $"CNPJ: {cfe.infCFe.emit.CNPJ}",
          LF, $"IE: {cfe.infCFe.emit.IE}",
          LF, line, SelectCharSizeHeight(CharSizeHeight.Double), $"Extrato No. {cfe.infCFe.ide.nCFe}",
          LF, "CUPOM FISCAL ELETRONICO - SAT", SelectCharSizeHeight(CharSizeHeight.Normal),
          LF, LF);

if (!string.IsNullOrEmpty(cfe.infCFe.dest?.Item))
    array.Add(line, "CPF/CNPJ do Consumidor:", cfe.infCFe.dest.Item, LF);

array.Add(line, "#|COD|DESC|QTD|UN|VL UNIT R$|(VL TRIB R$)*|VL ITEM R$",
          LF, line, SelectJustification(Justification.Left));

int i = 1;
foreach (var det in cfe.infCFe.det)
{
    string prod = $"{det.prod.cProd} {det.prod.xProd} {det.prod.qCom:0.0##} {det.prod.uCom} X {det.prod.vUnCom:0.00#} {((det.imposto?.vItem12741 ?? 0) == 0 ? "" : $"({det.imposto.vItem12741:f2})*")}";
    array.Add($" {i++:D3} ");
    while (prod.Length > 20)
    {
        var wrap = prod.Length >= 20 ? prod.Substring(0,20) : prod;
        array.Add(wrap), LF, "     ");
        prod = prod.Substring(20);
    }
    array.Add(prod.PadRight(20), det.prod.vProd.ToString("f2").PadLeft(6), LF);
}

array.Add(LF);

if (cfe.infCFe.total.ICMSTot.vDesc > 0)
    array.Add($" Desconto R${cfe.infCFe.total.ICMSTot.vDesc.ToString("f2").PadLeft(19)}", LF);

if (cfe.infCFe.total.ICMSTot.vOutro > 0)
    array.Add($" Acrescimo R${cfe.infCFe.total.ICMSTot.vOutro.ToString("f2").PadLeft(18)}", LF);

array.Add(SelectCharSizeHeight(CharSizeHeight.Double), $" TOTAL R${cfe.infCFe.total.vCFe.ToString("f2").PadLeft(22)}", LF,
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

    array.Add($" {description.PadRight(18)}{mp.vMP.ToString("f2").PadLeft(12)}", LF);
}

String accessKey = cfe.infCFe.Id.Substring(3, 44);
array.Add($" Troco{cfe.infCFe.pgto.vTroco.ToString("f2").PadLeft(25)}", LF);

foreach (var obs in cfe.infCFe.infAdic.obsFisco)
    array.Add($" {obs.xCampo}-{obs.xTexto}", LF);

array.Add(line, "OBSERVACOES DO CONTRIBUINTE", LF);
foreach (var item in cfe.infCFe.infAdic.infCpl.Split(';'))
    array.Add(item, LF);

array.Add(LF, line, SelectCharSizeHeight(CharSizeHeight.Double), SelectJustification(Justification.Center), $"SAT No. {cfe.infCFe.ide.nserieSAT}",
          LF, SelectCharSizeHeight(CharSizeHeight.Normal), DateTime.ParseExact($"{cfe.infCFe.ide.dEmi} {cfe.infCFe.ide.hEmi}", "yyyyMMdd HHmmss", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss"),
          LF, LF, SelectCharSizeHeight(CharSizeHeight.Double), accessKey,
          LF, LF, Barcode(BarCodeType.CODE128, accessKey.Substring(0, 22), 30), Barcode(BarCodeType.CODE128, accessKey.Substring(22), 30),
          LF, LF,
          QRCode($"{accessKey}|{cfe.infCFe.ide.dEmi}{cfe.infCFe.ide.hEmi}|{cfe.infCFe.total.vCFe}|{cfe.infCFe.dest?.Item ?? ""}|{cfe.infCFe.ide.assinaturaQRCODE}"),
          SelectCharSizeHeight(CharSizeHeight.Normal),
          LF, line, LF, LF, LF);

array.Print(@"\\127.0.0.1\printer");
```

# Considerations
When printing CODE128 barcodes, it will use automatically subset B, which supports numbers, upper and lower case letters and some additional characters.

You can see the changelog [here](CHANGELOG.md).


## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Figorocampos%2FESCPOS.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2Figorocampos%2FESCPOS?ref=badge_large)

