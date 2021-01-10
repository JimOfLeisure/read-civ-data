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
        // this.TerrainAsSprites();
        this.TerrainAsTileMap();
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
        Image GrassLandImage = ByteArrayToImage(TerrainTexture.Image, TerrainTexture.Palette, TerrainTexture.Width, TerrainTexture.Height);
        Sprite TerrSprite = new Sprite();
        ImageTexture TerrText = new ImageTexture();
        // TODO: parametrize flags parameter
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
        // Although tiles appear isometric, they are logically laid out as a checkerboard pattern on a square grid
        TileMap TM = new TileMap();
        TM.CellSize = new Vector2(64,32);
        TileSet TS = new TileSet();
        TM.TileSet = TS;

        Pcx PcxTxtr = new Pcx(Civ3Path + "/Art/Terrain/xpgc.pcx");
        Image ImgTxtr = ByteArrayToImage(PcxTxtr.Image, PcxTxtr.Palette, PcxTxtr.Width, PcxTxtr.Height);
        ImageTexture Txtr = new ImageTexture();

        int id = TS.GetLastUnusedTileId();
        for (int y = 0; y < PcxTxtr.Height; y += 64) {
            for (int x = 0; x < PcxTxtr.Width; x+= 128, id++) {
                TS.CreateTile(id);
                TS.TileSetTexture(id, Txtr);
                TS.TileSetRegion(id, new Rect2(x, y, 128, 64));
            }
        }

        int mywidth = 14;
        for (int y = 0; y < mywidth; y++) {
            for (int x = 0; x < mywidth; x+=2) {
                try {
                // TM.SetCellv(new Vector2(x, y), x + y * mywidth);
                // TM.SetCellv(new Vector2(x + (y % 2), y), 0);
                TM.SetCellv(new Vector2(x + (y % 2), y), (x / 2) % TS.GetTilesIds().Count);
                } catch { GD.Print(x + y * mywidth); }
            }
        }
        GD.Print(TS.GetTilesIds());
        AddChild(TM);
    }
    // I'm failing to get the auto/atlas thing working in code
    public void TerrainAsAtlasTileMap() {
        // Although tiles appear isometric, they are logically laid out as a checkerboard pattern on a square grid
        TileMap TM = new TileMap();
        TM.CellSize = new Vector2(64,32);
        TileSet TS = new TileSet();
        TM.TileSet = TS;

        int id = TS.GetLastUnusedTileId();
        TS.CreateTile(id);
        TS.CreateTile(id+1);

        Pcx PcxTxtr = new Pcx(Civ3Path + "/Art/Terrain/xpgc.pcx");
        Image ImgTxtr = ByteArrayToImage(PcxTxtr.Image, PcxTxtr.Palette, PcxTxtr.Width, PcxTxtr.Height);
        ImageTexture Txtr = new ImageTexture();
        // TODO: parametrize flags parameter
        Txtr.CreateFromImage(ImgTxtr, 0);
        // TODO: figure out what significance id parameter is
        TS.TileSetTexture(0, Txtr);
        TS.TileSetRegion(0, new Rect2(new Vector2(0,0), new Vector2(PcxTxtr.Width, PcxTxtr.Height)));
        TS.TileSetTileMode(0, TileSet.TileMode.AtlasTile);
        TS.AutotileSetSize(0, new Vector2(128,64));
        TS.TileSetRegion(0, new Rect2(new Vector2(0,0), new Vector2(PcxTxtr.Width / 128 * 128, PcxTxtr.Height / 64 * 64)));
        GD.Print(TS.TileGetRegion(0));
        GD.Print(TS.AutotileGetSize(0));
        // TS._ForwardAtlasSubtileSelection()
        // ResourceSaver.Save("tileset.tres", TS);
        // FIXME: None of the following seems to place tiles
        // TM.SetCellv(new Vector2(10,10), 0);
        int mywidth = 14;
        for (int y = 0; y < mywidth; y++) {
            for (int x = 0; x < mywidth; x+=2) {
                try {
                // TM.SetCellv(new Vector2(x, y), x + y * mywidth);
                // TM.SetCellv(new Vector2(x + (y % 2), y), 0);
                TM.SetCellv(new Vector2(x + (y % 2), y), (x / 2) % 2);
                } catch { GD.Print(x + y * mywidth); }
            }
        }
        GD.Print(TS.GetTilesIds());
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
