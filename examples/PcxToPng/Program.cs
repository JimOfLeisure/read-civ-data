using System;
using System.IO;
using ReadCivData.ConvertCiv3Media;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace FlcToPngs
{
    class Program
    {
        static string OutFilePath = "out.png";
        static string Civ3RootPath = @"/Volumes/minio-storage/steamapps/steamapps/common/Sid Meier's Civilization III Complete";
        static string PcxFilePath = Civ3RootPath + @"/Art/SmallHeads/popHeads.pcx";
        static void Main(string[] args)
        {
            // PCX decoder
            Pcx PopHeads = new Pcx(PcxFilePath);

            // Create ImageSharp color Palette
            Rgba32[] ISPalette = new Rgba32[256]; 
            for (int i = 0; i < 256; i++) {
                // Have to explicitly cast byte or it may use the normalized float constructor instead
                ISPalette[i] = new Rgba32(
                    PopHeads.Palette[i,0],
                    PopHeads.Palette[i,1],
                    PopHeads.Palette[i,2],
                    // For PopHeads, palette 254 and 255 are transparent
                    i > 253 ? (byte)0 : (byte)255
                );
            }

            // Create ImageSharp Image
            Image<Rgba32> ISImage = new Image<Rgba32>(PopHeads.Width, PopHeads.Height);
            for (int y = 0; y < PopHeads.Height; y++) {
                for (int x = 0; x < PopHeads.Width; x++) {
                    ISImage[x, y] = ISPalette[PopHeads.Image[x + y * PopHeads.Width]];
                }
            }

            // Save as PNG
            using (FileStream fs = File.Create(OutFilePath)) {
                ISImage.SaveAsPng(fs);
            }
        }
    }
}
