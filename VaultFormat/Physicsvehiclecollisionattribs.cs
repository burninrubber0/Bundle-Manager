using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using BundleFormat;
using BundleUtilities;

namespace VaultFormat
{
    public class Physicsvehiclecollisionattribs : IAttribute
    {
        public AttributeHeader header { get; set; }
        public SizeAndPositionInformation info { get; set; }

        public Vector4 BodyBox { get; set; }
        public Physicsvehiclecollisionattribs(SizeAndPositionInformation chunk, AttributeHeader dataChunk)
        {
            header = dataChunk;
            info = chunk;
        }

        public int getDataSize()
        {
            List<byte[]> bytes = new List<byte[]>
            {
                BodyBox.toBytes()
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
            BodyBox = br.ReadVector4();
        }

        public void Write(BinaryWriter wr)
        {
            wr.Write(BodyBox.toBytes());
        }
    }
}
