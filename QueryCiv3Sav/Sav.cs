using System;

namespace ReadCivData.QueryCiv3Sav
{
    public class SavData
    {
        public BicData Bic;
        public Civ3File Sav;
        public GameSection Game;
        public WrldSection Wrld;
        public SavData(string path)
        {
            Sav = new Civ3File();
            Sav.Load(path);
            Wrld = new WrldSection(this);
            Game = new GameSection(this);
        }
        public class GameSection
        {
            private int Offset;
            private SavData Data;
            public GameSection(SavData sav) //Civ3File save, int continentCount)
            // Need to know continent count for some offsets, not sure if it's somewhere in Game so grabbing it from Wrld
            {
                Data = sav;
                // TODO: Separate Sav and Bic data–or at least the pointers–in QueryCiv3
                // TODO: Change nth to 1 after separating BIC and SAV data
                Offset = Data.Sav.SectionOffset("GAME", 2);
            }
            // TODO: Return Bic sections, not IDs
            public int DifficultyID{get{ return Data.Sav.ReadInt32(Offset+20); }}
            public int UnitCount{get{ return Data.Sav.ReadInt32(Offset+28); }}
            public int CityCount{get{ return Data.Sav.ReadInt32(Offset+32); }}
        }
        public class WrldSection
        {
            private SavData Data;
            private int Offset1, Offset2, Offset3;
            public WrldSection(SavData sav)
            {
                Data = sav;
                Offset1 = Data.Sav.SectionOffset("WRLD", 1);
                Offset2 = Data.Sav.SectionOffset("WRLD", 2);
                Offset3 = Data.Sav.SectionOffset("WRLD", 3);
            }
            public int ContinentCount{get{ return Data.Sav.ReadInt16(Offset1+4); }}
            // TODO: Return Bic sections, not IDs
            public int WsizID{get{ return Data.Sav.ReadInt32(Offset1+234); }}
        }
    }
}