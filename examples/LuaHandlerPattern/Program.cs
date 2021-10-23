using System;
using ReadCivData.LuaCiv3;
using ReadCivData.QueryCiv3Sav;
using ReadCivData.UtilsCiv3;

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
            Script Lua = new Script();

            // Define a Lua handler function (probably usually loaded from elsewhere)
            Lua.DoString(@"
                -- Lua script
                function process_save(civ3)
                    process_count = process_count + 1
                    io.write(civ3.leaderItem[2].raceID)
                end

                -- if desired, can alter the Lua environment while defining function
                process_count = 0
            ");

            // Get Civ3 install path from registry
            string installPath = Util.GetCiv3Path();

            // Read (with auto decompress) default biq and a save file
            byte[] defaultBicBytes = Util.ReadFile(installPath + "/Conquests/conquests.biq");
            byte[] savFileBytes = Util.ReadFile(installPath + "/Conquests/Saves/Auto/Conquests Autosave 4000 BC.SAV");

        }
    }
}
