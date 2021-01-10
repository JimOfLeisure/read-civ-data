# GodotCiv3

This is an example Godot app using C#, so Godot with mono support is required. It links to the ConvertCiv3Media library of this repo.

## Use

- In Godot, select the root Node2D scene
- Edit "Civ 3 Path" in the inspector under Script Varibables to point to the installed location of Civ 3
- Build and run

It currently just reads the grass terrain pcx, turns it into a sprite tile set, and places random tiles from the set in a layout like the real game.

## Building

Godot with mono support should be able to build and compile this without additional tooling.
