using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BundleUtilities;

namespace VaultFormat
{
    public class Physicsvehicleboostattribs : IAttribute
    {
        public AttributeHeader header { get; set; }
        public SizeAndPositionInformation info { get; set; }
        public float MaxBoostSpeed { get; set; }
        public int BoostRule { get; set; }
        public float BoostKickTime { get; set; }
        public float BoostKickMinTime { get; set; }
        public float BoostKickMaxTime { get; set; }
        public float BoostKickMaxStartSpeed { get; set; }
        public float BoostKickHeightOffset { get; set; }
        public float BoostKickAcceleration { get; set; }
        public float BoostKick { get; set; }
        public float BoostHeightOffset { get; set; }
        public float BoostBase { get; set; }
        public float BoostAcceleration { get; set; }
        public float BlueMaxBoostSpeed { get; set; }
        public float BlueBoostKickTime { get; set; }
        public float BlueBoostKick { get; set; }
        public float BlueBoostBase { get; set; }

        public Physicsvehicleboostattribs(SizeAndPositionInformation chunk, AttributeHeader dataChunk)
        {
            header = dataChunk;
            info = chunk;
        }

        public int getDataSize()
        {
            List<byte[]> bytes = new List<byte[]>
            {
                BitConverter.GetBytes(MaxBoostSpeed),
                BitConverter.GetBytes(BoostRule),
                BitConverter.GetBytes(BoostKickTime),
                BitConverter.GetBytes(BoostKickMinTime),
                BitConverter.GetBytes(BoostKickMaxTime),
                BitConverter.GetBytes(BoostKickMaxStartSpeed),
                BitConverter.GetBytes(BoostKickHeightOffset),
                BitConverter.GetBytes(BoostKickAcceleration),
                BitConverter.GetBytes(BoostKick),
                BitConverter.GetBytes(BoostHeightOffset),
                BitConverter.GetBytes(BoostBase),
                BitConverter.GetBytes(BoostAcceleration),
                BitConverter.GetBytes(BlueMaxBoostSpeed),
                BitConverter.GetBytes(BlueBoostKickTime),
                BitConverter.GetBytes(BlueBoostKick),
                BitConverter.GetBytes(BlueBoostBase)
            };
            Console.WriteLine(bytes.SelectMany(i => i).Count());
            return CountingUtilities.AddPadding(bytes);
        }

        public AttributeHeader getHeader()
        {
            return header;
        }

        public SizeAndPositionInformation getInfo()
        {
            return info;
        }

        public void Read(ILoader loader, BinaryReader2 br)
        {
            MaxBoostSpeed = br.ReadSingle();
            BoostRule = br.ReadInt32();
            BoostKickTime = br.ReadSingle();
            BoostKickMinTime = br.ReadSingle();
            BoostKickMaxTime = br.ReadSingle();
            BoostKickMaxStartSpeed = br.ReadSingle();
            BoostKickHeightOffset = br.ReadSingle();
            BoostKickAcceleration = br.ReadSingle();
            BoostKick = br.ReadSingle();
            BoostHeightOffset = br.ReadSingle();
            BoostBase = br.ReadSingle();
            BoostAcceleration = br.ReadSingle();
            BlueMaxBoostSpeed = br.ReadSingle();
            BlueBoostKickTime = br.ReadSingle();
            BlueBoostKick = br.ReadSingle();
            BlueBoostBase = br.ReadSingle();
        }

        public void Write(BinaryWriter wr)
        {
            wr.Write(MaxBoostSpeed);
            wr.Write(BoostRule);
            wr.Write(BoostKickTime);
            wr.Write(BoostKickMinTime);
            wr.Write(BoostKickMaxTime);
            wr.Write(BoostKickMaxStartSpeed);
            wr.Write(BoostKickHeightOffset);
            wr.Write(BoostKickAcceleration);
            wr.Write(BoostKick);
            wr.Write(BoostHeightOffset);
            wr.Write(BoostBase);
            wr.Write(BoostAcceleration);
            wr.Write(BlueMaxBoostSpeed);
            wr.Write(BlueBoostKickTime);
            wr.Write(BlueBoostKick);
            wr.Write(BlueBoostBase);
        }
    }
}
