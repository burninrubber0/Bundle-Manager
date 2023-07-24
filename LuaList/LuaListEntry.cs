using BundleUtilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System;

namespace LuaList
{
    public enum ScoreMultiplier { 
       StandardPoints = 0,
       DoublePoints = 1 ,
       TriplePoints = 2
    }

    public enum ScoringMethod
    {
        ScoreByPosition = 0,
        ScoreBySuccessOrFail = 1
    }

    public class LuaListEntry
    {
        [TypeConverter(typeof(EncryptedStringConverter))]
        public EncryptedString CgsId { get; set; } = new EncryptedString("SC_1000000");
        public string Name { get; set; } = "";
        public string Goal { get; set; } = "";
        public string Description { get; set; } = "";
        public ScoreMultiplier ScoreMultiplier { get; set; } = ScoreMultiplier.StandardPoints;
        public ScoringMethod ScoringMethod { get; set; } = ScoringMethod.ScoreBySuccessOrFail;
        public int Type { get; set; } = 0;
        public int Variables { get; set; } = 0;

        public int getDataSize() {
            List<byte[]> bytes = new List<byte[]>
            {
                BitConverter.GetBytes(CgsId.Encrypted),
                Encoding.ASCII.GetBytes((Name.PadRight(128, '\0').Substring(0, 128).ToCharArray())),
                Encoding.ASCII.GetBytes((Goal.PadRight(128, '\0').Substring(0, 128).ToCharArray())),
                Encoding.ASCII.GetBytes((Description.PadRight(256, '\0').Substring(0, 256).ToCharArray())),
                BitConverter.GetBytes((int)ScoreMultiplier),
                BitConverter.GetBytes((int)ScoringMethod),
                BitConverter.GetBytes((int)Type),
                BitConverter.GetBytes((int)Variables),
                new byte[8]
            };
            return bytes.SelectMany(i => i).Count();
        }

        public void Read(ILoader loader, BinaryReader2 br) {
            CgsId = new EncryptedString(br.ReadUInt64());
            Name = br.ReadLenString(128);
            Goal = br.ReadLenString(128);
            Description = br.ReadLenString(256);
            ScoreMultiplier = (ScoreMultiplier) br.ReadInt32();
            ScoringMethod = (ScoringMethod) br.ReadInt32();
            Type = br.ReadInt32();
            Variables = br.ReadInt32();
            br.ReadBytes(8); // padding
        }

        public void Write(BinaryWriter wr)
        {
            wr.WriteEncryptedString(CgsId);
            wr.Write(Encoding.ASCII.GetBytes((Name.PadRight(128, '\0').Substring(0, 128).ToCharArray())));
            wr.Write(Encoding.ASCII.GetBytes((Goal.PadRight(128, '\0').Substring(0, 128).ToCharArray())));
            wr.Write(Encoding.ASCII.GetBytes((Description.PadRight(256, '\0').Substring(0, 256).ToCharArray())));
            wr.Write((int)ScoreMultiplier);
            wr.Write((int)ScoringMethod);
            wr.Write(Type);
            wr.Write(Variables);
            wr.WriteUniquePadding(8);
        }
    }
}
