Copy-Item ".\GameMaster.App\bin\Debug\netcoreapp2.0\Resources" -Destination ".\Resources" -Recurse -ErrorAction SilentlyContinue

$localname = hostname
$address = Test-Connection $localname -count 1 | select Ipv6Address | ft -HideTableHeaders | Out-String
$address = $address.Trim()
$gameConfigFilePath = "ExampleConfig.xml"

echo ".\GameMaster.App\bin\Debug\netcoreapp2.0\GameMaster.App.dll --port 11000 --conf $gameConfigFilePath --address $address --game game"
$address = echo $address
echo $gameConfigFilePath

Start-Process -FilePath "dotnet" -ArgumentList  ".\CommunicationServer.App\bin\Debug\netcoreapp2.0\CommunicationServer.App.dll --port 11000 --conf $gameConfigFilePath" 
Start-Sleep -s 4
Start-Process -FilePath "dotnet" -ArgumentList  ".\GameMaster.App\bin\Debug\netcoreapp2.0\GameMaster.App.dll --port 11000 --conf $gameConfigFilePath --address $address --game game"
Start-Sleep -s 2


For ($i=0; $i -le 1; $i++) 
{
    Start-Process -FilePath "dotnet" -ArgumentList  ".\Player.App\bin\Debug\netcoreapp2.0\Player.App.dll --port 11000 --conf $gameConfigFilePath --address $address --game game --team blue --role leader"
    Start-Sleep -s 1
}
