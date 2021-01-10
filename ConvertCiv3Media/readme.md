# ReadCiv3Data.ConvertCiv3Media

A class library assembly to convert from Civ3 media formats to a newer format. Probably Unity or Godot assets, but as a standin will use PNG.

Uses [ImageSharp](https://github.com/SixLabors/ImageSharp) Apache-2.0-licensed library for pixel format and PNG output.

The parameters and output formats **will** change. Likely.

## Status

Under Construction.

The PCX reader works fairly well, but there is no accounting for unit civ colors.

The Flic reader mostly works but needs to better understsnd the Civ3-specific format changes and handle Civ3 color palettes properly.

## Objects

### Pcx

Example: `Pcx MyImage = new Pcx(@"path/to/file.pcx");`

### Properties

- `int Width` - Width of the image
- `int Height` - Height of the image
- `byte[,] Palette` - A 256x3 byte array of 256 colors in red, green, blue order
- `byte[] Image` - A flat byte array of the image data. Each byte is a pixel, the value of which is the index of the Palette indicating the color

### Methods

- Can pass a path string to the constructor and it will run Load() on creation
- `void Load(string path)` - Loads and decodes the PCX file at the path location and populates the object properties

## Functions

- Flic.Read - static - Currently takes a byte array of a Flic file and returns an SixLabors.ImageSharp.Image\<Rgba32\>[] of the converted image sequence. It works pretty fantastic on Civ3 units, and it works against other Civ3 Flics, but the colors are funny. Also I'm currently hard-coding unit shadow values which don't apply to non-unit Flics.
  - I have learned Civ3 Flics kind of do their own thing, especially with color palettes, so there is some work to do to get the colors right, get all frames, separate them into direction groups, and allow for changing the civ colors after conversion.

## Build

ImageSharp is required, but the build process should fetch it via NuGet.

- Uses dotnet core 3.1 cli
- CD to this folder and `dotnet build`
- Files will be in obj/Debug/
- To use dll in another project, cd into that projects directory and `dotnet add reference path/to/ReadCivData.ConvertCiv3Media` and reference the namespace `ReadCivData.ConvertCiv3Media` in `using` or as a prefix to the functions.