using System;
using System.Collections.Generic;

namespace ReadCivData.QueryCiv3Sav
{
    public class BicData
    {
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
            //
        }
        public class ListSection<T> where T : ISectionListItem<T>, new()
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
                    T Item = new T();
                    Item.Init(Bic, CurrentOffset, ItemLength);
                    CurrentOffset += ItemLength;

                }
                return OutList;
            }}
            public void Init(Civ3File bic, int offset, int length){}
        }
    }
    public interface ISectionListItem<T>
    {
        void Init(Civ3File bic, int offset, int length);

    }
}