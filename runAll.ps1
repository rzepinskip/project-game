Copy-Item ".\GameMaster.App\bin\Debug\netcoreapp2.0\Resources" -Destination ".\Resources" -Recurse -ErrorAction SilentlyContinue

Start-Process -FilePath "dotnet" -ArgumentList  ".\CommunicationServer.App\bin\Debug\netcoreapp2.0\CommunicationServer.App.dll"
Start-Sleep -s 4
Start-Process -FilePath "dotnet" -ArgumentList  ".\GameMaster.App\bin\Debug\netcoreapp2.0\GameMaster.App.dll"
Start-Sleep -s 2


For ($i=0; $i -le 1; $i++) 
{
    Start-Process -FilePath "dotnet" -ArgumentList  ".\Player.App\bin\Debug\netcoreapp2.0\Player.App.dll"
    Start-Sleep -s 1
}