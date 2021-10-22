# ReadCivData.QueryCiv3Sav

QueryCiv3Sav is an assembly designed to read a Civilization III SAV or BIQ file, identify offsets of the 4-character section headers, and retrieve data based on offsets in the data stream.

It is a C# port of my [Go library in c3sat](https://github.com/myjimnelson/c3sat/tree/master/queryciv3).

QueryCiv3Sav is MIT licenced but relies on Blast, an Apache-2.0-licensed shared library for decompressing SAV and BIQ files.

## Status

The core works. It will read in a SAV or BIQ file in byte array format, index the 'section header' strings, and then you can query data from an offset of the nth occurnce of a section header string.

I am not planning on immediately implementing all the detailed decoding I've done in the Go library until I figure out the final form and use of the extracted data.

2021-10-19 Update: Returning to this after a few months away, I had to figure out where I left off. The SAV portion of the data may be up to parity with my Go code, but I am not sure of that yet. The BIC portion is reading the generic section lists and getting BLDG names, but that's it so far.

I like the extraction API which is classes for each section with a pointer to the raw data, a section offset, and getters for the data. This should allow future flexibility as I learn more of the data. Ideally eventually each section could be called with the offset where the last left off, but until then there is the sectionheader offset seek method.

The main downside of this approach is that adding new data for writing out is not easy, but that was never my plan, and if I find a need to write more data to a save–as opposed to just changing data in-place–it can be handled on a case-by-case basis, and other reference IDs and counters may need updating in the process.

2021-10-22 Update: Overthinking this code is preventing me from moving forward with short-term plans. I eventually want a way to expose the game data to user-generated script while hiding the methods to load files and change values, but it's not important short term. I also would like to have a non-spoiling interface to expose, but again not important immediately.

So I anticipate in the future defining one or more interfaces, and some utility that uses the base class today may want to use the future interface. In general I am happy with the functionality and structure of the current code, but in the future it will refactor some to allow for differently-privileged use cases for the same data and methods.

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
