using System;
using System.IO;
/*
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
*/

namespace ReadCivData.ConvertCiv3Media
{
    // Under construction
    // Not intended to be a generalized/universal Flic reader
    // Implementing from description at https://www.drdobbs.com/windows/the-flic-file-format/184408954
    public class Flic {
        // Images is an array of images, each of which is a byte array of palette indexes
        // In the future this may become a 3-dimensional array of direction - images - byte array
        public byte[][] Images;
        // All animations/images have same palette, height, and width
        // Palette is 256 colors in red, green, blue order
        public byte[,] Palette = new byte[256,3];
        public int Width = 0;
        public int Height = 0;

        // constructors
        public Flic(){}
        public Flic(string path) {
            this.Load(path);
        }
        public void Load(string path) {
            byte[] FlicBytes = File.ReadAllBytes(path);

            int FileFormat = BitConverter.ToUInt16(FlicBytes, 4);
            // Should be 0xAF12
            // Console.WriteLine(String.Format("0x{0:X04}", FileFormat));

            // TODO: this may not be right for Civ3 FLCs
            int NumFrames = BitConverter.ToUInt16(FlicBytes, 6);
            this.Width = BitConverter.ToUInt16(FlicBytes, 8);
            this.Height = BitConverter.ToUInt16(FlicBytes, 10);
            int ImageLength = this.Width * this.Height;

            // Initialize image frames
            this.Images = new byte[NumFrames][];
            for (int i = 0; i < this.Images.Length; i++) {
                this.Images[i] = new byte[this.Width * this.Height];
            }

            // technically should be UInt32 I think
            // frame 1 chunk offset
            int Offset = BitConverter.ToInt32(FlicBytes, 80);
            // bool PaletteIsFilled = false;
            // Flic frames loop
            // FIXME: The following two result in different numbers of frames processed!
            // for (; Offset < FlicBytes.Length;) {
            for (int f = 0; f < NumFrames; f++) {
                // Frame chunk headers should be 0xF1Fa; prefix chunk header is 0xF100
                int ChunkLength = BitConverter.ToInt32(FlicBytes, Offset);
                int Chunktype = BitConverter.ToUInt16(FlicBytes, Offset + 4);
                int NumSubChunks = BitConverter.ToUInt16(FlicBytes, Offset + 6);

                // bool ImageReady = false;
                // Chunk loop; I may be mixing up chunks, frames and subchunks in var names
                for (int i = 0, SubOffset = Offset + 16; i < NumSubChunks; i++) {
                    int SubChunkLength = BitConverter.ToInt32(FlicBytes, SubOffset);
                    int SubChunkType = BitConverter.ToUInt16(FlicBytes, SubOffset + 4);
                    // byte[,] PixelArray = new byte[Width, Height];
                    switch (SubChunkType) {
                        case 4:
                            // Palette chunk
                            int NumPackets = BitConverter.ToUInt16(FlicBytes, SubOffset + 6);
                            if (NumPackets != 1) {
                                throw new ArgumentException("Unable to deal with color palette with more than one packet; NumPackets = " + NumPackets);
                            }
                            int SkipCount = BitConverter.GetBytes(BitConverter.ToChar(FlicBytes, SubOffset + 8))[0];
                            if (SkipCount != 0) {
                                throw new ArgumentException("Unable to deal with color palette with non-zero SkipCount = " + SkipCount);
                            }
                            for (int p = 0; p < 256; p++) {
                                this.Palette[p,0] = FlicBytes[8 + SubOffset + p * 3];
                                this.Palette[p,1] = FlicBytes[8 + SubOffset + p * 3 + 1];
                                this.Palette[p,2] = FlicBytes[8 + SubOffset + p * 3 + 2];
                            }
                            // PaletteIsFilled = true;
                            break;
                        case 15:
                            // run-length-encoded full frame chunk
                            // Assuming only one 15 subchunk per frame
                            // first frame has pixel subchunk before palette subchunk, so will extract to temp byte array
                            for (int y = 0, x = 0, head = SubOffset + 6; y < Height; y++, x = 0) {
                                // first byte of row is obsolete
                                head++;
                                for (; x < Width;) {
                                    int TypeSize = (sbyte)FlicBytes[head];
                                    if (TypeSize == 0) {
                                        throw new ApplicationException("TypeSize is 0");
                                    }
                                    if (TypeSize > 0) {
                                        head++;
                                        for (int foo = 0; foo < Math.Abs(TypeSize); foo++) {
                                            // PixelArray[x,y] = FlicBytes[head];
                                            try {
                                            this.Images[f][y * this.Width + x] = FlicBytes[head];
                                            } catch { Console.WriteLine(f + " " + y + " " + x); }
                                            x++;
                                        }
                                        head++;
                                    } else {
                                        head++;
                                        for (int foo = 0; foo < Math.Abs(TypeSize); foo++) {
                                            // PixelArray[x,y] = FlicBytes[head];
                                            this.Images[f][y * this.Width + x] = FlicBytes[head];
                                            head++;
                                            x++;
                                        }
                                    }
                                }
                            }
                            // ImageReady = true;
                            break;
                        case 7:
                            // TODO: figure out why frame 0 gets overwritten
                            if (f == 0) {
                                break;
                            }
                            // diff chunk
                            // Copy last frame image
                            Array.Copy(this.Images[f-1], this.Images[f], this.Images[f].Length);
                            int NumLines = BitConverter.ToUInt16(FlicBytes, SubOffset + 6);
                            for (int Line = 0, y = 0, head = SubOffset + 8; Line < NumLines; Line++) {
                                int WordsPerLine = BitConverter.ToInt16(FlicBytes, head);
                                head += 2;
                                // if two high bits are 1s, this is a special skip-lines word
                                if ((WordsPerLine & 0xc00) == 0xc00) {
                                    y += Math.Abs(WordsPerLine);
                                    WordsPerLine = BitConverter.ToInt16(FlicBytes, head);
                                    head+=2;
                                }
                                // If two high bits are 10, this is a special word to set the last pixel for odd-length lines
                                // This may not have been tested; none of my Flics change the last pixel
                                if ((WordsPerLine & 0x800) == 0x800) {
                                    // OutImages[f][Width - 1, y] = Palette[(byte)(WordsPerLine & 0xff)];
                                    this.Images[f][this.Width * (y + 1) - 1] = (byte)(WordsPerLine & 0xff);
                                    WordsPerLine = BitConverter.ToInt16(FlicBytes, head);
                                    head+=2;
                                }
                                // We're out of special words; if this word has either high bit set, throw exception
                                if ((WordsPerLine & 0xc00) != 0) {
                                    throw new ApplicationException("WordsPerLine high bits set: " + WordsPerLine);
                                }
                                // I wonder if WordsPerLine should actally be PacketsPerLine
                                // Loop over the packets for this line
                                for (int packet = 0, x = 0; packet < WordsPerLine; packet++) {
                                    // least significant byte of word (first byte) is columns to skip
                                    x += FlicBytes[head];
                                    head++;
                                    // most significant byte of word (second byte) is number of words in the packet
                                    int NumWords = (sbyte)FlicBytes[head];
                                    bool Positive = NumWords > 0;
                                    head++;
                                    // If NumWords is positive, copy NumWords following words to image
                                    // If NumWords is negative, repeat the next word abs(NumWords) times
                                    for (int ii = 0; ii < Math.Abs(NumWords); ii++) {
                                        // OutImages[f][x,y] = Palette[FlicBytes[head]];
                                        // OutImages[f][x+1,y] = Palette[FlicBytes[head+1]];
                                        this.Images[f][this.Width * y + x] = FlicBytes[head];
                                        this.Images[f][this.Width * y + x + 1] = FlicBytes[head + 1];
                                        if (Positive) { head += 2; }
                                        x += 2;
                                    }
                                    // If NumWords was negative, we're still pointing at the repeated word, so advance head
                                    if (! Positive) { head += 2; }
                                }
                                y++;
                            }
                            break;
                        default:
                            // Console.WriteLine("Subchunk not recognized: " + SubChunkType);
                            break;
                    }
                    /*
                    if (PaletteIsFilled && ImageReady) {
                        for (int y = 0; y < Height; y++) {
                            for (int x = 0; x < Width; x++) {
                                OutImages[f][x,y] = Palette[PixelArray[x,y]];
                            }
                        }
                        ImageReady = false;
                    }
                    */
                    SubOffset += SubChunkLength;
                }
                Offset += ChunkLength;
            }
        }
        /*
        static public Image<Rgba32>[] Read(byte[] inFlic, int[] transparent) {
            int FileFormat = BitConverter.ToUInt16(inFlic, 4);
            // Should be 0xAF12
            // Console.WriteLine(String.Format("0x{0:X04}", FileFormat));

            int NumFrames = BitConverter.ToUInt16(inFlic, 6);
            Image<Rgba32>[] OutImages = new Image<Rgba32>[NumFrames];
            Rgba32[] Palette = new Rgba32[256];

            int Width = BitConverter.ToUInt16(inFlic, 8);
            int Height = BitConverter.ToUInt16(inFlic, 10);
            for (int i = 0; i < NumFrames; i++) {
                OutImages[i] = new Image<Rgba32>(Width, Height, new Rgba32(127,127,127,255));
            }

            // technically should be UInt32 I think
            // frame 1 chunk offset
            int Offset = BitConverter.ToInt32(inFlic, 80);
            bool PaletteIsFilled = false;
            // FIXME: The following two result in different numbers of frames processed!
            // for (; Offset < inFlic.Length;) {
            for (int f = 0; f < NumFrames; f++) {
                // Frame chunk headers should be 0xF1Fa; prefix chunk header is 0xF100
                int ChunkLength = BitConverter.ToInt32(inFlic, Offset);
                int Chunktype = BitConverter.ToUInt16(inFlic, Offset + 4);
                int NumSubChunks = BitConverter.ToUInt16(inFlic, Offset + 6);

                bool ImageReady = false;
                for (int i = 0, SubOffset = Offset + 16; i < NumSubChunks; i++) {
                    int SubChunkLength = BitConverter.ToInt32(inFlic, SubOffset);
                    int SubChunkType = BitConverter.ToUInt16(inFlic, SubOffset + 4);
                    byte[,] PixelArray = new byte[Width, Height];
                    switch (SubChunkType) {
                        case 4:
                            int NumPackets = BitConverter.ToUInt16(inFlic, SubOffset + 6);
                            if (NumPackets != 1) {
                                throw new ArgumentException("Unable to deal with color palette with more than one packet; NumPackets = " + NumPackets);
                            }
                            int SkipCount = BitConverter.GetBytes(BitConverter.ToChar(inFlic, SubOffset + 8))[0];
                            if (SkipCount != 0) {
                                throw new ArgumentException("Unable to deal with color palette with non-zero SkipCount = " + SkipCount);
                            }
                            for (int p = 0; p < 256; p++) {
                                // Have to explicitly cast byte or it may use the normalized float constructor instead
                                Palette[p] = new Rgba32(
                                    (byte)inFlic[8 + SubOffset + p * 3],
                                    (byte)inFlic[8 + SubOffset + p * 3 + 1],
                                    (byte)inFlic[8 + SubOffset + p * 3 + 2],
                                    Array.Exists<int>(transparent, e => e == p) ? (byte)0 : (byte)255
                                );
                                // Trying out shadows, but this only works on units I think
                                // These shadows look good on units!
                                for (int foo = 240; foo < 256; foo++) {
                                    Palette[foo] = new Rgba32(0,0,0,(byte)((255 - foo) *16));
                                }
                            }
                            PaletteIsFilled = true;
                            break;
                        case 15:
                            // Assuming only one 15 subchunk per frame
                            // first frame has pixel subchunk before palette subchunk, so will extract to temp byte array
                            for (int y = 0, x = 0, head = SubOffset + 6; y < Height; y++, x = 0) {
                                // first byte of row is obsolete
                                head++;
                                for (; x < Width;) {
                                    int TypeSize = (sbyte)inFlic[head];
                                    if (TypeSize == 0) {
                                        throw new ApplicationException("TypeSize is 0");
                                    }
                                    if (TypeSize > 0) {
                                        head++;
                                        for (int foo = 0; foo < Math.Abs(TypeSize); foo++) {
                                            PixelArray[x,y] = inFlic[head];
                                            x++;
                                        }
                                        head++;
                                    } else {
                                        head++;
                                        for (int foo = 0; foo < Math.Abs(TypeSize); foo++) {
                                            PixelArray[x,y] = inFlic[head];
                                            head++;
                                            x++;
                                        }
                                    }
                                }
                            }
                            ImageReady = true;
                            break;
                        case 7:
                            // Copy last frame image
                            OutImages[f] = OutImages[f-1].CloneAs<Rgba32>();
                            int NumLines = BitConverter.ToUInt16(inFlic, SubOffset + 6);
                            for (int Line = 0, y = 0, head = SubOffset + 8; Line < NumLines; Line++) {
                                int WordsPerLine = BitConverter.ToInt16(inFlic, head);
                                head += 2;
                                // skip lines?
                                if ((WordsPerLine & 0xc00) == 0xc00) {
                                    y += Math.Abs(WordsPerLine);
                                    WordsPerLine = BitConverter.ToInt16(inFlic, head);
                                    head+=2;
                                }
                                // Last pixel for odd-length lines?
                                // This may not have been tested; none of my Flics change the last pixel
                                if ((WordsPerLine & 0x800) == 0x800) {
                                    OutImages[f][Width - 1, y] = Palette[(byte)(WordsPerLine & 0xff)];
                                    WordsPerLine = BitConverter.ToInt16(inFlic, head);
                                    head+=2;
                                }
                                if ((WordsPerLine & 0xc00) != 0) {
                                    throw new ApplicationException("WordsPerLine high bits set: " + WordsPerLine);
                                }
                                for (int packet = 0, x = 0; packet < WordsPerLine; packet++) {
                                    // column skip
                                    x += inFlic[head];
                                    head++;
                                    int NumWords = (sbyte)inFlic[head];
                                    bool Positive = NumWords > 0;
                                    head++;
                                    for (int ii = 0; ii < Math.Abs(NumWords); ii++) {
                                        OutImages[f][x,y] = Palette[inFlic[head]];
                                        OutImages[f][x+1,y] = Palette[inFlic[head+1]];
                                        if (Positive) { head += 2; }
                                        x += 2;
                                    }
                                    if (! Positive) { head += 2; }
                                }
                                y++;
                            }
                            break;
                        default:
                            // Console.WriteLine("Subchunk not recognized: " + SubChunkType);
                            break;
                    }
                    if (PaletteIsFilled && ImageReady) {
                        for (int y = 0; y < Height; y++) {
                            for (int x = 0; x < Width; x++) {
                                OutImages[f][x,y] = Palette[PixelArray[x,y]];
                            }
                        }
                        ImageReady = false;
                    }
                    SubOffset += SubChunkLength;
                }
                Offset += ChunkLength;
            }
            return OutImages;
        }
        public static void SaveOutPngs() {
            byte[] FlicData = File.ReadAllBytes(@"/Volumes/minio-storage/steamapps/steamapps/common/Sid Meier's Civilization III Complete" +
                @"/Art/Units/warrior/warriorRun.flc");
            int[] Transparent = new int[]{254,255};
            Image<Rgba32>[] ImgData = Read(FlicData, Transparent);
            for (int i = 0; i < ImgData.Length; i++) {
                string Path = "out-" + i.ToString("D2") + ".png";
                if (File.Exists(Path)) {
                    File.Delete(Path);
                }
                using (FileStream fs = File.Create(Path)) {
                        ImgData[i].SaveAsPng(fs);
                }
            }
        }
        */
    }
}