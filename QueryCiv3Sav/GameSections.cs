using System;
using System.Collections.Generic;

namespace ReadCivData.QueryCiv3Sav
{
    public class GenericSection
    {
        public int Offset { get; protected set; }
        private SavData Sav;
        private int Length;
        public GenericSection(SavData sav, string headerString, int length = 256)
        {
            Sav = sav;
            Offset = Sav.Sav.SectionOffset(headerString, 1);
        }
        public byte[] RawBytes { get => Sav.Sav.GetBytes(Offset, Length); }
    }
     public class GameSection
    {
        private int Offset;
        private SavData Data;
        public GameSection(SavData sav)
        // Need to know continent count for some offsets, not sure if it's somewhere in Game so grabbing it from Wrld
        {
            Data = sav;
            // TODO: Change nth to 1 after separating BIC and SAV data
            Offset = Data.Sav.SectionOffset("GAME", 2);
        }
        // TODO: Return Bic sections, not IDs
        public int DifficultyID { get => Data.Sav.ReadInt32(Offset+20); }
        public DiffSection Difficulty { get => Data.Bic.Diff[DifficultyID]; }
        public int UnitCount { get => Data.Sav.ReadInt32(Offset+28); }
        public int CityCount { get => Data.Sav.ReadInt32(Offset+32); }
        // The per-civ tech list is actually a per-tech 32-bit bitmask, and the number of continents impacts its offset
        public int[] TechCivMask
        { get {
            int MaskOffset = Offset + 856 + (Data.Wrld.ContinentCount * 4);
            int NumTechs = Data.Bic.Tech.Length;
            List<int> Out = new List<int>();
            for(int i=0; i<NumTechs; i++) Out.Add(Data.Sav.ReadInt32(MaskOffset + 4 * i));
            return Out.ToArray();
        }}
    }
   public class WrldSection
    {
        private SavData Data;
        private int Offset1, Offset2, Offset3;
        public WrldSection(SavData sav)
        {
            Data = sav;
            Offset1 = Data.Sav.SectionOffset("WRLD", 1);
            Offset2 = Data.Sav.SectionOffset("WRLD", 2);
            Offset3 = Data.Sav.SectionOffset("WRLD", 3);
        }
        public short ContinentCount{ get => Data.Sav.ReadInt16(Offset1+4); }
        // TODO: Return Bic sections, not IDs
        public int WsizID{ get => Data.Sav.ReadInt32(Offset1+234); }
    }
}