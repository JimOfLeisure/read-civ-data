using System;
using MoonSharp.Interpreter;
using ReadCivData.QueryCiv3Sav;

namespace ReadCivData.LuaCiv3 {
    public class Civ3Script : Script
    {
        private SavData SavFile;
        public Civ3Script(string path)
        {
            SavFile = new SavData(path);
            RegisterUserData();
            SetGlobals();
        }
        private void RegisterUserData()
        {
            UserData.RegisterType<Civ3File>();
            UserData.RegisterType<SavData.GameSection>();
            UserData.RegisterType<SavData.WrldSection>();
        }
        private void SetGlobals()
        {
            Globals["install_path"] = ReadCivData.UtilsCiv3.Util.GetCiv3Path();
            Globals["sav"] = SavFile.SavFile;
            Globals["game"] = SavFile.Game;
            Globals["wrld"] = SavFile.Wrld;
        }
    }
}