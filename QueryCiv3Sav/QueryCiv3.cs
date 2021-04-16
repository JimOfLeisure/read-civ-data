using System;
using System.IO;
using System.Collections.Generic;
using Blast;

namespace ReadCivData.QueryCiv3Sav {
    public class Civ3Section {
        public string Name;
        public int Offset;
    }
    public class Civ3File {
        protected internal byte[] FileData;
        protected internal Civ3Section[] Sections;
        public bool CustomBic {get; protected set;}
        public void Load(byte[] fileBytes)
        {
            this.FileData = fileBytes;
            // TODO: Check for CIV3 or BIC header?
            Sections = PopulateSections(FileData);
            int BicOffset = SectionOffset("VER#", 1);
            CustomBic = (uint)ReadInt32(BicOffset+8) != (uint)0xcdcdcdcd;
        }
        public void Load(string pathName)
        {
            byte[] MyFileData = File.ReadAllBytes(pathName);
            if (MyFileData[0] == 0x00 && (MyFileData[1] == 0x04 || MyFileData[1] == 0x05 || MyFileData[1] == 0x06))
            {
                Load(Decompress(MyFileData));
            }
            else
            {
                Load(MyFileData);
            }
        }
        // For dev validation only
        public void PrintFirstFourBytes()
        {
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            Console.WriteLine(ascii.GetString(this.FileData, 0, 4));
        }
        protected internal byte[] Decompress(byte[] compressedBytes)
        {
            MemoryStream DecompressedStream = new MemoryStream();
            BlastDecoder Decompressor = new BlastDecoder(new MemoryStream(compressedBytes, writable: false), DecompressedStream);
            Decompressor.Decompress();
            return DecompressedStream.ToArray();
        }
        protected internal Civ3Section[] PopulateSections(byte[] Data)
        {
            int Count = 0;
            int Offset = 0;
            List<Civ3Section> MySectionList = new List<Civ3Section>();
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            for (int i = 0; i < Data.Length; i++) {
                if (Data[i] < 0x20 || Data[i] > 0x5a) {
                    Count = 0;
                } else {
                    if (Count == 0) {
                        Offset = i;
                    }
                    Count++;
                }
                if (Count > 3) {
                    Count = 0;
                    Civ3Section Section = new Civ3Section();
                    Section.Offset = Offset;
                    Section.Name = ascii.GetString(Data, Offset, 4);
                    MySectionList.Add(Section);
                }
            }
            // TODO: Filter junk and dirty data from array (e.g. stray CITYs, non-headers, and such)
            return MySectionList.ToArray();
        }
        public int SectionOffset(string name, int nth)
        {
            int n = 0;
            for (int i = 0; i < this.Sections.Length; i++) {
                if (this.Sections[i].Name == name) {
                    n++;
                    if (n >= nth) {
                        return this.Sections[i].Offset + name.Length;
                    }
                }
            }
            // TODO: Add name and nth to message
            throw new ArgumentException("Unable to find section");
        }
        // TODO: Force little endian conversion on big endian systems
        //  although anticipated Intel and ARM targets are little endian, so maybe not important
        // NOTE: Cast result as (uint) if unsigned desired
        public int ReadInt32(int offset) => BitConverter.ToInt32(this.FileData, offset);
        // NOTE: Cast result as (ushort) if unsigned desired
        public short ReadInt16(int offset) => BitConverter.ToInt16(this.FileData, offset);
        // NOTE: Cast result as (sbyte) if signed desired
        public byte ReadByte(int offset) => this.FileData[offset];
    }
}