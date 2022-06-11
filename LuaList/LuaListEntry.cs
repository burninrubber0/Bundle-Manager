using BundleUtilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System;

namespace LuaList
{
    public class LuaListEntry
    {
        [TypeConverter(typeof(EncryptedStringConverter))]
        public EncryptedString CgsId { get; set; }
        public string Name { get; set; } = "";
        public string Goal { get; set; } = "";
        public string Description { get; set; } = "";
        [Category("Undefined Datastructure"), Description("This is currently not implemented and can be ignored")]
        public string unknown1 { get; set; } = "";
        [Category("Undefined Datastructure"), Description("This is currently not implemented and can be ignored")]
        public int unknown2 { get; set; } = 0;
        [Category("Undefined Datastructure"), Description("This is currently not implemented and can be ignored")]
        public int unknown3 { get; set; } = 0;
        public int Type { get; set; } = 0;
        public int Variables { get; set; } = 0;

        public int getDataSize() {
            List<byte[]> bytes = new List<byte[]>();
            bytes.Add(BitConverter.GetBytes(CgsId.Encrypted));
            bytes.Add(Encoding.ASCII.GetBytes((Name.PadRight(128, '\0').Substring(0, 128).ToCharArray())));
            bytes.Add(Encoding.ASCII.GetBytes((Goal.PadRight(128, '\0').Substring(0, 128).ToCharArray())));
            bytes.Add(Encoding.ASCII.GetBytes((Description.PadRight(128, '\0').Substring(0, 128).ToCharArray())));
            bytes.Add(Encoding.ASCII.GetBytes((unknown1.PadRight(128, '\0').Substring(0, 128).ToCharArray())));
            bytes.Add(BitConverter.GetBytes(unknown2));
            bytes.Add(BitConverter.GetBytes(unknown3));
            bytes.Add(BitConverter.GetBytes((int)Type));
            bytes.Add(BitConverter.GetBytes((int)Variables));
            bytes.Add(new byte[8]);
            return bytes.SelectMany(i => i).Count();
        }

        public void Read(ILoader loader, BinaryReader2 br) {
            CgsId = br.ReadEncryptedString();
            Name = br.ReadLenString(128);
            Goal = br.ReadLenString(128);
            Description = br.ReadLenString(128);
            unknown1 = br.ReadLenString(128);
            unknown2 = br.ReadInt32();
            unknown3 = br.ReadInt32();
            Type = br.ReadInt32();
            Variables = br.ReadInt32();
            br.ReadBytes(8); // padding
        }

        public void Write(BinaryWriter wr)
        {
            wr.WriteEncryptedString(CgsId);
            wr.Write(Encoding.ASCII.GetBytes((Name.PadRight(128, '\0').Substring(0, 128).ToCharArray())));
            wr.Write(Encoding.ASCII.GetBytes((Goal.PadRight(128, '\0').Substring(0, 128).ToCharArray())));
            wr.Write(Encoding.ASCII.GetBytes((Description.PadRight(128, '\0').Substring(0, 128).ToCharArray())));
            wr.Write(Encoding.ASCII.GetBytes((unknown1.PadRight(128, '\0').Substring(0, 128).ToCharArray())));
            wr.Write(unknown2);
            wr.Write(unknown3);
            wr.Write(Type);
            wr.Write(Variables);
            wr.WriteUniquePadding(8);
        }
    }
}
