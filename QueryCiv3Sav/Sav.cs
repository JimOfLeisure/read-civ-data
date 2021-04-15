using System;

namespace ReadCivData.QueryCiv3Sav {
    public class SavData
    {
        public BicData Bic;
        public Civ3File SavFile;
        public GameSection Game;
        public SavData(string path)
        {
            SavFile = new Civ3File();
            SavFile.Load(path);
            Game = new GameSection(SavFile);
        }
        public class GameSection
        {
            private int Offset;
            private Civ3File SavFile;
            public GameSection(Civ3File save)
            {
                // TODO: Separate Sav and Bic data–or at least the pointers–in QueryCiv3
                SavFile = save;
                // TODO: Change nth to 1 after separating BIC and SAV data
                Offset = SavFile.SectionOffset("GAME", 2);
            }
            public int CityCount
            { get {
                    return SavFile.ReadInt32(Offset+32);
            }}
        }
    }
}