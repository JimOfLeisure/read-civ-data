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
        public class ListSection<T>
        {
            int Offset;
            int ItemCount;
            public ListSection(Civ3File bic, int offset)
            {
                ItemCount = bic.ReadInt32(offset);
                Offset = offset;
            }
            public List<T> Sections
            { get {
                List<T> OutList = new List<T>();
                return OutList;
            }}
        }
    }
}