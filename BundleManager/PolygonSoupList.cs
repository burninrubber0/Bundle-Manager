using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BundleFormat;
using MathLib;
using OpenTK;
using StandardExtension;

namespace BundleManager
{
    public class PolygonSoupUnknown
    {
        public uint ID;
        public Vector3 Vertex;

        public PolygonSoupUnknown()
        {
            
        }

        public static PolygonSoupUnknown Read(BinaryReader br)
        {
            PolygonSoupUnknown result = new PolygonSoupUnknown();

            result.ID = br.ReadUInt32();
            result.Vertex = br.ReadVector3();

            return result;
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(ID);
            bw.Write(Vertex);
        }

        public override string ToString()
        {
            return "ID: 0x" + ID.ToString("X8") + ", Vertex: " + Vertex.ToString();
        }
    }

    public class PolygonSoupChunk
    {
        public int Unknown1;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
        public uint IntListStart;
        public uint ShortListStart;
        public short Unknown7;
        public byte Unknown8;
        public byte Unknown9;
        public int Unknown10;

        public List<short> UnknownShorts;
        public List<int> UnknownInts;

        public PolygonSoupChunk()
        {
            UnknownShorts = new List<short>();
            UnknownInts = new List<int>();
        }

        public static PolygonSoupChunk Read(BinaryReader br)
        {
            PolygonSoupChunk result = new PolygonSoupChunk();

            result.Unknown1 = br.ReadInt32();
            result.Unknown2 = br.ReadInt32();
            result.Unknown3 = br.ReadInt32();
            result.Unknown4 = br.ReadInt32();
            result.IntListStart = br.ReadUInt32();
            result.ShortListStart = br.ReadUInt32();
            result.Unknown7 = br.ReadInt16();
            result.Unknown8 = br.ReadByte();
            result.Unknown9 = br.ReadByte();
            result.Unknown10 = br.ReadInt32();

            br.BaseStream.Position = result.ShortListStart;
            while (br.BaseStream.Position < result.IntListStart)
            {
                short s = br.ReadInt16();
                if (s == 0)
                    break;
                result.UnknownShorts.Add(s);
            }

            br.BaseStream.Position = result.IntListStart;
            while (!br.BaseStream.EOF())
            {
                int num = br.ReadInt32();
                if (num == 0)
                    break;
                result.UnknownInts.Add(num);
            }

            return result;
        }

        public void Write(BinaryWriter bw)
        {
            // TODO: Write PolygonSoupChunk
        }
    }

    public class PolygonSoupList
    {
        public float Unknown1;
        public float Unknown2;
        public float Unknown3;
        public int Unknown4;
        public float Unknown5;
        public float Unknown6;
        public float Unknown7;
        public int Unknown8;
        public int ChunkPointerStart;
        public int UnknownSectionStart;
        public int ChunkCount;
        public int FileSize;

        public List<uint> ChunkPointers;
        public List<PolygonSoupUnknown> Unknowns;
        public List<PolygonSoupChunk> Chunks;

        public PolygonSoupList()
        {
            ChunkPointers = new List<uint>();
            Unknowns = new List<PolygonSoupUnknown>();
            Chunks = new List<PolygonSoupChunk>();
        }

        public static PolygonSoupList Read(BundleEntry entry)
        {
            PolygonSoupList result = new PolygonSoupList();

            MemoryStream ms = entry.MakeStream();
            BinaryReader br = new BinaryReader(ms);

            result.Unknown1 = br.ReadSingle();
            result.Unknown2 = br.ReadSingle();
            result.Unknown3 = br.ReadSingle();
            result.Unknown4 = br.ReadInt32();
            result.Unknown5 = br.ReadSingle();
            result.Unknown6 = br.ReadSingle();
            result.Unknown7 = br.ReadSingle();
            result.Unknown8 = br.ReadInt32();
            result.ChunkPointerStart = br.ReadInt32();
            result.UnknownSectionStart = br.ReadInt32();
            result.ChunkCount = br.ReadInt32();
            result.FileSize = br.ReadInt32();

            // No Data
            if (result.ChunkPointerStart == 0)
            {
                br.Close();
                ms.Close();
                return result;
            }

            br.BaseStream.Position = result.ChunkPointerStart;

            for (int i = 0; i < result.ChunkCount; i++)
            {
                result.ChunkPointers.Add(br.ReadUInt32());
            }

            //br.BaseStream.Position += (16 - br.BaseStream.Position % 16);
            br.BaseStream.Position = result.UnknownSectionStart;

            while (!br.BaseStream.EOF())
            {
                PolygonSoupUnknown unk = PolygonSoupUnknown.Read(br);
                if (unk.ID == 0xFFFFFFFF)
                    break;
                result.Unknowns.Add(unk);
            }

            for (int i = 0; i < result.ChunkPointers.Count; i++)
            {
                br.BaseStream.Position = result.ChunkPointers[i];

                result.Chunks.Add(PolygonSoupChunk.Read(br));
            }

            br.Close();
            ms.Close();

            return result;
        }

        public void Write(BundleEntry entry)
        {
            // TODO: Write PolygonSoupList
        }
    }
}
