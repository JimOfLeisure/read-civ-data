using System;

namespace ReadCivData.QueryCiv3Sav
{
    public class BicData
    {
        public Civ3File Bic;
        public BicData(string bicPath = "/Users/jim/civ3/Conquests/Conquests.biq")
        {
            // TODO: auto load default biq only if biq not in sav
            Bic = new Civ3File();
            Bic.Load(bicPath);
        }
    }
}