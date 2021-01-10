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
        this.TerrainAsSprites();
        // this.TerrainAsTileMap();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void TerrainAsSprites()
    {
        string path = this.Civ3Path + "/Art/Terrain/xpgc.pcx";
        Pcx TerrainTexture = new Pcx(path);
        // Image GrassLandImage = new Image();
        Image GrassLandImage = ByteArrayToImage(TerrainTexture.Image, TerrainTexture.Palette, TerrainTexture.Width, TerrainTexture.Height);
        Sprite TerrSprite = new Sprite();
        ImageTexture TerrText = new ImageTexture();
        TerrText.CreateFromImage(GrassLandImage, 0);
        TerrSprite.Texture = TerrText;
        TerrSprite.Hframes = 9;
        TerrSprite.Vframes = 9;
        for (int i=0; i < 180; i++)
        {
            Vector2 pos = new Vector2((i % 9) * 128 + (64 * ((i / 9) % 2)),(i / 9) * 32 );
            TerrSprite.Position = pos;
            TerrSprite.Frame = (new Random()).Next() % 81;
            AddChild(TerrSprite.Duplicate());
        }
    }
    public void TerrainAsTileMap() {
        // Default values of square tiles and 64x64 size works for our needs
        // Although tiles appear isometric, they are logically laid out as a checkerboard pattern on a square grid
        TileMap TM = new TileMap();
        TileSet TS = new TileSet();
        TM.TileSet = TS;
        AddChild(TM);
    }
    Image ByteArrayToImage(byte[] ba, byte[,] palette, int width, int height) {
        Image OutImage = new Image();
        OutImage.Create(width, height, false, Image.Format.Rgba8);
        OutImage.Lock();
        for (int i = 0; i < width * height; i++)
        {
            OutImage.SetPixel(i % width, i / width, Color.Color8(palette[ba[i],0], palette[ba[i],1], palette[ba[i],2], ba[i] == 255 ? (byte)0 : (byte)255));
        }
        OutImage.Unlock();

        return OutImage;
    }
}
