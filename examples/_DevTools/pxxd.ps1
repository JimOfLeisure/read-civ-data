# pipe output of cmdlet to xxd or Format-Hex
param(
    $path = '/Volumes/minio-storage/civ3/Conquests/conquests.biq'
)
# Must first run `dotnet build` in ../../Blast/Blast
Add-Type -Path ($PSScriptRoot + '../../../Blast/Blast/bin/Debug/netstandard2.0/Blast.dll')

$FileData = [System.IO.File]::ReadAllBytes($path)
$CompressedMs = New-Object System.IO.MemoryStream($FileData, $false)
$decoder = New-Object Blast.BlastDecoder($CompressedMs, [System.Console]::OpenStandardOutput())
$decoder.Decompress()

