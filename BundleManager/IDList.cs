using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BundleFormat;
using BundleUtilities;

namespace BundleManager
{
    public class IDList
    {
        public int Unknown1;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
        public ulong ReferenceEntryID;
        public int Unknown6;
        public int Unknown7;

        public IDList()
        {
            
        }

        public static IDList Read(BundleEntry entry)
        {
            IDList result = new IDList();

            MemoryStream ms = entry.MakeStream();
            BinaryReader2 br = new BinaryReader2(ms);
            br.BigEndian = entry.Console;

            result.Unknown1 = br.ReadInt32();
            result.Unknown2 = br.ReadInt32();
            result.Unknown3 = br.ReadInt32();
            result.Unknown4 = br.ReadInt32();
            result.ReferenceEntryID = br.ReadUInt64();
            result.Unknown6 = br.ReadInt32();
            result.Unknown7 = br.ReadInt32();

            br.Close();
            ms.Close();

            return result;
        }

        public void Write(BundleEntry entry)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(Unknown1);
            bw.Write(Unknown2);
            bw.Write(Unknown3);
            bw.Write(Unknown4);
            bw.Write(ReferenceEntryID);
            bw.Write(Unknown6);
            bw.Write(Unknown7);

            bw.Flush();

            byte[] data = ms.ToArray();

            bw.Close();
            ms.Close();

            entry.Header = data;
            entry.Dirty = true;
        }
    }
}
