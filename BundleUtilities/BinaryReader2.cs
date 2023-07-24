using System.Buffers.Binary;
using System.IO;
using System.Numerics;

namespace BundleUtilities
{
    public class BinaryReader2 : BinaryReader
    {
        public bool BigEndian { get; set; } = false;

        public BinaryReader2(Stream input) : base(input)
        {
            
        }

        public override short ReadInt16()
        {
            if (BaseStream.Length - BaseStream.Position < 2)
                throw new EndOfStreamException();
            if (BigEndian)
                return BinaryPrimitives.ReadInt16BigEndian(ReadBytes(2));
            return BinaryPrimitives.ReadInt16LittleEndian(ReadBytes(2));
        }

        public override int ReadInt32()
        {
            if (BaseStream.Length - BaseStream.Position < 4)
                throw new EndOfStreamException();
            if (BigEndian)
                return BinaryPrimitives.ReadInt32BigEndian(ReadBytes(4));
            return BinaryPrimitives.ReadInt32LittleEndian(ReadBytes(4));
        }

        public override long ReadInt64()
        {
            if (BaseStream.Length - BaseStream.Position < 8)
                throw new EndOfStreamException();
            if (BigEndian)
                return BinaryPrimitives.ReadInt64BigEndian(ReadBytes(8));
            return BinaryPrimitives.ReadInt64LittleEndian(ReadBytes(8));
        }

        public override ushort ReadUInt16()
        {
            if (BaseStream.Length - BaseStream.Position < 2)
                throw new EndOfStreamException();
            if (BigEndian)
                return BinaryPrimitives.ReadUInt16BigEndian(ReadBytes(2));
            return BinaryPrimitives.ReadUInt16LittleEndian(ReadBytes(2));
        }

        public override uint ReadUInt32()
        {
            if (BaseStream.Length - BaseStream.Position < 4)
                throw new EndOfStreamException();
            if (BigEndian)
                return BinaryPrimitives.ReadUInt32BigEndian(ReadBytes(4));
            return BinaryPrimitives.ReadUInt32LittleEndian(ReadBytes(4));
        }

        public override ulong ReadUInt64()
        {
            if (BaseStream.Length - BaseStream.Position < 8)
                throw new EndOfStreamException();
            if (BigEndian)
                return BinaryPrimitives.ReadUInt64BigEndian(ReadBytes(8));
            return BinaryPrimitives.ReadUInt64LittleEndian(ReadBytes(8));
        }

        public override float ReadSingle()
        {
            if (BaseStream.Length - BaseStream.Position < 4)
                throw new EndOfStreamException();
            if (BigEndian)
                return BinaryPrimitives.ReadSingleBigEndian(ReadBytes(8));
            return BinaryPrimitives.ReadSingleLittleEndian(ReadBytes(4));
        }

        public override double ReadDouble()
        {
            if (BaseStream.Length - BaseStream.Position < 8)
                throw new EndOfStreamException();
            if (BigEndian)
                return BinaryPrimitives.ReadDoubleBigEndian(ReadBytes(8));
            return BinaryPrimitives.ReadDoubleLittleEndian(ReadBytes(8));
        }

        public Vector4 ReadVector4()
        {
            return new Vector4(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        public void SkipUniquePadding(int numberOfBytes) {
            BaseStream.Position = BaseStream.Position + numberOfBytes;
        }

        public void SkipPadding()
        {
            long currentLength = BaseStream.Position;
            if (currentLength % 16 != 0)
            {
                BaseStream.Position = BaseStream.Position + (16 - currentLength % 16);
            };
        }
    }
}
