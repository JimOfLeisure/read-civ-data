
using System;
using Xunit;
using ReadCivData.LuaCiv3;

namespace LuaCiv3.Tests
{
    public class DevTest
    {
        [Fact]
        public void Test1()
        {
            Console.WriteLine("Test stub");
            string Civ3Path = ReadCivData.UtilsCiv3.Util.GetCiv3Path();
            Console.WriteLine(Civ3Path);
        }
    }
}
