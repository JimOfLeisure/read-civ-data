using System;
using MoonSharp.Interpreter;
using ReadCivData.QueryCiv3Sav;
using ReadCivData.UtilsCiv3;

namespace ReadCivData.LuaCiv3 {
    // Passthrough so calling program doesn't need to use MoonSharp namespace; also sandboxes by default
    public class Script : MoonSharp.Interpreter.Script {
        // Default script environment is hard sandbox; see https://www.moonsharp.org/sandbox.html#removing-dangerous-apis
        public Script(CoreModules coreModules = CoreModules.Preset_HardSandbox) : base(coreModules) {
            RegisterQueryCiv3Types();
        }
        public void LoadCiv3(byte[] savBytes, byte[] defaultBicBytes) {
            SavData savFile = new SavData(savBytes, defaultBicBytes);

            Globals["civ3"] = savFile;
        }
        // Enables these type instances to be accssed directly by Lua
        private void RegisterQueryCiv3Types() {
            UserData.RegisterType<SavData>();
            UserData.RegisterType<BicData>();
            UserData.RegisterType<Civ3File>();
            UserData.RegisterType<GameSection>();
            UserData.RegisterType<WrldSection>();
            UserData.RegisterType<MapTile>();
            UserData.RegisterType<ContItem>();
            UserData.RegisterType<LeaderItem>();
            UserData.RegisterType<CityItem>();
            
            UserData.RegisterType<BldgSection>();
            UserData.RegisterType<CtznSection>();
            UserData.RegisterType<CultSection>();
            UserData.RegisterType<DiffSection>();
            UserData.RegisterType<ErasSection>();
            UserData.RegisterType<EspnSection>();
            UserData.RegisterType<ExprSection>();
            UserData.RegisterType<GoodSection>();
            UserData.RegisterType<GovtSection>();
            UserData.RegisterType<PrtoSection>();
            UserData.RegisterType<RaceSection>();
            UserData.RegisterType<TechSection>();
            UserData.RegisterType<TfrmSection>();
            UserData.RegisterType<TerrSection>();
            UserData.RegisterType<WsizSection>();
            UserData.RegisterType<FlavSection>();
        }
    }
    public class Civ3AsGlobalScript : Script
    {
        private SavData SavFile;
        public Civ3AsGlobalScript(string savPath, string defaultBicPath) : base(CoreModules.Preset_HardSandbox)
        {
            SavFile = new SavData(Util.ReadFile(savPath), Util.ReadFile(defaultBicPath));
            RegisterUserData();
            SetGlobals();
        }
        private void RegisterUserData()
        {
            UserData.RegisterType<Civ3File>();
            UserData.RegisterType<GameSection>();
            UserData.RegisterType<WrldSection>();
            UserData.RegisterType<MapTile>();
            UserData.RegisterType<ContItem>();
            UserData.RegisterType<LeaderItem>();
            UserData.RegisterType<CityItem>();
            
            UserData.RegisterType<BldgSection>();
            UserData.RegisterType<CtznSection>();
            UserData.RegisterType<CultSection>();
            UserData.RegisterType<DiffSection>();
            UserData.RegisterType<ErasSection>();
            UserData.RegisterType<EspnSection>();
            UserData.RegisterType<ExprSection>();
            UserData.RegisterType<GoodSection>();
            UserData.RegisterType<GovtSection>();
            UserData.RegisterType<PrtoSection>();
            UserData.RegisterType<RaceSection>();
            UserData.RegisterType<TechSection>();
            UserData.RegisterType<TfrmSection>();
            UserData.RegisterType<TerrSection>();
            UserData.RegisterType<WsizSection>();
            UserData.RegisterType<FlavSection>();
        }
        private void SetGlobals()
        {
            Globals["install_path"] = ReadCivData.UtilsCiv3.Util.GetCiv3Path();
            Globals["sav"] = SavFile.Sav;
            Globals["game"] = SavFile.Game;
            Globals["wrld"] = SavFile.Wrld;
            Globals["tile"] = SavFile.Tile;
            Globals["cont"] = SavFile.Cont;
            Globals["lead"] = SavFile.Lead;
            Globals["city"] = SavFile.City;

            Globals["bldg"] = SavFile.Bic.Bldg;
            Globals["ctzn"] = SavFile.Bic.Ctzn;
            Globals["cult"] = SavFile.Bic.Cult;
            Globals["diff"] = SavFile.Bic.Diff;
            Globals["eras"] = SavFile.Bic.Eras;
            Globals["espn"] = SavFile.Bic.Espn;
            Globals["expr"] = SavFile.Bic.Expr;
            Globals["good"] = SavFile.Bic.Good;
            Globals["govt"] = SavFile.Bic.Govt;
            Globals["prto"] = SavFile.Bic.Prto;
            Globals["race"] = SavFile.Bic.Race;
            Globals["tech"] = SavFile.Bic.Tech;
            Globals["tfrm"] = SavFile.Bic.Tfrm;
            Globals["terr"] = SavFile.Bic.Terr;
            Globals["wsiz"] = SavFile.Bic.Wsiz;
            Globals["flav"] = SavFile.Bic.Flav;
        }
    }
}