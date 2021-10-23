
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
            Console.WriteLine("Test stub");
            string Civ3Path = Util.GetCiv3Path();
            Console.WriteLine(Civ3Path);
            string SavPath = Civ3Path + "/Conquests/Saves/Auto/Conquests Autosave 3950 BC.SAV";
            string DefaultBicPath = Civ3Path + "/Conquests/conquests.biq";
            Civ3AsGlobalScript Lua = new Civ3AsGlobalScript((SavPath), DefaultBicPath);
            Lua.DoString(@"
    print(install_path)
    print(game.CityCount)
    print(wrld.WsizID)
    print(bldg)
    print(bldg[1].Name)
    print(bldg[1].DevTest)
    print(city[1].Name)
");
        }
    }
}
