# Must first run `dotnet build` in the type/project folders
Add-Type -Path ('../UtilsCiv3/bin/Debug/netstandard2.0/ReadCivData.UtilsCiv3.dll')
Add-Type -Path ('../QueryCiv3Sav/bin/Debug/netstandard2.0/ReadCivData.QueryCiv3Sav.dll')

$Civ3Path = [ReadCivData.UtilsCiv3.Util]::GetCiv3Path()

"4000", "3950" | ForEach-Object {
    $SavPath = "${Civ3Path}/Conquests/Saves/Auto/Conquests Autosave ${PSItem} BC.SAV"
    $SavPath
    $Sav = New-Object ReadCivData.QueryCiv3Sav.SavData($SavPath)
    $Sav.Game
    # $Sav.Wrld

    <#

    $GameOffset = $Sav.SavFile.SectionOffset("GAME", 2)
    0..10 |% {
        $Offset = $PSItem * 4
        $foo = $Sav.SavFile.ReadByte($Offset + $GameOffset)
        "$Offset $foo"
    }
    #>
}

