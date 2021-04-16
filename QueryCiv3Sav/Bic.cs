using System;
using System.Collections.Generic;

namespace ReadCivData.QueryCiv3Sav
{
    public class BicData
    {
        public BldgSection[] Bldg;
        public Civ3File Bic;
        public BicData(byte[] bicBytes)
        {
            Bic = new Civ3File();
            Bic.Load(bicBytes);
            Init();
        }
        public BicData(string bicPath)
        {
            Bic = new Civ3File();
            Bic.Load(bicPath);
            Init();
        }
        public void Init()
        {
            Bldg = (new ListSection<BldgSection>(Bic, Bic.SectionOffset("BLDG", 1))).Sections.ToArray();
        }
    }
    public interface ISectionListItem
    {
        void Init(Civ3File bic, int offset, int length);

    }
    public class ListSection<T> where T : ISectionListItem, new()
    {
        Civ3File Bic;
        int Offset;
        int ItemCount;
        public ListSection(Civ3File bic, int offset)
        {
            Bic = bic;
            ItemCount = Bic.ReadInt32(offset);
            Offset = offset;
        }
        public List<T> Sections
        { get {
            int CurrentOffset = Offset + 4;
            List<T> OutList = new List<T>();
            for(int i=0; i<ItemCount; i++)
            {
                int ItemLength = Bic.ReadInt32(CurrentOffset);
                CurrentOffset += 4;
                T Item = new T();
                Item.Init(Bic, CurrentOffset, ItemLength);
                OutList.Add(Item);
                CurrentOffset += ItemLength;

            }
            return OutList;
        }}
        public void Init(Civ3File bic, int offset, int length){}
    }
    public class BldgSection : ISectionListItem
    {
        public string DevTest;
        public byte[] RawBytes;
        public void Init(Civ3File bic, int offset, int length)
        {
            DevTest = "BLDG off " + offset.ToString() + " len " + length.ToString();
            List<byte> ByteList = new List<byte>();
            for(int i=0; i<length; i++) ByteList.Add(bic.FileData[i+offset]);
            RawBytes = ByteList.ToArray();
        }
    }
}