using System;
using System.Collections.Generic;

namespace ReadCivData.QueryCiv3Sav
{
    public class SavData
    {
        public BicData Bic;
        public Civ3File Sav;
        public string DefaultBicPath;
        public GameSection Game;
        public WrldSection Wrld;
        public SavData(byte[] savBytes, string defaultBicPath)
        {
            DefaultBicPath = defaultBicPath;
            Sav = new Civ3File();
            Sav.Load(savBytes);
            Init();
        }
        public SavData(string savPath, string defaultBicPath)
        {
            DefaultBicPath = defaultBicPath;
            Sav = new Civ3File();
            Sav.Load(savPath);
            Init();
        }
        protected void Init()
        {
            if(Sav.HasCustomBic)
            {
                Bic = new BicData(Sav.CustomBic);
            }
            else
            {
                Bic = new BicData(DefaultBicPath);
            }
            Wrld = new WrldSection(this);
            Game = new GameSection(this);
        }
        public MapTile[] Tile
        { get {
            int TileOffset = Sav.SectionOffset("TILE", 1);
            int TileLength = 212;
            int TileCount = Wrld.Height * (Wrld.Width / 2);
            List<MapTile> TileList = new List<MapTile>();
            for(int i=0; i< TileCount; i++, TileOffset += TileLength) TileList.Add(new MapTile(this, TileOffset));
            return TileList.ToArray();
        }}
        public ContItem[] Cont
        { get {
            int ContCount = Wrld.ContinentCount;
            List<ContItem> LeadList = new List<ContItem>();
            for(int i=0; i< ContCount; i++)
            {
                int LeadOffset = Sav.SectionOffset("CONT", i+1);
                LeadList.Add(new ContItem(this, LeadOffset));
            }
            return LeadList.ToArray();
        }}
        public LeaderItem[] Lead
        { get {
            int LeadCount = 32;
            List<LeaderItem> LeadList = new List<LeaderItem>();
            for(int i=0; i< LeadCount; i++)
            {
                int LeadOffset = Sav.SectionOffset("LEAD", i+1);
                LeadList.Add(new LeaderItem(this, LeadOffset));
            }
            return LeadList.ToArray();
        }}
    }
}