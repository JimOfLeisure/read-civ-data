# ReadCivData.QueryCiv3Sav

QueryCiv3Sav is an assembly designed to read a Civilization III SAV or BIQ file, identify offsets of the 4-character section headers, and retrieve data based on offsets in the data stream.

It is a C# port of my [Go library in c3sat](https://github.com/myjimnelson/c3sat/tree/master/queryciv3).

QueryCiv3Sav is MIT licenced but relies on Blast, an Apache-2.0-licensed shared library for decompressing SAV and BIQ files.

## Status

The core works. It will read in a SAV or BIQ file in byte array format, index the 'section header' strings, and then you can query data from an offset of the nth occurnce of a section header string.

I am not planning on immediately implementing all the detailed decoding I've done in the Go library until I figure out the final form and use of the extracted data.

## Build

- If cloned via git, `git submodule init` and then `git submodule update` to fetch the Blast code
- If not cloned via git, copy the [Blast repo](https://github.com/jamestelfer/Blast) into ../Blast folder.
- cd into this folder
- With dotnet core cli 3.1, `dotnet build`

## Example Use

```cs
using System;
using ReadCivData.QueryCiv3Sav;

class Program {
    static void Main(string[] args) {
        Civ3File SaveData = new Civ3File();
        SaveData.Load(@"PATH/TO/FILE.SAV");

        int Offset = SaveData.SectionOffset("WRLD", 2) + 8;
        int WorldHeight = SaveData.ReadInt32(Offset);
        int WorldWidth = SaveData.ReadInt32(Offset + 5*4);
        Console.WriteLine("World Size:");
        Console.WriteLine(WorldWidth + " x " + WorldHeight);        
    }
}
```

## Methods

- `Civ3File.Load(string pathName)` - Pass it a path to a Civ3 SAV or BIQ file, and it will load the file data into the class instance and index the section header strings.
- `Civ3File.SectionOffset(string name, int nth)` - Returns an int offset of the nth (1, 2, 3, etc.) occurrence of the named section header (GAME, TILE, CITY, etc.).
- `Civ3File.ReadInt32(int offset, bool signed = true)` - Returns an int containing the 32-bit integer at the provided offset.
- `Civ3File.ReadInt16(int offset, bool signed = false)` - Returns an int containing the 16-bit integer at the provided offset.
- `Civ3File.ReadByte(int offset)` - Returns an int containing the 8-bit integer at the provided offset.
