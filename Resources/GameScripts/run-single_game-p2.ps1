$localname = hostname
$address = Test-Connection $localname -count 1 | select Ipv6Address | ft -HideTableHeaders | Out-String
$address = $address.Trim()
$CSAppPath = "..\..\CommunicationServer.App\bin\Debug\netcoreapp2.0\CommunicationServer.App.dll"
$GMAppPath = "..\..\GameMaster.App\bin\Debug\netcoreapp2.0\GameMaster.App.dll"
$PlayerAppPath = "..\..\Player.App\bin\Debug\netcoreapp2.0\Player.App.dll"
$gameConfigFilePath = "..\GameConfigs\run-single_game-p2.xml"
$playersCount = 2
$portNumber = 11000


Start-Process -FilePath "dotnet" -ArgumentList  "$CSAppPath --port $portNumber --conf $gameConfigFilePath" 
Start-Sleep -s 1
Start-Process -FilePath "dotnet" -ArgumentList  "$GMAppPath --port $portNumber --conf $gameConfigFilePath --address $address --game game"
Start-Sleep -s 1


For ($i=0; $i -le ($playersCount - 1); $i++) 
{
    Start-Process -FilePath "dotnet" -ArgumentList  "$PlayerAppPath --port $portNumber --conf $gameConfigFilePath --address $address --game game --team blue --role leader"
}