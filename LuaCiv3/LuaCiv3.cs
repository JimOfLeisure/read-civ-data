using System;
using MoonSharp.Interpreter;
using ReadCivData.QueryCiv3Sav;

namespace ReadCivData.LuaCiv3 {
    public class Civ3Script : Script
    {
        private SavData SavFile;
        public Civ3Script(string savPath)
        {
            SavFile = new SavData(savPath);
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
            Globals["sav"] = SavFile.Sav;
            Globals["game"] = SavFile.Game;
            Globals["wrld"] = SavFile.Wrld;
        }
    }
}