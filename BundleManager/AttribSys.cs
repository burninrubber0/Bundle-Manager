using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BundleFormat;
using BundleUtilities;

namespace BundleManager
{
    public class NestedChunk
    {
        public ulong Hash;
        public ulong EntryTypeHash;
        public int DataChunkSize;
        public int DataChunkPosition;
    }

    public class DataChunk
    {
        public ulong MethodHash;
        public ulong ClassHash;
        public byte[] Unknown1; // always 0?
        public int ItemCount;
        public int Unknown2;
        public int ItemCountDup; // ?
        public short ParameterCount;
        public short ParametersToRead;
        public byte[] Unknown3; // always 0?
        public ulong[] ParameterTypeHashes;
        public DataItem[] Items;
    }

    public class DataItem
    {
        public ulong Hash;
        public byte[] Unknown1; // 4 bytes, undetermined data type
        public short ParameterIdx; // guess?
        public short Unknown2;
    }

    public class PtrChunk
    {
        public List<PtrChunkData> Mode1Data;
        public List<PtrChunkData> Mode2Data;
    }

    public class PtrChunkData
    {
        public uint Ptr;
        public ulong Data;
    }

    public class AttribSys
    {
        public ulong VersionHash;

        public ulong DepHash1;
        public ulong DepHash2;
        public int DepNop;
        public List<string> Dependencies;

        public long StrUnknown1;

        public List<NestedChunk> NestedChunks;
        public List<DataChunk> DataChunks;

        public List<string> Strings;

        public PtrChunk PtrN;

        public byte[] Data;
        /*public List<float> FloatBlock1;
        public List<ulong> HashBlock1;
        public List<float> FloatBlock2;
        public List<int> IntBlock;
        public List<short> ShortBlock;
        public List<ulong> HashBlock2;
        public byte[] BytesUnknown;
        public List<float> FloatBlock3;*/

        public AttribSys()
        {
            Dependencies = new List<string>();
            NestedChunks = new List<NestedChunk>();
            DataChunks = new List<DataChunk>();
            Strings = new List<string>();
            /*FloatBlock1 = new List<float>();
            HashBlock1 = new List<ulong>();
            FloatBlock2 = new List<float>();
            IntBlock = new List<int>();
            ShortBlock = new List<short>();
            HashBlock2 = new List<ulong>();
            FloatBlock3 = new List<float>();*/
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
                    DepHash1 = br.ReadUInt64();
                    DepHash2 = br.ReadUInt64();
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
                        chunk.Hash = br.ReadUInt64();
                        chunk.EntryTypeHash = br.ReadUInt64();
                        chunk.DataChunkSize = br.ReadInt32();
                        chunk.DataChunkPosition = br.ReadInt32();
                        NestedChunks.Add(chunk);

                        long pos = br.BaseStream.Position;
                        br.BaseStream.Position = chunk.DataChunkPosition;

                        if (chunk.EntryTypeHash == 0xAD303B8F42B3307E)
                        {
                            DataChunk dataChunk = new DataChunk();
                            dataChunk.MethodHash = br.ReadUInt64();
                            dataChunk.ClassHash = br.ReadUInt64();
                            dataChunk.Unknown1 = br.ReadBytes(8);
                            dataChunk.ItemCount = br.ReadInt32();
                            dataChunk.Unknown2 = br.ReadInt32();
                            dataChunk.ItemCountDup = br.ReadInt32();
                            dataChunk.ParameterCount = br.ReadInt16();
                            dataChunk.ParametersToRead = br.ReadInt16();
                            dataChunk.Unknown3 = br.ReadBytes(8);
                            dataChunk.ParameterTypeHashes = new ulong[dataChunk.ParameterCount];
                            for (int j = 0; j < dataChunk.ParameterCount; j++)
                                dataChunk.ParameterTypeHashes[j] = br.ReadUInt64();
                            for (int j = 0; j < (dataChunk.ParametersToRead - dataChunk.ParameterCount); j++)
                                br.ReadUInt64(); // zero
                            dataChunk.Items = new DataItem[dataChunk.ItemCount];
                            for (int j = 0; j < dataChunk.ItemCount; j++)
                            {
                                DataItem item = new DataItem();
                                item.Hash = br.ReadUInt64();
                                item.Unknown1 = br.ReadBytes(4);
                                item.ParameterIdx = br.ReadInt16();
                                item.Unknown2 = br.ReadInt16();
                            }
                            DataChunks.Add(dataChunk);
                        }
                        else
                        {
                            throw new ReadFailedError("Unknown entry type: " + chunk.EntryTypeHash);
                        }

                        br.BaseStream.Position = pos;
                    }
                    break;
                case "PtrN":
                    // Unknown
                    int dataSize = size - 8;
                    PtrN = new PtrChunk();
                    PtrN.Mode1Data = new List<PtrChunkData>();
                    PtrN.Mode2Data = new List<PtrChunkData>();

                    bool mode1 = false;
                    for (int i = 0; i < dataSize / 16; i++)
                    {
                        PtrChunkData data = new PtrChunkData();
                        data.Ptr = br.ReadUInt32();
                        short type = br.ReadInt16();
                        short flag = br.ReadInt16();
                        data.Data = br.ReadUInt64();

                        if (type == 2)
                        {
                            mode1 = (flag == 1);
                        }
                        else if (type == 0)
                        {
                            break;
                        }
                        else if (type == 3)
                        {
                            if (mode1)
                                PtrN.Mode1Data.Add(data);
                            else
                                PtrN.Mode2Data.Add(data);
                        }
                        else
                        {
                            throw new ReadFailedError("Unknown PtrN type: " + type);
                        }
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

                while (br.BaseStream.Position < initialPos + size)
                    Strings.Add(br.ReadCStr());
                Strings = Strings.AsEnumerable().Reverse().SkipWhile(str => str.Length == 0).Reverse().ToList();

                br.BaseStream.Position = initialPos + size;

                int dataSize = (int)(br.BaseStream.Length - br.BaseStream.Position);
                Data = br.ReadBytes(dataSize);
                /*for (int i = 0; i < 108; i++)
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
                }*/
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
