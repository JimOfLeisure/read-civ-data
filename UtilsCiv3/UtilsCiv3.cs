using System;
using System.IO;
using Blast;

namespace ReadCivData.UtilsCiv3 {
    public class Util
    {
        static public byte[] ReadFile(string pathName)
        {
            byte[] MyFileData = File.ReadAllBytes(pathName);
            if (MyFileData[0] == 0x00 && (MyFileData[1] == 0x04 || MyFileData[1] == 0x05 || MyFileData[1] == 0x06))
            {
                return Decompress(MyFileData);
            }
            return MyFileData;
        }
        static public byte[] Decompress(byte[] compressedBytes)
        {
            MemoryStream DecompressedStream = new MemoryStream();
            BlastDecoder Decompressor = new BlastDecoder(new MemoryStream(compressedBytes, writable: false), DecompressedStream);
            Decompressor.Decompress();
            return DecompressedStream.ToArray();
        }

        static public string GetCiv3Path()
        {
            // Use CIV3_HOME env var if present
            string path = System.Environment.GetEnvironmentVariable("CIV3_HOME");
            if (path != null) return path;

            // TODO: Maybe check an array of hard-coded paths during dev time?
            return "/civ3/path/not/found";
        }

        static public string Civ3MediaPath(string relPath, string relModPath = "")
        // Pass this function a relative path (e.g. Art/Terrain/xpgc.pcx) and it will grab the correct version
        // Assumes Conquests/Complete
        {
            string Civ3Root = GetCiv3Path();
            string [] TryPaths = new string [] {
                relModPath,
                "Conquests",
                "civ3PTW",
                ""
            };
            for(int i = 0; i < TryPaths.Length; i++)
            {
                // If relModPath not set, skip that check
                if(i == 0 && relModPath == "") { continue; }
                string pathCandidate = Civ3Root + "/" + TryPaths[i] + "/" + relPath;
                if(System.IO.File.Exists(pathCandidate)) { return pathCandidate; }
            }
            throw new ApplicationException("Media path not found: " + relPath);
        }
    }
}