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
    public class ProgressionSection1
    {
        
    }

    public class ProgressionData
    {
        public int Unknown1;
        public int FileSize;
        public int HeaderSize;
        public int Unknown3;
        public int Unknown4;
        public int Unknown5;
        public int Unknown6;
        public int Section1Count;
        public uint Section2Offset; // entry len is 0xF8
        public int Section2Count;
        public uint Section3Offset; // entry len is 0x38
        public int Section3Count;
        public uint Section4Offset;
        public int Section4Count;
        public uint Section5Offset;
        public int Section5Count;
        public uint Section6Offset;
        public int Section6Count;
        public uint Section7Offset;
        public int Section7Count;

        public List<ProgressionSection1> Section1Entries;

        public ProgressionData()
        {
            Section1Entries = new List<ProgressionSection1>();
        }

        public static ProgressionData Read(BundleEntry entry)
        {
            ProgressionData result = new ProgressionData();

            MemoryStream ms = entry.MakeStream();
            BinaryReader2 br = new BinaryReader2(ms);

            result.Unknown1 = br.ReadInt32();
            result.FileSize = br.ReadInt32();
            result.HeaderSize = br.ReadInt32();
            result.Unknown3 = br.ReadInt32();
            result.Unknown4 = br.ReadInt32();
            result.Unknown5 = br.ReadInt32();
            result.Unknown6 = br.ReadInt32();
            result.Section1Count = br.ReadInt32();
            result.Section2Offset = br.ReadUInt32();
            result.Section2Count = br.ReadInt32();
            result.Section3Offset = br.ReadUInt32();
            result.Section3Count = br.ReadInt32();
            result.Section4Offset = br.ReadUInt32();
            result.Section4Count = br.ReadInt32();
            result.Section5Offset = br.ReadUInt32();
            result.Section5Count = br.ReadInt32();
            result.Section6Offset = br.ReadUInt32();
            result.Section6Count = br.ReadInt32();
            result.Section7Offset = br.ReadUInt32();
            result.Section7Count = br.ReadInt32();

            for (int i = 0; i < result.Section1Count; i++)
            {
                
            }

            // TODO: Finish Read

            br.Close();
            ms.Close();

            return result;
        }

        public void Write(BundleEntry entry)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            // TODO: Write

            bw.Flush();
            byte[] data = ms.ToArray();
            bw.Close();
            ms.Close();

            entry.Header = data;
            entry.Dirty = true;
        }
    }
}
