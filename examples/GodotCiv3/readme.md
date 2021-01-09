# GodotCiv3

This is an example Godot app using C#, so Godot with mono support is required. It links to the ConvertCiv3Media library of this repo.

## Use

- In Godot, select the root Node2D scene
- Edit "Civ 3 Path" in the inspector under Script Varibables to point to the installed location of Civ 3
- Build and run

It currently just reads the grass terrain pcx, turns it into a sprite tile set, and places random tiles from the set in a layout like the real game.

## Building

Godot with mono support should be able to build and compile this without additional tooling.

### For noobs like me who haven't quite figured out how to set everything up yet

My VSCode environment is dotnet core targeting .Net Standard 2.0, and Godot is Mono targeting .Net framework 4.5.1, so VSCode isn't doing auto completion for me.

I'm sure there's a better way, but if I stuff this in csproj then VSCode is happy but Godot loses its mind. Eventually by deleting this and the .mono directory and restarting everything Godot might be ok again.

```xml
  <ItemGroup>
    <PackageReference Include="Microsoft.NETFramework.ReferenceAssemblies" Version="1.0.0" PrivateAssets="All" />
  </ItemGroup>
```
