$path = '/Volumes/minio-storage/civ3/Conquests/conquests.biq'

# Must first run `dotnet build` in the type/project folders
Add-Type -Path ('../UtilsCiv3/bin/Debug/netstandard2.0/ReadCivData.UtilsCiv3.dll')
Add-Type -Path ('../LuaCiv3/bin/Debug/netstandard2.0/ReadCivData.LuaCiv3.dll')

"Hi"

[ReadCivData.UtilsCiv3.Util]::GetCiv3Path()
[ReadCivData.LuaCiv3.Test]::Foo()
