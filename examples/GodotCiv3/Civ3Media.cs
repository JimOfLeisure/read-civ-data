using Godot;
using System;
using System.Collections;
using ReadCivData.ConvertCiv3Media;

public class Civ3Media : Node2D
{
    public string Civ3Path;
    [Export()]
    public byte UnitColor;
    int[,] Map;
    Hashtable Terrmask = new Hashtable();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Civ3Path = Util.GetCiv3Path();
        // GD.Print(Civ3Path);
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

        int mywidth = 14, myheight = 18;
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
                OutImage.SetPixel(i % width, i / width, Color.Color8(0,0,0, (byte)((255 -ba[i]) * 16)));
                // using the palette color but adding transparency
                // OutImage.SetPixel(i % width, i / width, Color.Color8(palette[ba[i],0], palette[ba[i],1], palette[ba[i],2], (byte)((255 -ba[i]) * 16)));
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
        for (int y = 0; y < 9; y++) {
        Civ3Unit MyUnit = new Civ3Unit(Civ3Path + "/Art/Units/Samurai/Samurai.INI", UnitColor);
            for (int x=0; x < 7; x++) {
                // AnimatedSprite foo = (AnimatedSprite)MyUnit.AS.Duplicate();
                Civ3Unit DupUnit = new Civ3Unit(MyUnit);
                AddChild(DupUnit.AS);
                DupUnit.AS.Position = new Vector2(128 * x + 64 + 64 * (y % 2), 64 * y + 44);
                // DupUnit.AS.Scale = new Vector2((x+1) * (float)0.5, (x+1) * (float)0.5);

                // Random direction
                Direction dir = (Direction)((new Random()).Next() % (Enum.GetValues(typeof(Direction)).Length));
                // Direction dir = Direction.S;

                // keep trying random actions until the action/direction combo exists for unit (warriors don't build roads)
                // UnitAction actn = (UnitAction)((new Random()).Next() % (Enum.GetValues(typeof(UnitAction)).Length));
                UnitAction actn = UnitAction.RUN;
                string actnName = actn.ToString() + "-" + dir.ToString();
                for (;!MyUnit.SF.HasAnimation(actnName);) {
                    actn = (UnitAction)((new Random()).Next() % (Enum.GetValues(typeof(UnitAction)).Length));
                    actnName = actn.ToString() + "-" + dir.ToString();
                }
                // DupUnit.AS.Play(actn.ToString() + "-" + dir.ToString());
                DupUnit.Animation(actn, dir);
                DupUnit.Move(dir);
            }
        }
    }
    // TODO: Animation speed and movement speed should have ideally a 1:1 relationship
    public class Civ3Unit : Civ3UnitSprite {
        public MovingSprite AS;
        public SpriteFrames SF;
        // constructor to copy existing unit
        public Civ3Unit(Civ3Unit civ3Unit) : base(civ3Unit) {
            this.AS = (MovingSprite)civ3Unit.AS.Duplicate();
            this.SF = (SpriteFrames)civ3Unit.SF.Duplicate();
            this.AS.Frames = this.SF;
        }
        public Civ3Unit(string path, byte unitColor = 0) : base(path, unitColor) {
            AS = new MovingSprite();
            AS.Position = new Vector2(128 * 5, 64 * 3 - 12);
            // temporarily making it bigger
            // AS.Scale = new Vector2(2, 2);
            SF = new SpriteFrames();
            AS.Frames = SF;
            // TODO: Loop through animations and create sprites
            foreach (UnitAction actn in Enum.GetValues(typeof(UnitAction))) {
                // Ensuring there is image data for this action
                if (Animations[(int)actn] != null) {
                    foreach (Direction dir in Enum.GetValues(typeof(Direction))) {
                        string ActionAndDirection = String.Format("{0}-{1}", actn.ToString(), dir.ToString());
                        SF.AddAnimation(ActionAndDirection);
                        SF.SetAnimationSpeed(ActionAndDirection, 10);

                        for (int i = 0; i < Animations[(int)actn].Images.GetLength(1); i++) {
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
        public override void Animation(UnitAction action, Direction direction) {
            string actnName = action.ToString() + "-" + direction.ToString();
            if (!SF.HasAnimation(actnName)) {
                throw new ApplicationException(String.Format("Animation does not exist for {0} action and {1} direction", action.ToString(), direction.ToString()));
            }
            AS.Play(actnName);
        }
        public override void Move(Direction direction, float speed = (float)0.6) {
            switch (direction)
            {
                case Direction.SW:
                    AS.Velocity = new Vector2(-1, 1).Normalized() * speed;
                    break;
                case Direction.S:
                    AS.Velocity = new Vector2(0, 1) * speed;
                    break;
                case Direction.SE:
                    AS.Velocity = new Vector2(1, 1).Normalized() * speed;
                    break;
                case Direction.E:
                    AS.Velocity = new Vector2(1, 0) * speed;
                    break;
                case Direction.NE:
                    AS.Velocity = new Vector2(1, -1).Normalized() * speed;
                    break;
                case Direction.N:
                    AS.Velocity = new Vector2(0, -1) * speed;
                    break;
                case Direction.NW:
                    AS.Velocity = new Vector2(-1, -1).Normalized() * speed;
                    break;
                case Direction.W:
                    AS.Velocity = new Vector2(-1, 0) * speed;
                    break;
            }
        }
    }
    // This is just to add some movement to an AnimatedSprite
    public class MovingSprite : AnimatedSprite {
        public Vector2 Velocity = new Vector2(0, 0);
        public override void _PhysicsProcess(float delta) {
            Position = Position + Velocity;
            if (Position.x > 1040) {
                Position = new Vector2(-30, Position.y);
            }
            if (Position.x < -30) {
                Position = new Vector2(1040, Position.y);
            }
            if (Position.y > 800) {
                Position = new Vector2(Position.x, -30);
            }
            if (Position.y < -30) {
                Position = new Vector2(Position.x, 800);
            }
        }
    }
}

public class Util
{
    static public string GetCiv3Path()
    {
        // Use CIV3_HOME env var if present
        string path = System.Environment.GetEnvironmentVariable("CIV3_HOME");
        if (path != null) return path;

        // Look up in Windows registry if present
        path = Civ3PathFromRegistry("");
        if (path != "") return path;

        // TODO: Maybe check an array of hard-coded paths during dev time?
        return "/civ3/path/not/found";
    }

    static public string Civ3PathFromRegistry(string defaultPath = "D:/Civilization III")
    {
        // Assuming 64-bit platform, get vanilla Civ3 install folder from registry
        return (string)Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Infogrames Interactive\Civilization III", "install_path", defaultPath);
    }
}