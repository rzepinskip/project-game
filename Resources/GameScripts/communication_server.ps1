$CSAppPath = "..\..\CommunicationServer.App\bin\Debug\netcoreapp2.0\CommunicationServer.App.dll"
$gameConfigFilePath = "..\GameConfigs\run-single_game-p2.xml"
$portNumber = 11000

Start-Process -FilePath "dotnet" -ArgumentList  "$CSAppPath --port $portNumber --conf $gameConfigFilePath" 