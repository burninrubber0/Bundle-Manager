using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BundleFormat;
using BundleUtilities;
using MathLib;
using OpenTK;
using OpenTK.Input;

namespace BundleManager
{
    public struct Section1Entry
    {
        public float PositionX;
        public float PositionY;
        public float PositionZ;
        public float RotationX;
        public float RotationY;
        public float RotationZ;
        public float SizeX;
        public float SizeY;
        public float SizeZ;
        public int ID;
        public short Number;
        public byte Unknown12;
        public byte Unknown13;
        public int Offset1;
        public byte Unknown15;
        public byte Unknown16;
        public byte Unknown17;
        public byte Unknown18;
    }

    public struct Section2Entry
    {
        public float PositionX;
        public float PositionY;
        public float PositionZ;
        public float RotationX;
        public float RotationY;
        public float RotationZ;
        public float SizeX;
        public float SizeY;
        public float SizeZ;
        public uint ID;
        public short Number;
        public byte Unknown9;
        public byte Unknown10;
        public int Unknown11;
        public int Unknown12;
        public byte Unknown13;
        public byte Unknown14;
        public short Type;
    }

    public struct Section3Entry
    {
        public uint Offset1;
        public int Count1;
        public uint Offset2;
        public int Count2;
    }

    public struct Section4Entry
    {
        public float Unknown1;
        public float Unknown2;
        public float Unknown3;
        public float Unknown4;
        public byte Unknown5;
        public byte Unknown6;
        public byte Unknown7;
        public byte Unknown8;
        public int Unknown9;
        public int Unknown10;
        public int Unknown11;
    }

    public struct Section5Entry
    {
        public float Unknown1;
        public float Unknown2;
        public float Unknown3;
        public float Unknown4;
        public float Unknown5;
        public float Unknown6;
        public float Unknown7;
        public float Unknown8;
        public ulong Offset;
        public byte Unknown9;
        public byte Unknown10;
        public byte Unknown11;
        public byte Unknown12;
        public int Unknown13;
    }

    public class TriggerData
    {
        public int Unknown1;
        public int FileSize;
        public int Unknown3;
        public int Unknown4;
        public float Unknown5;
        public float Unknown6;
        public int Unknown7;
        public int Unknown8;
        public int Unknown9;
        public int Unknown10;
        public int Unknown11;
        public int Unknown12;
        public int Section1Offset;
        public int Section1Count;
        public int Unknown15;
        public uint Section2Offset1;
        public int Section2Count;
        public uint Section3Offset;
        public int Section3Count;
        public int Section4Offset;
        public int Section4Count;
        public int Section5Offset;
        public int Section5Count;
        public int Section6Offset;
        public int Section6Count;
        public int Section7Offset;
        public int Section7Count;
        public int Section8Offset;
        public int Section8Count;
        public int Section9Offset;
        public int Section9Count;
        public int Unknown32;

        public List<Section1Entry> Section1Entries;
        public List<Section2Entry> Section2Entries;
        public List<Section3Entry> Section3Entries;
        public List<Section4Entry> Section4Entries;
        public List<Section5Entry> Section5Entries;
        public List<uint> Section6Entries;

        public TriggerData()
        {
            Section1Entries = new List<Section1Entry>();
            Section2Entries = new List<Section2Entry>();
            Section3Entries = new List<Section3Entry>();
            Section4Entries = new List<Section4Entry>();
            Section5Entries = new List<Section5Entry>();
            Section6Entries = new List<uint>();
        }

        public static TriggerData Read(BundleEntry entry)
        {
            TriggerData result = new TriggerData();

            MemoryStream ms = entry.MakeStream();
            BinaryReader2 br = new BinaryReader2(ms);
            br.BigEndian = entry.Console;

            result.Unknown1 = br.ReadInt32();
            result.FileSize = br.ReadInt32();
            result.Unknown3 = br.ReadInt32();
            result.Unknown4 = br.ReadInt32();
            result.Unknown5 = br.ReadSingle();
            result.Unknown6 = br.ReadSingle();
            result.Unknown7 = br.ReadInt32();
            result.Unknown8 = br.ReadInt32();
            result.Unknown9 = br.ReadInt32();
            result.Unknown10 = br.ReadInt32();
            result.Unknown11 = br.ReadInt32();
            result.Unknown12 = br.ReadInt32();
            result.Section1Offset = br.ReadInt32();
            result.Section1Count = br.ReadInt32();
            result.Unknown15 = br.ReadInt32();
            result.Section2Offset1 = br.ReadUInt32();
            result.Section2Count = br.ReadInt32();
            result.Section3Offset = br.ReadUInt32();
            result.Section3Count = br.ReadInt32();
            result.Section4Offset = br.ReadInt32();
            result.Section4Count = br.ReadInt32();
            result.Section5Offset = br.ReadInt32();
            result.Section5Count = br.ReadInt32();
            result.Section6Offset = br.ReadInt32();
            result.Section6Count = br.ReadInt32();
            result.Section7Offset = br.ReadInt32();
            result.Section7Count = br.ReadInt32();
            result.Section8Offset = br.ReadInt32();
            result.Section8Count = br.ReadInt32();
            result.Section9Offset = br.ReadInt32();
            result.Section9Count = br.ReadInt32();
            result.Unknown32 = br.ReadInt32();

            br.BaseStream.Position = result.Section1Offset;

            for (int i = 0; i < result.Section1Count; i++)
            {
                Section1Entry section1Entry = new Section1Entry();

                section1Entry.PositionX = br.ReadSingle();
                section1Entry.PositionY = br.ReadSingle();
                section1Entry.PositionZ = br.ReadSingle();
                section1Entry.RotationX = br.ReadSingle();
                section1Entry.RotationY = br.ReadSingle();
                section1Entry.RotationZ = br.ReadSingle();
                section1Entry.SizeX = br.ReadSingle();
                section1Entry.SizeY = br.ReadSingle();
                section1Entry.SizeZ = br.ReadSingle();
                section1Entry.ID = br.ReadInt32();
                section1Entry.Number = br.ReadInt16();
                section1Entry.Unknown12 = br.ReadByte();
                section1Entry.Unknown13 = br.ReadByte();
                section1Entry.Offset1 = br.ReadInt32();
                section1Entry.Unknown15 = br.ReadByte();
                section1Entry.Unknown16 = br.ReadByte();
                section1Entry.Unknown17 = br.ReadByte();
                section1Entry.Unknown18 = br.ReadByte();

                result.Section1Entries.Add(section1Entry);
            }

            br.BaseStream.Position = result.Section3Offset;

            for (int i = 0; i < result.Section3Count; i++)
            {
                Section2Entry section2Entry = new Section2Entry();

                section2Entry.PositionX = br.ReadSingle();
                section2Entry.PositionY = br.ReadSingle();
                section2Entry.PositionZ = br.ReadSingle();
                section2Entry.RotationX = br.ReadSingle();
                section2Entry.RotationY = br.ReadSingle();
                section2Entry.RotationZ = br.ReadSingle();
                section2Entry.SizeX = br.ReadSingle();
                section2Entry.SizeY = br.ReadSingle();
                section2Entry.SizeZ = br.ReadSingle();
                section2Entry.ID = br.ReadUInt32();
                section2Entry.Number = br.ReadInt16();
                section2Entry.Unknown9 = br.ReadByte();
                section2Entry.Unknown10 = br.ReadByte();
                section2Entry.Unknown11 = br.ReadInt32();
                section2Entry.Unknown12 = br.ReadInt32();
                section2Entry.Unknown13 = br.ReadByte();
                section2Entry.Unknown14 = br.ReadByte();
                section2Entry.Type = br.ReadInt16();

                result.Section2Entries.Add(section2Entry);
            }

            br.BaseStream.Position = result.Section4Offset;

            for (int i = 0; i < result.Section4Count; i++)
            {
                Section3Entry section3Entry = new Section3Entry();

                section3Entry.Offset1 = br.ReadUInt32();
                section3Entry.Count1 = br.ReadInt32();
                section3Entry.Offset2 = br.ReadUInt32();
                section3Entry.Count2 = br.ReadInt32();

                result.Section3Entries.Add(section3Entry);
            }

            br.BaseStream.Position = result.Section7Offset;

            for (int i = 0; i < result.Section7Count; i++)
            {
                Section4Entry section4Entry = new Section4Entry();

                section4Entry.Unknown1 = br.ReadSingle();
                section4Entry.Unknown2 = br.ReadSingle();
                section4Entry.Unknown3 = br.ReadSingle();
                section4Entry.Unknown4 = br.ReadSingle();
                section4Entry.Unknown5 = br.ReadByte();
                section4Entry.Unknown6 = br.ReadByte();
                section4Entry.Unknown7 = br.ReadByte();
                section4Entry.Unknown8 = br.ReadByte();
                section4Entry.Unknown9 = br.ReadInt32();
                section4Entry.Unknown10 = br.ReadInt32();
                section4Entry.Unknown11 = br.ReadInt32();

                result.Section4Entries.Add(section4Entry);
            }

            br.BaseStream.Position = result.Section8Offset;

            for (int i = 0; i < result.Section8Count; i++)
            {
                Section5Entry section5Entry = new Section5Entry();

                section5Entry.Unknown1 = br.ReadSingle();
                section5Entry.Unknown2 = br.ReadSingle();
                section5Entry.Unknown3 = br.ReadSingle();
                section5Entry.Unknown4 = br.ReadSingle();
                section5Entry.Unknown5 = br.ReadSingle();
                section5Entry.Unknown6 = br.ReadSingle();
                section5Entry.Unknown7 = br.ReadSingle();
                section5Entry.Unknown8 = br.ReadSingle();
                section5Entry.Offset = br.ReadUInt64();
                section5Entry.Unknown9 = br.ReadByte();
                section5Entry.Unknown10 = br.ReadByte();
                section5Entry.Unknown11 = br.ReadByte();
                section5Entry.Unknown12 = br.ReadByte();
                section5Entry.Unknown13 = br.ReadInt32();

                result.Section5Entries.Add(section5Entry);
            }

            br.BaseStream.Position = result.Section9Offset;

            for (int i = 0; i < result.Section9Count; i++)
            {
                uint section6Entry = br.ReadUInt32();
                result.Section6Entries.Add(section6Entry);
            }

            br.Close();
            ms.Close();

            // TODO: Temp
            //result.Write(entry);

            return result;
        }

        public void Write(BundleEntry entry)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(Unknown1);
            long fileSizeOffset = bw.BaseStream.Position;
            bw.Write((int)0);
            bw.Write(Unknown3);
            bw.Write(Unknown4);
            bw.Write(Unknown5);
            bw.Write(Unknown6);
            bw.Write(Unknown7);
            bw.Write(Unknown8);
            bw.Write(Unknown9);
            bw.Write(Unknown10);
            bw.Write(Unknown11);
            bw.Write(Unknown12);
            bw.Write(Section1Offset);
            bw.Write(Section1Count);
            bw.Write(Unknown15);
            bw.Write(Section2Offset1);
            bw.Write(Section2Count);
            bw.Write(Section3Offset);
            bw.Write(Section3Count);
            bw.Write(Section4Offset);
            bw.Write(Section4Count);
            bw.Write(Section5Offset);
            bw.Write(Section5Count);
            bw.Write(Section6Offset);
            bw.Write(Section6Count);
            bw.Write(Section7Offset);
            bw.Write(Section7Count);
            bw.Write(Section8Offset);
            bw.Write(Section8Count);
            bw.Write(Section9Offset);
            bw.Write(Section9Count);
            bw.Write(Unknown32);

            bw.BaseStream.Position = Section1Offset;

            for (int i = 0; i < Section1Entries.Count; i++)
            {
                Section1Entry section1Entry = Section1Entries[i];

                bw.Write(section1Entry.PositionX);
                bw.Write(section1Entry.PositionY);
                bw.Write(section1Entry.PositionZ);
                bw.Write(section1Entry.RotationX);
                bw.Write(section1Entry.RotationY);
                bw.Write(section1Entry.RotationZ);
                bw.Write(section1Entry.SizeX);
                bw.Write(section1Entry.SizeY);
                bw.Write(section1Entry.SizeZ);
                bw.Write(section1Entry.ID);
                bw.Write(section1Entry.Number);
                bw.Write(section1Entry.Unknown12);
                bw.Write(section1Entry.Unknown13);
                bw.Write(section1Entry.Offset1);
                bw.Write(section1Entry.Unknown15);
                bw.Write(section1Entry.Unknown16);
                bw.Write(section1Entry.Unknown17);
                bw.Write(section1Entry.Unknown18);
            }

            bw.BaseStream.Position = Section3Offset;

            for (int i = 0; i < Section2Entries.Count; i++)
            {
                Section2Entry section2Entry = Section2Entries[i];

                bw.Write(section2Entry.PositionX);
                bw.Write(section2Entry.PositionY);
                bw.Write(section2Entry.PositionZ);
                bw.Write(section2Entry.RotationX);
                bw.Write(section2Entry.RotationY);
                bw.Write(section2Entry.RotationZ);
                bw.Write(section2Entry.SizeX);
                bw.Write(section2Entry.SizeY);
                bw.Write(section2Entry.SizeZ);
                bw.Write(section2Entry.ID);
                bw.Write(section2Entry.Number);
                bw.Write(section2Entry.Unknown9);
                bw.Write(section2Entry.Unknown10);
                bw.Write(section2Entry.Unknown11);
                bw.Write(section2Entry.Unknown12);
                bw.Write(section2Entry.Unknown13);
                bw.Write(section2Entry.Unknown14);
                bw.Write(section2Entry.Type);
            }

            bw.BaseStream.Position = Section4Offset;

            for (int i = 0; i < Section3Entries.Count; i++)
            {
                Section3Entry section3Entry = Section3Entries[i];

                bw.Write(section3Entry.Offset1);
                bw.Write(section3Entry.Count1);
                bw.Write(section3Entry.Offset2);
                bw.Write(section3Entry.Count2);
            }

            bw.BaseStream.Position = Section7Offset;

            for (int i = 0; i < Section4Entries.Count; i++)
            {
                Section4Entry section4Entry = Section4Entries[i];

                bw.Write(section4Entry.Unknown1);
                bw.Write(section4Entry.Unknown2);
                bw.Write(section4Entry.Unknown3);
                bw.Write(section4Entry.Unknown4);
                bw.Write(section4Entry.Unknown5);
                bw.Write(section4Entry.Unknown6);
                bw.Write(section4Entry.Unknown7);
                bw.Write(section4Entry.Unknown8);
                bw.Write(section4Entry.Unknown9);
                bw.Write(section4Entry.Unknown10);
                bw.Write(section4Entry.Unknown11);
            }

            bw.BaseStream.Position = Section8Offset;

            for (int i = 0; i < Section5Entries.Count; i++)
            {
                Section5Entry section5Entry = Section5Entries[i];

                bw.Write(section5Entry.Unknown1);
                bw.Write(section5Entry.Unknown2);
                bw.Write(section5Entry.Unknown3);
                bw.Write(section5Entry.Unknown4);
                bw.Write(section5Entry.Unknown5);
                bw.Write(section5Entry.Unknown6);
                bw.Write(section5Entry.Unknown7);
                bw.Write(section5Entry.Unknown8);
                bw.Write(section5Entry.Offset);
                bw.Write(section5Entry.Unknown9);
                bw.Write(section5Entry.Unknown10);
                bw.Write(section5Entry.Unknown11);
                bw.Write(section5Entry.Unknown12);
                bw.Write(section5Entry.Unknown13);
            }

            bw.BaseStream.Position = Section9Offset;

            for (int i = 0; i < Section6Entries.Count; i++)
            {
                uint section6Entry = Section6Entries[i];
                bw.Write(section6Entry);
            }

            long fileSize = bw.BaseStream.Position;
            bw.BaseStream.Position = fileSizeOffset;
            bw.Write((int)fileSize);

            bw.BaseStream.Position = fileSize;

            long paddingCount = 16 - (bw.BaseStream.Position % 16);
            for (int i = 0; i < paddingCount; i++)
                bw.Write((byte) 0);

            bw.Flush();

            byte[] data = ms.ToArray();

            bw.Close();
            ms.Close();

            entry.Header = data;
            entry.Dirty = true;
        }
    }
}
