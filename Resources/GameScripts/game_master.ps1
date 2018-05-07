$localname = hostname
$address = Test-Connection $localname -count 1 | select Ipv6Address | ft -HideTableHeaders | Out-String
$address = $address.Trim()

$GMAppPath = "..\..\GameMaster.App\bin\Debug\netcoreapp2.0\GameMaster.App.dll"
$gameConfigFilePath = "..\GameConfigs\run-single_game-p2.xml"
$portNumber = 11000

Start-Process -FilePath "dotnet" -ArgumentList  "$GMAppPath --port $portNumber --conf $gameConfigFilePath --address $address --game game"