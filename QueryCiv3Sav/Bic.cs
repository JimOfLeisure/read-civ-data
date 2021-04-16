using System;

namespace ReadCivData.QueryCiv3Sav
{
    public class BicData
    {
        public Civ3File Bic;
        public BicData(byte[] bicBytes)
        {
            Bic = new Civ3File();
            Bic.Load(bicBytes);
        }
        public BicData(string bicPath)
        {
            Bic = new Civ3File();
            Bic.Load(bicPath);
        }
    }
}