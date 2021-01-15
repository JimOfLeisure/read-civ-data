using Godot;
using System;
using System.Collections;
using ReadCivData.ConvertCiv3Media;

public class Civ3Media : Node2D
{
    [Export(PropertyHint.Dir)]
    public string Civ3Path;
    int[,] Map;
    Hashtable Terrmask = new Hashtable();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print(Civ3Path);
        GD.Randomize();
        // this.TerrainAsSprites();
        this.TerrainAsTileMap();
        // this.TerrainAsAtlasTileMap();
        // this.AnimatedSpritePlay();
        this.MoreUnitSpritePlay();
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
        // TM.CenteredTextures = true;
        TileSet TS = new TileSet();
        TM.TileSet = TS;

        Pcx PcxTxtr = new Pcx(Civ3Path + "/Art/Terrain/xpgc.pcx");
        Image ImgTxtr = ByteArrayToImage(PcxTxtr.Image, PcxTxtr.Palette, PcxTxtr.Width, PcxTxtr.Height);
        ImageTexture Txtr = new ImageTexture();
        Txtr.CreateFromImage(ImgTxtr, 0);

        int id = TS.GetLastUnusedTileId();
        for (int y = 0; y < PcxTxtr.Height; y += 64) {
            for (int x = 0; x < PcxTxtr.Width; x+= 128, id++) {
                TS.CreateTile(id);
                TS.TileSetTexture(id, Txtr);
                TS.TileSetRegion(id, new Rect2(x, y, 128, 64));
                // order right, bottom, left, top; 0 is plains, 1 grass, 2 coast
                Terrmask.Add(
                    ((y / 64) % 3).ToString("D3") +
                    ((y / 64) / 3 % 3).ToString("D3") +
                    ((x / 128) / 3 % 3).ToString("D3") +
                    ((x / 128) % 3).ToString("D3")
                    , id);
            }
        }

        int mywidth = 14, myheight = 14;
        Map = new int[mywidth,myheight];
        // Populate map values, 0 out terrain mask
        for (int y = 0; y < myheight; y++) {
            for (int x = 0; x < mywidth; x++) {
                // If x & y are both even or odd, terrain value; if mismatched, terrain mask init to 0
                Map[x,y] = x%2 - y%2 == 0 ? (new Random()).Next(0,3) : 0;
            }
        }
        // Loop to lookup tile ids based on terrain mask
        for (int y = 0; y < myheight; y++) {
            for (int x = (1 - (y % 2)); x < mywidth; x+=2) {
                int Top = y == 0 ? (Map[(x+1) % mywidth,y]) : (Map[x,y-1]);
                int Bottom = y == myheight - 1 ? (Map[(x+1) % mywidth,y]) : (Map[x,y+1]);
                string foo = 
                    (Map[(x+1) % mywidth,y]).ToString("D3") +
                    Bottom.ToString("D3") +
                    (Map[Mathf.Abs((x-1) % mywidth),y]).ToString("D3") +
                    Top.ToString("D3")
                ;
                try {
                // Map[x,y] = (int)Terrmask["001001001001"];
                Map[x,y] = (int)Terrmask[foo];
                } catch { GD.Print(x + "," + y + " " + foo); }
            }
        }
        // loop to place tiles, each of which contains 1/4 of 4 'real' map locations
        for (int y = 0; y < myheight; y++) {
            for (int x = 1 - (y%2); x < mywidth; x+=2) {
                // TM.SetCellv(new Vector2(x + (y % 2), y), (new Random()).Next() % TS.GetTilesIds().Count);
                // try {
                TM.SetCellv(new Vector2(x, y), Map[x,y]);
                // } catch {}
            }
        }
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
    public static Image ByteArrayToImage(byte[] ba, byte[,] palette, int width, int height, int[] transparent = null, bool shadows = false) {
        Image OutImage = new Image();
        OutImage.Create(width, height, false, Image.Format.Rgba8);
        OutImage.Lock();
        for (int i = 0; i < width * height; i++)
        {
            if (shadows && ba[i] > 239) {
                // using black and transparency
                // OutImage.SetPixel(i % width, i / width, Color.Color8(0,0,0, (byte)((255 -ba[i]) * 16)));
                // using the palette color but adding transparency
                OutImage.SetPixel(i % width, i / width, Color.Color8(palette[ba[i],0], palette[ba[i],1], palette[ba[i],2], (byte)((255 -ba[i]) * 16)));
            } else {
                OutImage.SetPixel(i % width, i / width, Color.Color8(palette[ba[i],0], palette[ba[i],1], palette[ba[i],2], ba[i] == 255 ? (byte)0 : (byte)255));
            }
        }
        OutImage.Unlock();

        return OutImage;
    }
    public void AnimatedSpritePlay() {
        AnimatedSprite AS = new AnimatedSprite();
        AS.Position = new Vector2(128 * 3, 64 * 3 - 12);
        // temporarily making it bigger
        AS.Scale = new Vector2(4, 4);
        SpriteFrames SF = new SpriteFrames();
        AS.Frames = SF;
        AddChild(AS);

        SF.AddAnimation("Run SW");
        SF.SetAnimationSpeed("Run SW", 15);

        Flic Unit = new Flic(Civ3Path + "/Art/Units/warrior/warriorRun.flc");
        Pcx UnitPal = new Pcx(Civ3Path + "/Art/Units/Palettes/ntp00.pcx");
        byte[,] CivColorUnitPal = new byte[256,3];
        for (int i = 0, foo = 64; i < 256; i++) {
            byte[,] TempPal = i < foo ?  UnitPal.Palette : Unit.Palette ;
            for (int j = 0; j < 3; j++) {
                CivColorUnitPal[i,j] = TempPal[i < foo ? i : i, j];
            }
        }
        for (int i = 0; i < Unit.Images.GetLength(1); i++) {
            Image ImgTxtr = ByteArrayToImage(
                Unit.Images[0,i],
                CivColorUnitPal,
                Unit.Width,
                Unit.Height,
                shadows: true
            );
            ImageTexture Txtr = new ImageTexture();
            // TODO: parametrize flags parameter
            Txtr.CreateFromImage(ImgTxtr, 7);
            SF.AddFrame("Run SW", Txtr);
        }
        // GD.Print(SF.GetFrameCount("Run SW"));
        // GD.Print(AS.IsPlaying());
        AS.Play("Run SW");
        // GD.Print(AS.IsPlaying());
    }
    // using an in-development non-Godot-specific object for unit media
    public void MoreUnitSpritePlay() {
        for (int y = 0; y < 6; y++) {
        Civ3Unit MyUnit = new Civ3Unit(Civ3Path + @"/Art/Units/warrior/Warrior.INI");
            for (int x=0; x < 7; x++) {
                AnimatedSprite foo = (AnimatedSprite)MyUnit.AS.Duplicate();
                AddChild(foo);
                foo.Position = new Vector2(128 * x + 64 + 64 * (y % 2), 64 * y + 64);
                Direction dir = (Direction)((new Random()).Next() % (Enum.GetValues(typeof(Direction)).Length));
                UnitAction actn = (UnitAction)((new Random()).Next() % (Enum.GetValues(typeof(UnitAction)).Length));
                string actnName = actn.ToString() + "-" + dir.ToString();
                for (;!MyUnit.SF.HasAnimation(actnName);) {

                    actn = (UnitAction)((new Random()).Next() % (Enum.GetValues(typeof(UnitAction)).Length));
                    actnName = actn.ToString() + "-" + dir.ToString();
                }
                foo.Play(actn.ToString() + "-" + dir.ToString());
            }
        }
    }
    public class Civ3Unit : Civ3UnitSprite {
        public AnimatedSprite AS;
        public SpriteFrames SF;
        public Civ3Unit(string path) : base(path) {
            GD.Print("Hi");
            GD.Print(TestInt);
            AS = new AnimatedSprite();
            AS.Position = new Vector2(128 * 5, 64 * 3 - 12);
            // temporarily making it bigger
            AS.Scale = new Vector2(2, 2);
            SF = new SpriteFrames();
            AS.Frames = SF;
            // TODO: Loop through animations and create sprites
            foreach (UnitAction actn in Enum.GetValues(typeof(UnitAction))) {
                // Ensuring there is image data for this action
                // if (Animations[(int)actn].Images[0,0].Length > 0) {
                if (Animations[(int)actn] != null) {
                    foreach (Direction dir in Enum.GetValues(typeof(Direction))) {
                        string ActionAndDirection = String.Format("{0}-{1}", actn.ToString(), dir.ToString());
                        SF.AddAnimation(ActionAndDirection);
                        SF.SetAnimationSpeed(ActionAndDirection, 15);

                        for (int i = 0; i < Animations[(int)actn].Images.GetLength(1); i++) {
                            // GD.Print(Animations[(int)actn].Images[(int)dir,i][0]);
                            // GD.Print(Animations[(int)actn].Palette[255,2]);
                            // GD.Print(Animations[(int)actn].Width);
                            // GD.Print(Animations[(int)actn].Height);
                            Image ImgTxtr = Civ3Media.ByteArrayToImage(
                                Animations[(int)actn].Images[(int)dir,i],
                                Animations[(int)actn].Palette,
                                Animations[(int)actn].Width,
                                Animations[(int)actn].Height,
                                shadows: true
                            );
                            ImageTexture Txtr = new ImageTexture();
                            // TODO: parametrize flags parameter
                            Txtr.CreateFromImage(ImgTxtr, 7);
                            SF.AddFrame(ActionAndDirection, Txtr);
                        }

                    }
                }
            }
        }
        // looks like I can do this in the constructor instead
        public override void InitDisplay() {
            //
        }
    }
}
