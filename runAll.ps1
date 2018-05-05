Copy-Item ".\GameMaster.App\bin\Debug\netcoreapp2.0\Resources" -Destination ".\Resources" -Recurse -ErrorAction SilentlyContinue


Start-Process -FilePath "dotnet" -ArgumentList  ".\CommunicationServer.App\bin\Debug\netcoreapp2.0\CommunicationServer.App.dll --port 11000 --conf .\ExampleConfig.xml" 
Start-Sleep -s 4
Start-Process -FilePath "dotnet" -ArgumentList  ".\GameMaster.App\bin\Debug\netcoreapp2.0\GameMaster.App.dll"
Start-Sleep -s 2


For ($i=0; $i -le 1; $i++) 
{
    Start-Process -FilePath "dotnet" -ArgumentList  ".\Player.App\bin\Debug\netcoreapp2.0\Player.App.dll --port 11000 --conf .\ExampleConfig.xml --address 127.0.0.1 --game game --team blue --role leader"
    Start-Sleep -s 1
}
