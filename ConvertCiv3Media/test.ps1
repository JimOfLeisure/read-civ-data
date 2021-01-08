Add-Type  -Path '/Users/jim/.nuget/packages/sixlabors.imagesharp/1.0.2/lib/netstandard2.0/SixLabors.ImageSharp.dll'
Add-Type  -Path '/Users/jim/code/src/dotnet/read-civ-data/ConvertCiv3Media/bin/Debug/netstandard2.0/ReadCivData.ConvertCiv3Media.dll'

# [ReadCivData.ConvertCiv3Media.Flic]::SaveOutPngs()
# [ReadCivData.ConvertCiv3Media.Pcx]::SaveOutPng()

$foo = New-Object ReadCivData.ConvertCiv3Media.Pcx("/Users/jim/code/src/tmp/Godot/civ3play/temp/popHeads-ORIG.pcx")

$foo.Palette.Count
$foo.Image.Count
$foo.Width
$foo.Height
