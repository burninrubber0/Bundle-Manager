using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BundleFormat;
using BundleUtilities;
using PluginAPI;

namespace VaultFormat
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
        public string ClassName;
        public ulong CollectionHash;
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


        public void SetClassName()
        {
            if (ClassHash == 0x52B81656F3ADF675)
            {
                this.ClassName = "burnoutcarasset";
            }
            if (ClassHash == 0xF850281CA54C9B92)
            {
                this.ClassName = "physicsvehicleengineattribs";
            }
            if (ClassHash == 0x3F9370FCF8D767AC)
            {
                this.ClassName = "physicsvehicledriftattribs";
            }
            if (ClassHash == 0xDF956BC0568F138C)
            {
                this.ClassName = "physicsvehiclecollisionattribs";
            }
            if (ClassHash == 0x4297B5841F5231CF)
            {
                this.ClassName = "physicsvehiclesuspensionattribs";
            }
            if (ClassHash == 0x43462C59212A23CC)
            {
                this.ClassName = "physicsvehiclesteeringattribs";
            }
            if (ClassHash == 0xE9EDA3B8C4EA3C84)
            {
                this.ClassName = "cameraexternalbehaviour";
            }
            if (ClassHash == 0xF79C545E141DFFA6)
            {
                this.ClassName = "physicsvehiclebaseattribs";
            }
            if (ClassHash == 0xF0FF4DFD660F5A54)
            {
                this.ClassName = "burnoutcargraphicsasset";
            }
            if (ClassHash == 0xF3E3F8EF855F4F99)
            {
                this.ClassName = "camerabumperbehaviour";
            }
            if (ClassHash == 0xEADE7049AF7AB31E)
            {
                this.ClassName = "physicsvehicleboostattribs";
            }
            if (ClassHash == 0x966121397B502EED)
            {
                this.ClassName = "physicsvehiclehandling";
            }
            if (ClassHash == 0x7F161D94482CB3BF)
            {
                this.ClassName = "vehicleengine";
            }
        }
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

    public class AttribSys : IEntryData
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

        public AttribSys()
        {
            Dependencies = new List<string>();
            NestedChunks = new List<NestedChunk>();
            DataChunks = new List<DataChunk>();
            Strings = new List<string>();
        }

        private void ReadChunk(ILoader loader, BinaryReader br)
        {
            long initialPos = br.BaseStream.Position;
            string fourcc = Encoding.ASCII.GetString(BitConverter.GetBytes(br.ReadInt32()).Flip());
            int size = br.ReadInt32();
            Console.WriteLine(fourcc);
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
                            dataChunk.CollectionHash = br.ReadUInt64();
                            dataChunk.ClassHash = br.ReadUInt64();
                            dataChunk.SetClassName();
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
                default:
                    throw new ReadFailedError("Unknown Chunk: " + fourcc);
            }

            br.BaseStream.Position = initialPos + size;

        }

        private void ReadVlt(ILoader loader, BinaryReader br)
        {
            while (!br.EOF())
            {
                ReadChunk(loader, br);
            }
        }


        private void ReadBin(ILoader loader, BinaryReader2 br)
        {
            try
            {
                long initialPos = br.BaseStream.Position;
                string fourcc = Encoding.ASCII.GetString(br.ReadBytes(4).Flip());
                // StrE
                int size = br.ReadInt32(); // Size of Strings Array
                while (br.BaseStream.Position < initialPos + size)
                    Strings.Add(br.ReadCStr());
                Strings = Strings.AsEnumerable().Reverse().SkipWhile(str => str.Length == 0).Reverse().ToList();
                br.BaseStream.Position = initialPos + size;

                int dataSize = (int)(br.BaseStream.Length - br.BaseStream.Position);
                Data = br.ReadBytes(dataSize);
            }
            catch (IOException ex)
            {
                throw new ReadFailedError("Failed to read bin data", ex.InnerException);
            }
        }

        private void Clear()
        {
            VersionHash = default;
            DepHash1 = default;
            DepHash2 = default;
            DepNop = default;

            StrUnknown1 = default;

            PtrN = default;

            Data = default;

            Dependencies.Clear();
            NestedChunks.Clear();
            DataChunks.Clear();
            Strings.Clear();
        }

        public bool Read(BundleEntry entry, ILoader loader = null)
        {
            Clear();

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
            ReadVlt(loader, vltBr);
            vltBr.Close();

            br.BaseStream.Position = binPos;

            byte[] bin = br.ReadBytes(binSize);

            MemoryStream binStream = new MemoryStream(bin);
            BinaryReader2 binBr = new BinaryReader2(binStream);
            binBr.BigEndian = entry.Console;
            ReadBin(loader, binBr);
            binBr.Close();

            br.Close();
            ms.Close();

            return true;
        }

        public bool Write(BundleEntry entry)
        {
            return true;
        }

        public EntryType GetEntryType(BundleEntry entry)
        {
            return EntryType.AttribSysVault;
        }

        public IEntryEditor GetEditor(BundleEntry entry)
        {
            // To-Do: Create new Editor based on  something
            return null;
        }
    }
}
