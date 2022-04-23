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
    public class CountingUtilities
    {
        public static int AddPadding(List<byte[]> bytes)
        {
            if (bytes.SelectMany(i => i).Count() % 16 == 0)
            {
                return bytes.SelectMany(i => i).Count();
            }
            else
            {
                return bytes.SelectMany(i => i).Count() + (16 - (bytes.SelectMany(i => i).Count() % 16));
            }
        }
        public static int AddEightPadding(int value)
        {
            if (value % 8 == 0)
            {
                return value;
            }
            else
            {
                return value + (8 - (value % 8));
            }
        }
    }

    public class SizeAndPositionInformation
    {
        public ulong Hash;
        public ulong EntryTypeHash;
        public int DataChunkSize;
        public int DataChunkPosition;
    }

    public class AttributeHeader
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


        public IAttribute GetAttributeEntity(SizeAndPositionInformation chunk, AttributeHeader dataChunk)
        {
            if (ClassHash == 0x2e3b1dc7d248445e)
            {
                this.ClassName = "physicsvehiclebodyrollattribs";
                return new Physicsvehiclebodyrollattribs(chunk, dataChunk);
            }
            if (ClassHash == 0x52B81656F3ADF675)
            {
                this.ClassName = "burnoutcarasset";
                return new Burnoutcarasset(chunk, dataChunk);
            }
            if (ClassHash == 0xF850281CA54C9B92)
            {
                this.ClassName = "physicsvehicleengineattribs";
                return new Physicsvehicleengineattribs(chunk, dataChunk);
            }
            if (ClassHash == 0x3F9370FCF8D767AC)
            {
                this.ClassName = "physicsvehicledriftattribs";
                return new Physicsvehicledriftattribs(chunk, dataChunk);
            }
            if (ClassHash == 0xDF956BC0568F138C)
            {
                this.ClassName = "physicsvehiclecollisionattribs";
                return new Physicsvehiclecollisionattribs(chunk, dataChunk);
            }
            if (ClassHash == 0x4297B5841F5231CF)
            {
                this.ClassName = "physicsvehiclesuspensionattribs";
                return new Physicsvehiclesuspensionattribs(chunk, dataChunk);
            }
            if (ClassHash == 0x43462C59212A23CC)
            {
                this.ClassName = "physicsvehiclesteeringattribs";
                return new Physicsvehiclesteeringattribs(chunk, dataChunk);
            }
            if (ClassHash == 0xE9EDA3B8C4EA3C84)
            {
                this.ClassName = "cameraexternalbehaviour";
            }
            if (ClassHash == 0xF79C545E141DFFA6)
            {
                this.ClassName = "physicsvehiclebaseattribs";
                return new Physicsvehiclebaseattribs(chunk, dataChunk);
            }
            if (ClassHash == 0xF0FF4DFD660F5A54)
            {
                this.ClassName = "burnoutcargraphicsasset";
                return new Burnoutcargraphicsasset(chunk, dataChunk);
            }
            if (ClassHash == 0xF3E3F8EF855F4F99)
            {
                this.ClassName = "camerabumperbehaviour";
                return new Camerabumperbehaviour(chunk, dataChunk);
            }
            if (ClassHash == 0xEADE7049AF7AB31E)
            {
                this.ClassName = "physicsvehicleboostattribs";
                return new Physicsvehicleboostattribs(chunk, dataChunk);
            }
            if (ClassHash == 0x966121397B502EED)
            {
                this.ClassName = "physicsvehiclehandling";
                return new Physicsvehiclehandling(chunk, dataChunk);
            }
            if (ClassHash == 0x7F161D94482CB3BF)
            {
                this.ClassName = "vehicleengine";
            }
            return new UnimplementedAttribs(chunk, dataChunk);
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
        public List<PtrChunkData> allData;
    }

    public class PtrChunkData
    {
        public uint Ptr;
        public short type;
        public short flag;
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

        public List<IAttribute> Attributes;

        public List<string> Strings;

        public PtrChunk PtrN;

        public byte[] Data;

        public AttribSys()
        {
            Dependencies = new List<string>();
            Attributes = new List<IAttribute>();
            Strings = new List<string>();
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
                        SizeAndPositionInformation chunk = new SizeAndPositionInformation();
                        chunk.Hash = br.ReadUInt64();
                        chunk.EntryTypeHash = br.ReadUInt64();
                        chunk.DataChunkSize = br.ReadInt32();
                        chunk.DataChunkPosition = br.ReadInt32();

                        long pos = br.BaseStream.Position;
                        br.BaseStream.Position = chunk.DataChunkPosition;

                        if (chunk.EntryTypeHash == 0xAD303B8F42B3307E)
                        {
                            AttributeHeader dataChunk = new AttributeHeader();
                            dataChunk.CollectionHash = br.ReadUInt64();
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
                                dataChunk.Items[j] = item;
                            }
                            Attributes.Add(dataChunk.GetAttributeEntity(chunk, dataChunk));
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
                    PtrN.allData = new List<PtrChunkData>();
                    for (int i = 0; i < dataSize / 16; i++)
                    {
                        PtrChunkData data = new PtrChunkData();
                        data.Ptr = br.ReadUInt32();
                        data.type = br.ReadInt16();
                        data.flag = br.ReadInt16();
                        data.Data = br.ReadUInt64();
                        PtrN.allData.Add(data);
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
                int size = br.ReadInt32(); // Size of Strings Array in hex
                while (br.BaseStream.Position < initialPos + size)
                    Strings.Add(br.ReadCStr());
                Strings = Strings.AsEnumerable().Reverse().SkipWhile(str => str.Length == 0).Reverse().ToList();
                br.BaseStream.Position = initialPos + size;

                int dataSize = (int)(br.BaseStream.Length - br.BaseStream.Position);
                Console.WriteLine(dataSize);
                foreach (IAttribute attribute in Attributes)
                {
                    attribute.Read(loader, br);
                }
                Console.WriteLine(Attributes.Sum(attribute => attribute.getDataSize()));
                // Read Data of Unimplemented types
                Data = br.ReadBytes(dataSize - Attributes.Sum(attribute => attribute.getDataSize()));

            }
            catch (IOException ex)
            {
                throw new ReadFailedError("Failed to read bin data", ex.InnerException);
            }
        }

        private void WriteBin(BinaryWriter wr)
        {
            try
            {
                wr.Write(Utilities.Flip(Encoding.ASCII.GetBytes("StrE")));
                wr.Write(getSizeOfStrE());
                foreach (string String in Strings)
                {
                    wr.WriteCStr(String);
                }
                wr.WritePadding();
                foreach (IAttribute attribute in Attributes)
                {
                    attribute.Write(wr);
                }
                // Write Data of Unimplemented types
                wr.Write(Data);
                wr.WritePadding();
            }
            catch (IOException ex)
            {
                throw new ReadFailedError("Failed to write bin data", ex.InnerException);
            }
        }

        public int addPadding(List<byte[]> bytes)
        {
            if (bytes.SelectMany(i => i).Count() % 16 == 0)
            {
                return bytes.SelectMany(i => i).Count();
            }
            else
            {
                return ((bytes.SelectMany(i => i).Count() / 16) * 16) + 16;
            }
        }

        private int getSizeOfStrE()
        {
            List<byte[]> bytes = new List<byte[]>();
            bytes.Add(Utilities.Flip(Encoding.ASCII.GetBytes("StrE")));
            bytes.Add(BitConverter.GetBytes(16));
            foreach (string String in Strings)
            {
                byte[] nullterminating = { (byte)0 };
                bytes.Add(Encoding.ASCII.GetBytes(String).Concat(nullterminating).ToArray());
            }
            return addPadding(bytes);
        }

        private int getSizeOfVers()
        {
            List<byte[]> bytes = new List<byte[]>();
            bytes.Add(Utilities.Flip(Encoding.ASCII.GetBytes("Vers")));
            bytes.Add(BitConverter.GetBytes(16));
            bytes.Add(BitConverter.GetBytes(VersionHash));
            return addPadding(bytes);
        }

        private int getSizeOfDepN()
        {
            List<byte[]> bytes = new List<byte[]>();
            bytes.Add(Utilities.Flip(Encoding.ASCII.GetBytes("DepN")));
            bytes.Add(BitConverter.GetBytes(96));
            bytes.Add(BitConverter.GetBytes((UInt64)Dependencies.Count()));
            bytes.Add(BitConverter.GetBytes(DepHash1));
            bytes.Add(BitConverter.GetBytes(DepHash2));
            bytes.Add(BitConverter.GetBytes(DepNop));
            bytes.Add(BitConverter.GetBytes(Dependencies[0].Length + 1));
            foreach (string d in Dependencies)
            {
                byte[] nullterminating = { (byte)0 };
                bytes.Add(Encoding.ASCII.GetBytes(d).Concat(nullterminating).ToArray());
            }
            return addPadding(bytes);
        }

        private int getSizeOfDatN()
        {
            List<byte[]> bytes = new List<byte[]>();
            bytes.Add(Utilities.Flip(Encoding.ASCII.GetBytes("DatN")));
            foreach (AttributeHeader dataChunk in Attributes.Select(x => x.getHeader()))
            {
                bytes.Add(BitConverter.GetBytes(dataChunk.CollectionHash));
                bytes.Add(BitConverter.GetBytes(dataChunk.ClassHash));
                bytes.Add(dataChunk.Unknown1);
                bytes.Add(BitConverter.GetBytes(dataChunk.ItemCount));
                bytes.Add(BitConverter.GetBytes(dataChunk.Unknown2));
                bytes.Add(BitConverter.GetBytes(dataChunk.ItemCountDup));
                bytes.Add(BitConverter.GetBytes(dataChunk.ParameterCount));
                bytes.Add(BitConverter.GetBytes(dataChunk.ParametersToRead));
                bytes.Add(dataChunk.Unknown3);
                for (int j = 0; j < dataChunk.ParameterCount; j++)
                    bytes.Add(BitConverter.GetBytes((dataChunk.ParameterTypeHashes[j])));
                for (int j = 0; j < (dataChunk.ParametersToRead - dataChunk.ParameterCount); j++)
                {
                    UInt64 zero = 0;
                    bytes.Add(BitConverter.GetBytes(zero)); // zero
                }
                foreach (DataItem item in dataChunk.Items)
                {
                    bytes.Add(BitConverter.GetBytes(item.Hash));
                    bytes.Add(item.Unknown1);
                    bytes.Add(BitConverter.GetBytes(item.ParameterIdx));
                    bytes.Add(BitConverter.GetBytes(item.Unknown2));
                }
            }
            return addPadding(bytes);
        }

        private int getSizeOfExpN()
        {
            List<byte[]> bytes = new List<byte[]>();
            bytes.Add(Utilities.Flip(Encoding.ASCII.GetBytes("ExpN")));
            bytes.Add(BitConverter.GetBytes(16));
            bytes.Add(BitConverter.GetBytes((UInt64)Attributes.Select(x => x.getInfo()).Count()));
            foreach (SizeAndPositionInformation chunk in Attributes.Select(x => x.getInfo()))
            {
                bytes.Add(BitConverter.GetBytes(chunk.Hash));
                bytes.Add(BitConverter.GetBytes(chunk.EntryTypeHash));
                bytes.Add(BitConverter.GetBytes(chunk.DataChunkSize));
                bytes.Add(BitConverter.GetBytes(chunk.DataChunkPosition));
            }
            return addPadding(bytes);
        }

        private int getSizeOfStrN()
        {
            List<byte[]> bytes = new List<byte[]>();
            bytes.Add(Utilities.Flip(Encoding.ASCII.GetBytes("StrN")));
            bytes.Add(BitConverter.GetBytes(16));
            bytes.Add(BitConverter.GetBytes(StrUnknown1));
            return addPadding(bytes);
        }

        private int getSizeOfPtrN()
        {
            List<byte[]> bytes = new List<byte[]>();
            bytes.Add(Utilities.Flip(Encoding.ASCII.GetBytes("PtrN")));
            bytes.Add(BitConverter.GetBytes(16));
            foreach (PtrChunkData data in PtrN.allData)
            {
                bytes.Add(BitConverter.GetBytes(data.Ptr));
                bytes.Add(BitConverter.GetBytes(data.type));
                bytes.Add(BitConverter.GetBytes(data.flag));
                bytes.Add(BitConverter.GetBytes(data.Data));
            }
            return addPadding(bytes);
        }


        private void WriteVlt(BinaryWriter wr)
        {
            try
            {
                wr.Write(Utilities.Flip(Encoding.ASCII.GetBytes("Vers")));
                wr.Write(getSizeOfVers());
                wr.Write(VersionHash);
                wr.WritePadding();

                wr.Write(Utilities.Flip(Encoding.ASCII.GetBytes("DepN")));
                wr.Write(getSizeOfDepN());
                wr.Write((UInt64)Dependencies.Count());
                wr.Write(DepHash1);
                wr.Write(DepHash2);
                wr.Write(DepNop);
                // Have to add null terminating byte to length
                wr.Write(Dependencies[0].Length + 1);
                foreach (string d in Dependencies)
                {
                    wr.WriteCStr(d);
                }
                wr.WritePadding();

                wr.Write(Utilities.Flip(Encoding.ASCII.GetBytes("StrN")));
                wr.Write(getSizeOfStrN());
                wr.Write(StrUnknown1);
                wr.WritePadding();

                wr.Write(Utilities.Flip(Encoding.ASCII.GetBytes("DatN")));
                wr.Write(getSizeOfDatN());
                foreach (AttributeHeader dataChunk in Attributes.Select(x => x.getHeader()))
                {
                    wr.Write(dataChunk.CollectionHash);
                    wr.Write(dataChunk.ClassHash);
                    wr.Write(dataChunk.Unknown1);
                    wr.Write(dataChunk.ItemCount);
                    wr.Write(dataChunk.Unknown2);
                    wr.Write(dataChunk.ItemCountDup);
                    wr.Write(dataChunk.ParameterCount);
                    wr.Write(dataChunk.ParametersToRead);
                    wr.Write(dataChunk.Unknown3);
                    for (int j = 0; j < dataChunk.ParameterCount; j++)
                        wr.Write(dataChunk.ParameterTypeHashes[j]);
                    for (int j = 0; j < (dataChunk.ParametersToRead - dataChunk.ParameterCount); j++)
                    {
                        UInt64 zero = 0;
                        wr.Write(zero); // zero
                    }
                    foreach (DataItem item in dataChunk.Items)
                    {
                        wr.Write(item.Hash);
                        wr.Write(item.Unknown1);
                        wr.Write(item.ParameterIdx);
                        wr.Write(item.Unknown2);
                    }
                }
                wr.WritePadding();

                wr.Write(Utilities.Flip(Encoding.ASCII.GetBytes("ExpN")));
                wr.Write(getSizeOfExpN());
                wr.Write((UInt64)Attributes.Select(x => x.getInfo()).Count());
                foreach (SizeAndPositionInformation chunk in Attributes.Select(x => x.getInfo()))
                {
                    wr.Write(chunk.Hash);
                    wr.Write(chunk.EntryTypeHash);
                    wr.Write(chunk.DataChunkSize);
                    wr.Write(chunk.DataChunkPosition);
                }
                wr.WritePadding();

                wr.Write(Utilities.Flip(Encoding.ASCII.GetBytes("PtrN")));
                wr.Write(getSizeOfPtrN());
                foreach (PtrChunkData data in PtrN.allData)
                {
                    wr.Write(data.Ptr);
                    wr.Write(data.type);
                    wr.Write(data.flag);
                    wr.Write(data.Data);
                }
                wr.WritePadding();
            }
            catch (IOException ex)
            {
                throw new ReadFailedError("Failed to write bin data", ex.InnerException);
            }
        }

        private void Clear()
        {
            VersionHash = default;
            DepHash1 = default;
            DepHash2 = default;
            DepNop = default;
            Attributes.Clear();
            StrUnknown1 = default;

            PtrN = default;

            Dependencies.Clear();
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
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            int vltPos = 16; // Because vlt begins after the sizes
            int vltSize = getSizeOfDatN() + getSizeOfDepN() + getSizeOfExpN() + getSizeOfPtrN() + getSizeOfStrN() + getSizeOfVers();

            bw.Write(vltPos); //vltPos:
            bw.Write(vltSize); //vltSize
            bw.Write(vltSize + vltPos);//binPos;
            bw.Write(CountingUtilities.AddEightPadding(Attributes.Sum(attribute => attribute.getDataSize()) + Data.Length + getSizeOfStrE())); // binSize

            WriteVlt(bw);
            WriteBin(bw);

            bw.Flush();
            byte[] data = ms.ToArray();
            bw.Close();
            ms.Close();

            entry.EntryBlocks[0].Data = data;
            entry.Dirty = true;

            return true;
        }

        public EntryType GetEntryType(BundleEntry entry)
        {
            return EntryType.AttribSysVault;
        }

        public IEntryEditor GetEditor(BundleEntry entry)
        {
            AttribSysVaultForm attribSysVaultForm = new AttribSysVaultForm();
            attribSysVaultForm.AttribSys = this;
            attribSysVaultForm.EditEvent += () =>
            {
                Write(entry);
            };

            return attribSysVaultForm;
        }
    }
}
