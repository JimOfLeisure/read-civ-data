using System;
using ReadCivData.LuaCiv3;
using ReadCivData.QueryCiv3Sav;
using ReadCivData.UtilsCiv3;
using MoonSharp.Interpreter;

namespace LuaHandlerPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // .NET 5 and later need Nuget package "System.Text.Encoding.CodePages" and registration for Windows-1252 (and other old locales) encoding
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            // Create Lua environment
            ReadCivData.LuaCiv3.Script lua = new ReadCivData.LuaCiv3.Script();

            // Define a Lua handler function (probably usually loaded from elsewhere)
            lua.DoString(@"
                -- Lua script
                function process_save(civ3)
                    process_count = process_count + 1
                    return ""Player 1 raceID is "" .. civ3.lead[2].raceID
                end

                -- if desired, can alter the Lua environment while defining function
                process_count = 0
            ");

            // Get Civ3 install path from registry
            string installPath = Util.GetCiv3Path();

            // Read (with auto decompress) default biq and a save file
            byte[] defaultBicBytes = Util.ReadFile(installPath + "/Conquests/conquests.biq");
            byte[] savFileBytes = Util.ReadFile(installPath + "/Conquests/Saves/Auto/Conquests Autosave 4000 BC.SAV");

            // Create a SavData object
            SavData civ3 = new SavData(savFileBytes, defaultBicBytes);

            // Call previously-defined Lua function with the SavData object
            DynValue res = lua.Call(lua.Globals["process_save"], civ3);

            // Do something with the return value
            Console.WriteLine(res);
        }
    }
}
