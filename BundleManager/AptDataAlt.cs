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
    public class AptDataAlt
    {
        public string Component1Name;
        public string Component2Name;

        public AptDataAlt()
        {
            
        }

        public static AptDataAlt Read(BundleEntry entry)
        {
            AptDataAlt result = new AptDataAlt();

            MemoryStream ms = entry.MakeStream();
            BinaryReader2 br = new BinaryReader2(ms);
            br.BigEndian = entry.Console;

            uint componentName2Ptr = br.ReadUInt32();
            uint componentName1Ptr = br.ReadUInt32();
            uint aptDataOffset = br.ReadUInt32();
            uint constOffset = br.ReadUInt32();
            uint geometryOffset = br.ReadUInt32();
            uint fileSize = br.ReadUInt32();

            /*int numPadding = (int)(16 - br.BaseStream.Position % 16);
            for (int i = 0; i < numPadding; i++)
                br.ReadByte();*/

            br.BaseStream.Position = componentName1Ptr;
            result.Component1Name = br.ReadCStr();
            br.BaseStream.Position = componentName2Ptr;
            result.Component2Name = br.ReadCStr();

            return result;
        }

        public void Write(BundleEntry entry)
        {
            
        }
    }
}
