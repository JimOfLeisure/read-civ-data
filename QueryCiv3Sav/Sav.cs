using System;

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
    }
}