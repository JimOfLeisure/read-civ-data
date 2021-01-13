$path = '/Volumes/minio-storage/civ3/Conquests/conquests.biq'

# Must first run `dotnet build` in ../../Blast/Blast
Add-Type -Path ('../Blast/Blast/bin/Debug/netstandard2.0/Blast.dll')

$FileData = [System.IO.File]::ReadAllBytes($path)
$CompressedMs = New-Object System.IO.MemoryStream($FileData, $false)
$Decompressed = New-Object System.IO.FileStream("decompressed.bin", 2)
$decoder = New-Object Blast.BlastDecoder($CompressedMs, $Decompressed)
$decoder.Decompress()
