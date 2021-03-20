# GodotCiv3

This is an example Godot app using C#, so Godot with mono support is required. It links to the ConvertCiv3Media library of this repo.

## Use

- In Godot, select the root Node2D scene
- If Civ III is installed, its path will be read from registry
- If not installed, create environment variable `CIV3_HOME` with the path to the root Civ III install folder
- Build and run

It currently just reads the grass terrain pcx, turns it into a sprite tile set, and places random tiles from the set in a layout like the real game.

## Building

Requires:

- Godot with mono support
- Mono
- .NET 4.7.2 SDK
