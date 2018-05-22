$localname = hostname
$address = "127.0.0.1"


$GMAppPath = "..\..\GameMaster.App\bin\Debug\netcoreapp2.0\GameMaster.App.dll"
$gameConfigFilePath = "..\GameConfigs\run-single_game-p2.xml"
$portNumber = 11000

Start-Process -FilePath "dotnet" -ArgumentList  "$GMAppPath --port $portNumber --conf $gameConfigFilePath --address $address --game game --visualize"