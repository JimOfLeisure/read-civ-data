using System;
using System.Collections.Generic;
using System.IO;
/*
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
*/

namespace ReadCivData.ConvertCiv3Media
{
    // refactoring from static conversion to ImageSharp, to width, height, palette, and pixel data
    public class Pcx {
        public byte[,] Palette = new byte[256,3];
        public byte[] Image = new byte[]{};
        public int Width = 0;
        public int Height = 0;
        public Pcx(){}
        public Pcx(string path) {
            this.Load(path);
        }
        // not a generalized pcx reader
        // assumes 8-bit image with 256-color 8-bit rgb palette
        public void Load(string path) {
            byte[] PcxBytes = File.ReadAllBytes(path);
            int LeftMargin = BitConverter.ToInt16(PcxBytes, 4);
            int TopMargin = BitConverter.ToInt16(PcxBytes, 6);
            int RightMargin = BitConverter.ToInt16(PcxBytes, 8);
            int BottomMargin = BitConverter.ToInt16(PcxBytes, 10);
            // assuming 1 color plane
            // this is always even, so last byte may be junk if image width is odd
            int BytesPerLine = BitConverter.ToInt16(PcxBytes, 0x42);

            this.Width = RightMargin - LeftMargin;
            this.Height = BottomMargin - TopMargin;
            int ImageLength = BytesPerLine * Height;
            int PaletteOffset = PcxBytes.Length - 768;

            for (int i = 0; i < 256; i++) {
                this.Palette[i,0] = PcxBytes[PaletteOffset + i * 3];
                this.Palette[i,1] = PcxBytes[PaletteOffset + i * 3 + 1];
                this.Palette[i,2] = PcxBytes[PaletteOffset + i * 3 + 2];
            }
            List<byte> ListImage = new List<byte>();
            bool JunkByte = BytesPerLine > Width;
            for (int ImgIdx = 0, PcxIdx = 0x80, RunLen = 0, LineIdx = 0; ImgIdx < Width * Height; ) {
                // if two most significant bits are 11
                if ((PcxBytes[PcxIdx] & 0xc0) == 0xc0) {
                    // then it & 0x3f is the run length of the following byte
                    RunLen = PcxBytes[PcxIdx] & 0x3f;
                    PcxIdx++;
                    for (int j = 0; j < RunLen; j++) {
                        if (!(JunkByte && LineIdx % BytesPerLine == BytesPerLine - 1)) {
                            // OutPixel[ImgIdx % Width, ImgIdx / Width] = Palette[PcxBytes[PcxIdx]];
                            ListImage.Add(PcxBytes[PcxIdx]);
                            ImgIdx++;
                        }
                        LineIdx++;
                    }
                    PcxIdx++;
                } else {
                    if (!(JunkByte && LineIdx % BytesPerLine == BytesPerLine - 1)) {
                        // OutPixel[ImgIdx % Width, ImgIdx / Width] = Palette[PcxBytes[PcxIdx]];
                        ListImage.Add(PcxBytes[PcxIdx]);
                        ImgIdx++;
                    }
                    PcxIdx++;
                    LineIdx++;
                }
            }
            this.Image = ListImage.ToArray();

        }
        /*
        // not a generalized pcx reader
        // assumes 8-bit image with 256-color 8-bit rgb palette
        static Image<Rgba32> Read(byte[] inPcx, int[] transparent) {
            int LeftMargin = BitConverter.ToInt16(inPcx, 4);
            int TopMargin = BitConverter.ToInt16(inPcx, 6);
            int RightMargin = BitConverter.ToInt16(inPcx, 8);
            int BottomMargin = BitConverter.ToInt16(inPcx, 10);
            // assuming 1 color plane
            // this is always even, so last byte may be junk if image width is odd
            int BytesPerLine = BitConverter.ToInt16(inPcx, 0x42);

            int Width = RightMargin - LeftMargin;
            int Height = BottomMargin - TopMargin;
            int ImageLength = BytesPerLine * Height;
            int PaletteOffset = inPcx.Length - 768;

            Rgba32[] Palette = new Rgba32[256];
            // TODO: account for civ colors
            //  for popheads I seemed to be using "if i < 16 or i % 2 == 0"
            for (int i = 0; i < 256; i++) {
                // Have to explicitly cast byte or it may use the normalized float constructor instead
                Palette[i] = new Rgba32(
                    (byte)inPcx[PaletteOffset + i * 3],
                    (byte)inPcx[PaletteOffset + i * 3 + 1],
                    (byte)inPcx[PaletteOffset + i * 3 + 2],
                    Array.Exists<int>(transparent, e => e == i) ? (byte)0 : (byte)255
                );
            }
            Image<Rgba32> OutPixel = new Image<Rgba32>(Width, Height);
            bool JunkByte = BytesPerLine > Width;
            for (int ImgIdx = 0, PcxIdx = 0x80, RunLen = 0, LineIdx = 0; ImgIdx < Width * Height; ) {
                // if two most significant bits are 11
                if ((inPcx[PcxIdx] & 0xc0) == 0xc0) {
                    // then it & 0x3f is the run length of the following byte
                    RunLen = inPcx[PcxIdx] & 0x3f;
                    PcxIdx++;
                    for (int j = 0; j < RunLen; j++) {
                        if (!(JunkByte && LineIdx % BytesPerLine == BytesPerLine - 1)) {
                            OutPixel[ImgIdx % Width, ImgIdx / Width] = Palette[inPcx[PcxIdx]];
                            ImgIdx++;
                        }
                        LineIdx++;
                    }
                    PcxIdx++;
                } else {
                    if (!(JunkByte && LineIdx % BytesPerLine == BytesPerLine - 1)) {
                        OutPixel[ImgIdx % Width, ImgIdx / Width] = Palette[inPcx[PcxIdx]];
                        ImgIdx++;
                    }
                    PcxIdx++;
                    LineIdx++;
                }
            }
            return OutPixel;
        }
        // temp test function
        public static void SaveOutPng() {
            byte[] PcxData = File.ReadAllBytes(@"/Users/jim/code/src/tmp/Godot/civ3play/temp/popHeads-ORIG.pcx");
            int[] Transparent = new int[]{254, 255};
            Image<Rgba32> ImgData = Read(PcxData, Transparent);
            string Path = "out.png";
            if (File.Exists(Path)) {
                File.Delete(Path);
            }
            using (FileStream fs = File.Create(Path)) {
                ImgData.SaveAsPng(fs);
            }
        }
        */
    }
}
