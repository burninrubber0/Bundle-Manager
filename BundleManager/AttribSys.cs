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
    public class NestedChunk
    {
        public ulong Hash1;
        public ulong Hash2;
        public int DataChunkSize;
        public int DataChunkPosition;
    }

    public class DataChunk
    {
        public ulong Hash1;
        public ulong Hash2;
        public byte[] Unknown1; // 8 bytes, undetermined data type
        public int Unknown2;
        public byte[] Unknown3; // 4 bytes, undetermined data type
        public int Unknown4;
        public short Unknown5;
        public short Unknown6;
        public byte[] Unknown7; // 8 bytes, undetermined data type
        public ulong[] Unknown8; // 2 or 4 items?
    }

    public class PtrChunk
    {
        public PtrChunkData[] Data;
    }

    public class PtrChunkData
    {
        public int Unknown1;
        public short Unknown2;
        public short Unknown3;
        public long Unknown4;
    }

    public class AttribSys
    {
        public ulong VersionHash;

        public long DepUnknown1;
        public long DepUnknown2;
        public int DepNop;
        public List<string> Dependencies;

        public long StrUnknown1;

        public List<NestedChunk> NestedChunks;
        public List<DataChunk> DataChunks;

        public string String1;
        public string String2;
        public string String3;
        public string String4;

        public PtrChunk PtrN;

        public List<float> FloatBlock1;
        public List<ulong> HashBlock1;
        public List<float> FloatBlock2;
        public List<int> IntBlock;
        public List<short> ShortBlock;
        public List<ulong> HashBlock2;
        public byte[] BytesUnknown;
        public List<float> FloatBlock3;

        public AttribSys()
        {
            Dependencies = new List<string>();
            NestedChunks = new List<NestedChunk>();
            DataChunks = new List<DataChunk>();
            FloatBlock1 = new List<float>();
            HashBlock1 = new List<ulong>();
            FloatBlock2 = new List<float>();
            IntBlock = new List<int>();
            ShortBlock = new List<short>();
            HashBlock2 = new List<ulong>();
            FloatBlock3 = new List<float>();
        }

        private void ReadChunk(ILoader loader, BinaryReader br)
        {
            long initialPos = br.BaseStream.Position;
            string fourcc = Encoding.ASCII.GetString(BitConverter.GetBytes(br.ReadInt32()).Flip());
            int size = br.ReadInt32();

            switch (fourcc)
            {
                case "Vers":
                    VersionHash = br.ReadUInt64();
                    break;
                case "DepN":
                    long entryCount = br.ReadInt64();
                    DepUnknown1 = br.ReadInt64();
                    DepUnknown2 = br.ReadInt64();
                    DepNop = br.ReadInt32();
                    int sz = br.ReadInt32();
                    for (long i = 0; i < entryCount; i++)
                    {
                        string name = br.ReadLenString(sz);
                        Dependencies.Add(name);
                    }

                    break;
                case "StrN":
                    StrUnknown1 = br.ReadInt64();
                    break;
                case "DatN":
                    // Handled in ExpN
                    break;
                case "ExpN":
                    long nestedChunkCount = br.ReadInt64();
                    for (long i = 0; i < nestedChunkCount; i++)
                    {
                        NestedChunk chunk = new NestedChunk();
                        chunk.Hash1 = br.ReadUInt64();
                        chunk.Hash2 = br.ReadUInt64();
                        chunk.DataChunkSize = br.ReadInt32();
                        chunk.DataChunkPosition = br.ReadInt32();
                        NestedChunks.Add(chunk);

                        long pos = br.BaseStream.Position;
                        br.BaseStream.Position = chunk.DataChunkPosition;

                        DataChunk dataChunk = new DataChunk();
                        dataChunk.Hash1 = br.ReadUInt64();
                        dataChunk.Hash2 = br.ReadUInt64();
                        dataChunk.Unknown1 = br.ReadBytes(8);
                        dataChunk.Unknown2 = br.ReadInt32();
                        dataChunk.Unknown3 = br.ReadBytes(4);
                        dataChunk.Unknown4 = br.ReadInt32();
                        dataChunk.Unknown5 = br.ReadInt16();
                        dataChunk.Unknown6 = br.ReadInt16();
                        dataChunk.Unknown7 = br.ReadBytes(8);
                        int unknown8Count = (chunk.DataChunkSize - 48) / sizeof(ulong);
                        dataChunk.Unknown8 = new ulong[unknown8Count];
                        for (int j = 0; j < unknown8Count; j++)
                            dataChunk.Unknown8[j] = br.ReadUInt64();
                        DataChunks.Add(dataChunk);

                        br.BaseStream.Position = pos;
                    }
                    break;
                case "PtrN":
                    // Unknown
                    int dataSize = size - 8;
                    PtrN = new PtrChunk();
                    PtrN.Data = new PtrChunkData[dataSize / 16];
                    for (int i = 0; i < PtrN.Data.Length; i++)
                    {
                        PtrN.Data[i] = new PtrChunkData();
                        PtrN.Data[i].Unknown1 = br.ReadInt32();
                        PtrN.Data[i].Unknown2 = br.ReadInt16();
                        PtrN.Data[i].Unknown3 = br.ReadInt16();
                        PtrN.Data[i].Unknown4 = br.ReadInt64();
                    }

                    break;
                /*case "StrE": // In Binary Section??
                    String1 = br.ReadCString();
                    String2 = br.ReadCString();
                    ExhastID = br.ReadCString();
                    String4 = br.ReadCString();
                    break;*/
                default:
                    throw new ReadFailedError("Unknown Chunk: " + fourcc);
            }

            br.BaseStream.Position = initialPos + size;

            /*long pos = br.BaseStream.Position;

            if (pos % 16 != 0)
            {
                pos += pos % 16;
                br.BaseStream.Position = pos;
            }*/
        }

        private void ReadVlt(ILoader loader, BinaryReader br)
        {
            while (!br.EOF())
            {
                ReadChunk(loader, br);
            }
        }

        private void ReadBin(ILoader loader, BinaryReader br)
        {
            try
            {
                long initialPos = br.BaseStream.Position;
                string fourcc = Encoding.ASCII.GetString(br.ReadBytes(4).Flip());
                int size = br.ReadInt32();

                String1 = br.ReadCStr();
                String2 = br.ReadCStr();
                String3 = br.ReadCStr();
                String4 = br.ReadCStr();

                br.BaseStream.Position = initialPos + size;

                for (int i = 0; i < 108; i++)
                {
                    FloatBlock1.Add(br.ReadSingle());
                }

                for (int i = 0; i < 24; i++)
                {
                    HashBlock1.Add(br.ReadUInt64());
                }

                for (int i = 0; i < 26; i++)
                {
                    FloatBlock2.Add(br.ReadSingle());
                }

                for (int i = 0; i < 3; i++)
                {
                    IntBlock.Add(br.ReadInt32());
                }

                for (int i = 0; i < 10; i++)
                {
                    ShortBlock.Add(br.ReadInt16());
                }

                for (int i = 0; i < 67; i++)
                {
                    HashBlock2.Add(br.ReadUInt64());
                }

                BytesUnknown = br.ReadBytes(4);

                for (int i = 0; i < 103; i++)
                {
                    FloatBlock3.Add(br.ReadSingle());
                }
            }
            catch (IOException ex)
            {
                throw new ReadFailedError("Failed to read bin data", ex.InnerException);
            }
        }

        public static AttribSys Read(BundleEntry entry, ILoader loader = null)
        {
            AttribSys result = new AttribSys();

            MemoryStream ms = entry.MakeStream();
            BinaryReader2 br = new BinaryReader2(ms);
            br.BigEndian = entry.Console;

            int vltPos = br.ReadInt32();
            int vltSize = br.ReadInt32();
            int binPos = br.ReadInt32();
            int binSize = br.ReadInt32();

            br.BaseStream.Position = vltPos;

            byte[] vlt = br.ReadBytes(vltSize);

            MemoryStream vltStream = new MemoryStream(vlt);
            BinaryReader2 vltBr = new BinaryReader2(vltStream);
            vltBr.BigEndian = entry.Console;
            result.ReadVlt(loader, vltBr);
            vltBr.Close();

            br.BaseStream.Position = binPos;

            byte[] bin = br.ReadBytes(binSize);

            MemoryStream binStream = new MemoryStream(bin);
            BinaryReader2 binBr = new BinaryReader2(binStream);
            binBr.BigEndian = entry.Console;
            result.ReadBin(loader, binBr);
            binBr.Close();

            br.Close();
            ms.Close();

            return result;
        }
    }
}
