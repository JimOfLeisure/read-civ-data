using System;
using MoonSharp.Interpreter;
using ReadCivData.QueryCiv3Sav;

namespace ReadCivData.LuaCiv3 {
    public class Test
    {
        public static string Foo()
        {
            return "Hi from LuaCiv3";
        }
        public static void DoLua(string script)
        {
            Script.RunString(script);
        }
    }
    public class Civ3Script : Script
    {
        private string Civ3Path;
        private SavData SavFile;
        public Civ3Script(string path)
        {
            SavFile = new SavData(path);
            Console.WriteLine(SavFile.Game.CityCount);
        }
    }
        
}