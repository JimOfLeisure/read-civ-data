
using System;
using Xunit;
using ReadCivData.LuaCiv3;
using ReadCivData.UtilsCiv3;

namespace LuaCiv3.Tests
{
    public class DevTest
    {
        [Fact]
        public void Test1()
        {
            // .NET 5 needs Nuget package and registration for Windows-1252 encoding
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            string Civ3Path = Util.GetCiv3Path();
            string SavPath = Civ3Path + "/Conquests/Saves/Auto/Conquests Autosave 3950 BC.SAV";
            string DefaultBicPath = Civ3Path + "/Conquests/conquests.biq";
            Console.WriteLine(SavPath);
            Script Lua = new Script();
            Lua.LoadCiv3(Util.ReadFile(SavPath), Util.ReadFile(DefaultBicPath));
            Lua.DoString(@"
    print(civ3)
    print(civ3.sav)
    print(civ3.game)
    print(civ3.game.CityCount)
    print(civ3.wrld.WsizID)
    print(civ3.bic.bldg)
    print(civ3.bic.bldg[1].Name)
    print(civ3.bic.bldg[1].DevTest)
    print(civ3.city[1].Name)
");
        }
    }
}
