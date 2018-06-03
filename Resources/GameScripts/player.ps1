$localname = hostname
$address = "127.0.0.1"

$PlayerAppPath = "..\..\Player.App\bin\Debug\netcoreapp2.0\Player.App.dll"
$gameConfigFilePath = "..\GameConfigs\run-single_game-p2.xml"
$playersCount = 2
$portNumber = 11000

Start-Process -FilePath "dotnet" -ArgumentList  "$PlayerAppPath --port $portNumber --conf $gameConfigFilePath --address $address --game game --team blue --role leader"
