# Testing the ini-parser NuGet package against unit ini files

$NugetPath = '/Users/jim/.nuget/packages'
$Civ3Root = '/Volumes/minio-storage/civ3'
$IniPath = $Civ3Root + '/Art/Units/warrior/Warrior.INI'

Add-Type -Path ($NugetPath + '/ini-parser/3.4.0/lib/net20/INIFileParser.dll')

$parser = New-Object IniParser.FileIniDataParser
$data = $parser.ReadFile($IniPath)
$data.Sections["Animations"] | Select-Object KeyName, Value | Format-List

$foo = Add-Type -Language CSharp -TypeDefinition @"
namespace Foo {
    public class Doot {
        public enum Direction {
            SW,
            S,
            SE,
            E,
            NE,
            N,
            NW,
            W
        }
    }
}
"@ -PassThru



[Foo.Doot+Direction]::E + 100