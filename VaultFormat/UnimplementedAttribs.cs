using System.IO;
using BundleUtilities;

namespace VaultFormat
{
    public class UnimplementedAttribs : IAttribute
    {
        public AttributeHeader header;
        public SizeAndPositionInformation info;
        public byte[] data;

        public UnimplementedAttribs(SizeAndPositionInformation chunk, AttributeHeader dataChunk)
        {
            this.info = chunk;
            this.header = dataChunk;
        }

        public int getDataSize()
        {
            return 0;
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
        }

        public void Write(BinaryWriter wr)
        {
        }
    }
}