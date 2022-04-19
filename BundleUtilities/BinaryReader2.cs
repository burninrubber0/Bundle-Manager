using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundleUtilities
{
    public class Vector3I
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float S { get; set; }
        public Vector3I(float x, float y, float z, float s)
        {
            X = x;
            Y = y;
            Z = z;
            S = s;
        }

        public byte[] toBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(X));
            bytes.AddRange(BitConverter.GetBytes(Y));
            bytes.AddRange(BitConverter.GetBytes(Z));
            bytes.AddRange(BitConverter.GetBytes(S));
            return bytes.ToArray();
        }

    }

    public class BinaryReader2 : BinaryReader
    {
        public bool BigEndian { get; set; }

        public BinaryReader2(Stream input) : base(input)
        {
            BigEndian = false;
        }

        public BinaryReader2(Stream input, Encoding encoding) : base(input, encoding)
        {
            BigEndian = false;
        }

        public BinaryReader2(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
            BigEndian = false;
        }

        private bool ShouldFlip()
        {
            return (BigEndian && BitConverter.IsLittleEndian) || (!BigEndian && !BitConverter.IsLittleEndian);
        }

        public override short ReadInt16()
        {
            var data = base.ReadBytes(2);
            if (data.Length < 2)
                throw new EndOfStreamException();
            if (ShouldFlip())
                Array.Reverse(data);
            return BitConverter.ToInt16(data, 0);
        }

        public override int ReadInt32()
        {
            var data = base.ReadBytes(4);
            if (data.Length < 4)
                throw new EndOfStreamException();
            if (ShouldFlip())
                Array.Reverse(data);
            return BitConverter.ToInt32(data, 0);
        }

        public override long ReadInt64()
        {
            var data = base.ReadBytes(8);
            if (data.Length < 8)
                throw new EndOfStreamException();
            if (ShouldFlip())
                Array.Reverse(data);
            return BitConverter.ToInt64(data, 0);
        }

        public override ushort ReadUInt16()
        {
            var data = base.ReadBytes(2);
            if (data.Length < 2)
                throw new EndOfStreamException();
            if (ShouldFlip())
                Array.Reverse(data);
            return BitConverter.ToUInt16(data, 0);
        }

        public override uint ReadUInt32()
        {
            var data = base.ReadBytes(4);
            if (data.Length < 4)
                throw new EndOfStreamException();
            if (ShouldFlip())
                Array.Reverse(data);
            return BitConverter.ToUInt32(data, 0);
        }

        public override ulong ReadUInt64()
        {
            var data = base.ReadBytes(8);
            if (data.Length < 8)
                throw new EndOfStreamException();
            if (ShouldFlip())
                Array.Reverse(data);
            return BitConverter.ToUInt64(data, 0);
        }

        public override float ReadSingle()
        {
            var data = base.ReadBytes(4);
            if (data.Length < 4)
                throw new EndOfStreamException();
            if (ShouldFlip())
                Array.Reverse(data);
            return BitConverter.ToSingle(data, 0);
        }

        public override double ReadDouble()
        {
            var data = base.ReadBytes(8);
            if (data.Length < 8)
                throw new EndOfStreamException();
            if (ShouldFlip())
                Array.Reverse(data);
            return BitConverter.ToDouble(data, 0);
        }

        public void SkipUniquePadding(int numberOfBytes) {
            base.BaseStream.Position = base.BaseStream.Position + numberOfBytes;
        }

        public void SkipPadding()
        {
            long currentLength = base.BaseStream.Position;
            if (currentLength % 16 != 0)
            {
                base.BaseStream.Position = base.BaseStream.Position + (16 - currentLength % 16);
            };
        }


        public Vector3I ReadVector3I()
        {
            float x = base.ReadSingle();
            float y = base.ReadSingle();
            float z = base.ReadSingle();
            float s = base.ReadSingle();
            return new Vector3I(x, y, z, s);
        }
    }
}
