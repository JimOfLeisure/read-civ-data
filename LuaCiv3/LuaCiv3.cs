using System;
using MoonSharp.Interpreter;
using ReadCivData.QueryCiv3Sav;

namespace ReadCivData.LuaCiv3 {
    public class Civ3Script : Script
    {
        private SavData SavFile;
        public Civ3Script(string savPath, string defaultBicPath)
        {
            SavFile = new SavData(savPath, defaultBicPath);
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