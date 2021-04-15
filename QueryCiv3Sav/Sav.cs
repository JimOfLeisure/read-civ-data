using System;

namespace ReadCivData.QueryCiv3Sav {
    public class SavData
    {
        public BicData Bic;
        public Civ3File SavFile;
        public GameSection Game;
        public WrldSection Wrld;
        public SavData(string path)
        {
            SavFile = new Civ3File();
            SavFile.Load(path);
            Wrld = new WrldSection(SavFile);
            Game = new GameSection(SavFile, Wrld.ContinentCount);
        }
        public class GameSection
        {
            private int Offset;
            private Civ3File SavFile;
            public GameSection(Civ3File save, int continentCount)
            // Need to know continent count for some offsets, not sure if it's somewhere in Game so grabbing it from Wrld
            {
                // TODO: Separate Sav and Bic data–or at least the pointers–in QueryCiv3
                SavFile = save;
                // TODO: Change nth to 1 after separating BIC and SAV data
                Offset = SavFile.SectionOffset("GAME", 2);
            }
            // TODO: Return Bic sections, not IDs
            public int DifficultyID{get{ return SavFile.ReadInt32(Offset+20); }}
            public int UnitCount{get{ return SavFile.ReadInt32(Offset+28); }}
            public int CityCount{get{ return SavFile.ReadInt32(Offset+32); }}
        }
        public class WrldSection
        {
            private int Offset1, Offset2, Offset3;
            private Civ3File SavFile;
            public WrldSection(Civ3File save)
            {
                SavFile = save;
                Offset1 = SavFile.SectionOffset("WRLD", 1);
                Offset2 = SavFile.SectionOffset("WRLD", 2);
                Offset3 = SavFile.SectionOffset("WRLD", 3);
            }
            public int ContinentCount{get{ return SavFile.ReadInt16(Offset1+4); }}
            // TODO: Return Bic sections, not IDs
            public int WsizID{get{ return SavFile.ReadInt32(Offset1+234); }}
        }
    }
}