using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BundleFormat;
using BundleUtilities;

namespace BundleFormat
{
    public class VertexDesc
    {
        public int Unknown1;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
        public byte Unknown5;
        public byte Stride;
        public short Unknown6;
        public int Unknown7;
        public int Unknown8;
        public int Unknown9;

        public VertexDesc()
        {
            
        }

        public static VertexDesc Read(BundleEntry entry)
        {
            VertexDesc result = new VertexDesc();

            MemoryStream ms = entry.MakeStream();
            BinaryReader2 br = new BinaryReader2(ms);
            br.BigEndian = entry.Console;

            result.Unknown1 = br.ReadInt32();
            result.Unknown2 = br.ReadInt32();
            result.Unknown3 = br.ReadInt32();
            result.Unknown4 = br.ReadInt32();
            result.Unknown5 = br.ReadByte();
            result.Stride = br.ReadByte();
            result.Unknown6 = br.ReadInt16();
            result.Unknown7 = br.ReadInt32();
            result.Unknown8 = br.ReadInt32();
            result.Unknown9 = br.ReadInt32();

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
            bw.Write(Unknown5);
            bw.Write(Stride);
            bw.Write(Unknown6);
            bw.Write(Unknown7);
            bw.Write(Unknown8);
            bw.Write(Unknown9);

            bw.Flush();
            byte[] Data = ms.GetBuffer();
            bw.Close();
            ms.Close();

            entry.Header = Data;
            entry.Dirty = true;
        }
    }
}
