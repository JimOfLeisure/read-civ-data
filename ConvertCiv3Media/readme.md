# ReadCiv3Data.ConvertCiv3Media

A class library assembly to convert from Civ3 media formats to a newer format. Probably Unity or Godot assets, but as a standin will use PNG.

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

### Flic

Example: `Flic MyImage = new Flic(@"path/to/file.flc");`

### Properties

- `int Width` - Width of the images
- `int Height` - Height of the images
- `byte[,] Palette` - A 256x3 byte array of 256 colors in red, green, blue order
- `byte[,][] Images` - An array of animations, images, each of which is a byte array of the image data. Each byte is a pixel, the value of which is the index of the Palette indicating the color

### Methods

- Can pass a path string to the constructor and it will run Load() on creation
- `void Load(string path)` - Loads and decodes the FLC file at the path location and populates the object properties

## Build

- Uses dotnet core 3.1 cli
- CD to this folder and `dotnet build`
- Files will be in obj/Debug/
- To use dll in another project, cd into that projects directory and `dotnet add reference path/to/ReadCivData.ConvertCiv3Media` and reference the namespace `ReadCivData.ConvertCiv3Media` in `using` or as a prefix to the functions.