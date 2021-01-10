using System;
using ReadCivData.ConvertCiv3Media;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace FlcToPngs
{
    class Program
    {
        // static string OutFilePath = "out.png";
        static string Civ3RootPath = @"/Volumes/minio-storage/steamapps/steamapps/common/Sid Meier's Civilization III Complete";
        static string PcxFilePath = Civ3RootPath + @"/Art/Units/warrior/warriorRun.flc";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Flic MyAnimation = new Flic(PcxFilePath);
            Console.WriteLine(MyAnimation.Width);
            Console.WriteLine(MyAnimation.Height);
        }
    }
}
