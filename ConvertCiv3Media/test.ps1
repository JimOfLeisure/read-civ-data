Add-Type  -Path '/Users/jim/.nuget/packages/sixlabors.imagesharp/1.0.2/lib/netstandard2.0/SixLabors.ImageSharp.dll'
Add-Type  -Path 'bin/Debug/netstandard2.0/ReadCivData.ConvertCiv3Media.dll'

# [ReadCivData.ConvertCiv3Media.Flic]::SaveOutPngs()
# [ReadCivData.ConvertCiv3Media.Pcx]::SaveOutPng()

$foo = New-Object ReadCivData.ConvertCiv3Media.Pcx

$foo.Palette;
