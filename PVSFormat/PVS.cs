using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BundleFormat;

namespace PVSFormat
{
    public struct PVSEntry
    {
        public ulong Ptr1;
        public ulong Ptr2;
        public int TrackID;
        public int Unknown1;
        public short Unknown2;
        public short Count1;
        public short Unknown3;
        public short Count2;
        public int Unknown4;
        public int Unknown5;
        public int Unknown6;
        public int Unknown7;

        public override string ToString()
        {
            return "TrackID: " + TrackID;
        }
    }

    public class PVS
    {
        public int Unknown1;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
        public int Unknown6;
        public int Unknown7;
        public int Unknown8;
        public List<PVSEntry> Entries;

        public PVS()
        {
            Entries = new List<PVSEntry>();
        }

        public static PVS Read(BND2Entry entry, bool console)
        {
            PVS result = new PVS();

            Stream s = entry.MakeStream();
            BinaryReader br = new BinaryReader(s);

            result.Unknown1 = br.ReadInt32();
            result.Unknown2 = br.ReadInt32();
            result.Unknown3 = br.ReadInt32();
            result.Unknown4 = br.ReadInt32();
            int numEntries = br.ReadInt32();
            result.Unknown6 = br.ReadInt32();
            result.Unknown7 = br.ReadInt32();
            result.Unknown8 = br.ReadInt32();

            for (int i = 0; i < numEntries; i++)
            {
                PVSEntry pvsEntry = new PVSEntry();

                pvsEntry.Ptr1 = br.ReadUInt64();
                pvsEntry.Ptr2 = br.ReadUInt64();
                pvsEntry.TrackID = br.ReadInt32();
                pvsEntry.Unknown1 = br.ReadInt32();
                pvsEntry.Unknown2 = br.ReadInt16();
                pvsEntry.Count1 = br.ReadInt16();
                pvsEntry.Unknown3 = br.ReadInt16();
                pvsEntry.Count2 = br.ReadInt16();
                pvsEntry.Unknown4 = br.ReadInt32();
                pvsEntry.Unknown5 = br.ReadInt32();
                pvsEntry.Unknown6 = br.ReadInt32();
                pvsEntry.Unknown7 = br.ReadInt32();

                result.Entries.Add(pvsEntry);
            }

            br.Close();
            s.Close();

            return result;
        }

        public static BND2Entry Write(BND2Entry entry)
        {
            return entry;
        }
    }
}
