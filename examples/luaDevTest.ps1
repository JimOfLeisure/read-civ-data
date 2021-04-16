Add-Type -Path ('~/.nuget/packages/moonsharp/2.0.0/lib/netcore/MoonSharp.Interpreter.dll')

# Must first run `dotnet build` in the type/project folders
Add-Type -Path ('../UtilsCiv3/bin/Debug/netstandard2.0/ReadCivData.UtilsCiv3.dll')
Add-Type -Path ('../LuaCiv3/bin/Debug/netstandard2.0/ReadCivData.LuaCiv3.dll')

$Civ3Path = [ReadCivData.UtilsCiv3.Util]::GetCiv3Path()

$SavPath = "$Civ3Path/Conquests/Saves/Auto/Conquests Autosave 3950 BC.SAV"

$Lua = New-Object ReadCivData.LuaCiv3.Civ3Script($SavPath)

$Result = $Lua.DoString('print(install_path)')
$Result = $Lua.DoString('print(game.CityCount)')
