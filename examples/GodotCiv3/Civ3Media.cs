using Godot;
using System;
using ReadCivData.ConvertCiv3Media;

public class Civ3Media : Node2D
{
    [Export(PropertyHint.Dir)]
    public string Civ3Path;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print(Civ3Path);
        GD.Randomize();
        this.TerrainPlay();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void TerrainPlay()
    {
        string path = this.Civ3Path + "/Art/Terrain/xpgc.pcx";
        Pcx TerrainTexture = new Pcx(path);
        GD.Print(TerrainTexture.Height);
    }
}
