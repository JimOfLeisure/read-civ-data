using System;
using MoonSharp.Interpreter;
using ReadCivData.QueryCiv3Sav;

namespace ReadCivData.LuaCiv3 {
    public class Civ3Script : Script
    {
        private string Civ3Path;
        private SavData SavFile;
        public Civ3Script(string path) : base()
        {
            SavFile = new SavData(path);
            Globals["install_path"] = ReadCivData.UtilsCiv3.Util.GetCiv3Path();
        }
    }
        
}