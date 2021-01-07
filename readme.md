# Read Civ Data

A collection of C# class libraries for reading and converting data from 4X games, specifically Civilization III to begin with.

These libraries aren't polished, but they work to varying degrees.

## Projects in this repo

- ConvertCiv3Media - Library to read in particular Civ3 PCX and FLC files
- QueryCiv3Sav - Library to extract data from Civ3 SAV and BIQ files based on offsets from section headers
  - QueryCiv3Sav uses SixLabors.ImageSharp–an Apache-2.0-licensed image library–which should be automatically retrieved with NuGet when building
- Blast - This is an Apache-2.0-licensed submodule used by QueryCiv3Sav to read compressed Civ3 files
