using System;
using System.IO;
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
            // Flic decoder
            Flic MyAnimation = new Flic(PcxFilePath);
            Console.WriteLine(MyAnimation.Width);
            Console.WriteLine(MyAnimation.Height);

            // Create ImageSharp color Palette
            Rgba32[] ISPalette = new Rgba32[256]; 
            for (int i = 0; i < 256; i++) {
                // Have to explicitly cast byte or it may use the normalized float constructor instead
                ISPalette[i] = new Rgba32(
                    MyAnimation.Palette[i,0],
                    MyAnimation.Palette[i,1],
                    MyAnimation.Palette[i,2],
                    // For MyAnimation, palette 255 is transparent
                    // i == 255 ? (byte)0 : (byte)255
                    // shadows and transparency for 240-255
                    i > 239 ? (byte)((255 -i) * 16) : (byte)255
                );
            }

            // Create ImageSharp Image
            Image<Rgba32> ISImage = new Image<Rgba32>(MyAnimation.Width, MyAnimation.Height);

            // frames/images loop
            for (int frame = 0; frame < MyAnimation.Images.Length; frame++) {
                // image pixels loop
                for (int y = 0; y < MyAnimation.Height; y++) {
                    for (int x = 0; x < MyAnimation.Width; x++) {
                        ISImage[x, y] = ISPalette[MyAnimation.Images[frame][x + y * MyAnimation.Width]];
                    }
                }
                // Save as PNG
                using (FileStream fs = File.Create("out-" + frame.ToString("D2") + ".png")) {
                    ISImage.SaveAsPng(fs);
                }
            }

        }
    }
}
