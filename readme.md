# Read Civ Data

A collection of C# class libraries for reading and converting data from 4X games, specifically Civilization III to begin with.

These libraries aren't polished, but they work to varying degrees.

## Projects in this repo

- ConvertCiv3Media - Library to read in particular Civ3 PCX and FLC files
- LuaCiv3 - A Moonsharp Lua interface to QueryCiv3Sav (under construction)
- QueryCiv3Sav - Library to extract data from Civ3 SAV and BIQ files based on offsets from section headers
- UtilsCiv3 - Common functions (currently just fetching Civ3 home path)
- Blast - This is an Apache-2.0-licensed submodule used by QueryCiv3Sav to read compressed Civ3 files

## .NET Versions Note

I am relatively new to .NET, and very new to managing different versions and runtimes. I am using dotnetcore with a netstandard2.0 specification while developing and to make command line utilities and testing libraries, but I am also using the code in Godot Mono which uses Mono and .NET 4.x specification. So until I figure out how to properly juggle the versions and runtimes, things may seem inconsistent.

Until Godot Mono supports .NET 5, I suspect the libraries need to stay targeted to netstandard2.0.
