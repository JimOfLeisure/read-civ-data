using Godot;
using System;
using System.Collections.Generic;
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
        GD.Print(TerrainTexture.Width);
        GD.Print(TerrainTexture.Height);
        Image GrassLandImage = new Image();
        GrassLandImage.Create(TerrainTexture.Width, TerrainTexture.Height, false, Image.Format.Rgba8);
        GrassLandImage.Lock();
        for (int i = 0; i < TerrainTexture.Width * TerrainTexture.Height; i++)
        {
            try {
            GrassLandImage.SetPixel(i % TerrainTexture.Width, i / TerrainTexture.Width, Color.Color8(TerrainTexture.Palette[TerrainTexture.Image[i],0], TerrainTexture.Palette[TerrainTexture.Image[i],1], TerrainTexture.Palette[TerrainTexture.Image[i],2]));
            } catch{ GD.Print(i % TerrainTexture.Width + " " + i / TerrainTexture.Width); }
        }
        GrassLandImage.Unlock();
        Sprite TerrSprite = new Sprite();
        ImageTexture TerrText = new ImageTexture();
        TerrText.CreateFromImage(GrassLandImage, 0);
        TerrSprite.Texture = TerrText;
        TerrSprite.Hframes = 9;
        TerrSprite.Vframes = 9;
        for (int i=0; i < 180; i++)
        {
            // TerrSprite.Position.x = (i % 9) * 128 + (64 * ((i / 9) % 2));
            // TerrSprite.Position.y = (i / 9) * 32;
            TerrSprite.Frame = (new Random()).Next() % 81;
            AddChild(TerrSprite.Duplicate());
        }
    }
}
