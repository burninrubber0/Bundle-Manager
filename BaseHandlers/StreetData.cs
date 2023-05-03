using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BundleFormat;
using BundleUtilities;
using MathLib;
using OpenTK.Mathematics;
using PluginAPI;

namespace BaseHandlers
{
    public class StreetSection1
    {
        public int Unknown1;
        public short Index;
        public int Unknown3;
        public short Unknown4;
        public short Unknown5;
        public short Unknown6;

        public override string ToString()
        {
            return "{Unk1: " + Unknown1 + ", Index: " + Index + ", Unk3: " + Unknown3 + ", Unk4: " + Unknown4 + ", Unk5: " + Unknown5 + ", Unk6: " + Unknown6 + "}";
        }
    }

    public class StreetSection2
    {
        public int Unknown1;
        public short ID;
        public short Unknown3;
        public byte Unknown4;
        public byte Unknown5;
        public short Unknown6;
        public int Unknown7;
        public int Unknown8;
        public int Unknown9;
        public int Unknown10;
        public int Unknown11;
        public int Unknown12;

        public override string ToString()
        {
            return "{Unk1: " + Unknown1 + ", ID: " + ID + ", Unk3: " + Unknown3 + ", Unk4: " + Unknown4 + ", Unk5: " + Unknown5 + ", Unk6: " + Unknown6 + ", Unk7: " + Unknown7 + ", Unk8: " + Unknown8 + ", Unk9: " + Unknown9 + ", Unk10: " + Unknown10 + ", Unk11: " + Unknown11 + ", Unk12: " + Unknown12 + "}";
        }
    }

    public class StreetInfo
    {
        public Vector3 Coords;
        public int Unknown4;
        public long StreetID;
        public long Unknown6;
        public long Unknown7;
        public string StreetNameID;
        public int Unknown8;
        public int Unknown9; // PC Only, always 1
        public int Unknown10;

        public override string ToString()
        {
            return "{Coords: " + Coords + ", Unk4: " + Unknown4 + ", Unk5: " + StreetID + ", Unk6: " + Unknown6 + ", Unk7: " + Unknown7 + ", StreetNameID: " + StreetNameID + ", Unk8: " + Unknown8 + ", Unk9: " + Unknown9 + ", Unk10: " + Unknown10 + "}";
        }
    }

    public class RoadRuleInfo
    {
        public long Unknown1;
        public long Unknown2;
        public int Time;
        public int ShowTime;
        public long Unknown5;
        public long Unknown6;

        public override string ToString()
        {
            return "{Unk1: " + Unknown1 + ", Unk2: " + Unknown2 + ", Time: " + Time + ", ShowTime: " + ShowTime + ", Unk5: " + Unknown5 + ", Unk6: " + Unknown6 + "}";
        }
    }

    public class StreetSection5
    {
        public short Section1Index;
        public short Unknown2;
        public short Unknown3;
        public short Unknown4;

        public override string ToString()
        {
            return "{Section1Index: " + Section1Index + ", Unk2: " + Unknown2 + ", Unk3: " + Unknown3 + ", Unk4: " + Unknown4 + "}";
        }
    }

    public class StreetData : IEntryData
    {
        public int Unknown1;
        private int FileSize;
        public int Unknown3;
        private int Section2Offset;
        private int StreetOffset;
        private int RoadRuleOffset;
        private int Section1Count;
        private int RoadRuleCount;
        private int StreetCount;
        public int Unknown9;
        public int Unknown10;
        public int Unknown11;
        public List<StreetSection1> StreetSection1s;
        public List<StreetSection2> StreetSection2s;
        public List<StreetInfo> StreetInfos;
        public List<RoadRuleInfo> RoadRuleInfos;
        public List<StreetSection5> StreetSection5s;

        public StreetData()
        {
            StreetSection1s = new List<StreetSection1>();
            StreetSection2s = new List<StreetSection2>();
            StreetInfos = new List<StreetInfo>();
            RoadRuleInfos = new List<RoadRuleInfo>();
            StreetSection5s = new List<StreetSection5>();
        }

        private void Clear()
        {
            Unknown1 = default;
            FileSize = default;
            Unknown3 = default;
            Section2Offset = default;
            StreetOffset = default;
            RoadRuleOffset = default;
            Section1Count = default;
            RoadRuleCount = default;
            StreetCount = default;
            Unknown9 = default;
            Unknown10 = default;
            Unknown11 = default;

            StreetSection1s.Clear();
            StreetSection2s.Clear();
            StreetInfos.Clear();
            RoadRuleInfos.Clear();
            StreetSection5s.Clear();
        }

        public bool Read(BundleEntry entry, ILoader loader = null)
        {
            Clear();

            MemoryStream ms = entry.MakeStream();
            BinaryReader2 br = new BinaryReader2(ms);
            br.BigEndian = entry.Console;

            Unknown1 = br.ReadInt32();
            FileSize = br.ReadInt32();
            Unknown3 = br.ReadInt32();
            Section2Offset = br.ReadInt32();
            StreetOffset = br.ReadInt32();
            RoadRuleOffset = br.ReadInt32();
            Section1Count = br.ReadInt32();
            RoadRuleCount = br.ReadInt32();
            StreetCount = br.ReadInt32();
            Unknown9 = br.ReadInt32();
            Unknown10 = br.ReadInt32();
            Unknown11 = br.ReadInt32();

            for (int i = 0; i < Section1Count; i++)
            {
                StreetSection1 streetSection1 = new StreetSection1();

                streetSection1.Unknown1 = br.ReadInt32();
                streetSection1.Index = br.ReadInt16();
                streetSection1.Unknown3 = br.ReadInt32();
                streetSection1.Unknown4 = br.ReadInt16();
                streetSection1.Unknown5 = br.ReadInt16();
                streetSection1.Unknown6 = br.ReadInt16();

                StreetSection1s.Add(streetSection1);
            }

            br.BaseStream.Seek(Section2Offset, SeekOrigin.Begin);

            for (int i = 0; i < RoadRuleCount; i++)
            {
                StreetSection2 section2 = new StreetSection2();

                section2.Unknown1 = br.ReadInt32();
                section2.ID = br.ReadInt16();
                section2.Unknown3 = br.ReadInt16();
                section2.Unknown4 = br.ReadByte();
                section2.Unknown5 = br.ReadByte();
                section2.Unknown6 = br.ReadInt16();
                section2.Unknown7 = br.ReadInt32();
                section2.Unknown8 = br.ReadInt32();
                section2.Unknown9 = br.ReadInt32();
                section2.Unknown10 = br.ReadInt32();
                section2.Unknown11 = br.ReadInt32();
                section2.Unknown12 = br.ReadInt32();

                StreetSection2s.Add(section2);
            }

            br.BaseStream.Seek(StreetOffset, SeekOrigin.Begin);

            for (int i = 0; i < StreetCount; i++)
            {
                StreetInfo section3 = new StreetInfo();

                section3.Coords = br.ReadVector3F();
                section3.Unknown4 = br.ReadInt32();
                section3.StreetID = br.ReadInt64();
                section3.Unknown6 = br.ReadInt64();
                section3.Unknown7 = br.ReadInt64();
                section3.StreetNameID = Encoding.ASCII.GetString(br.ReadBytes(20));
                section3.Unknown8 = br.ReadInt32();

                //if (entry.Console)
                //    section3.Unknown9 = 1;
                //else
                section3.Unknown9 = br.ReadInt32();
                section3.Unknown10 = br.ReadInt32();

                StreetInfos.Add(section3);
            }
            
            br.BaseStream.Seek(RoadRuleOffset, SeekOrigin.Begin);

            for (int i = 0; i < RoadRuleCount; i++)
            {
                RoadRuleInfo section4 = new RoadRuleInfo();

                section4.Unknown1 = br.ReadInt64();
                section4.Unknown2 = br.ReadInt64();
                section4.Time = br.ReadInt32();
                section4.ShowTime = br.ReadInt32();
                section4.Unknown5 = br.ReadInt64();
                section4.Unknown6 = br.ReadInt64();

                RoadRuleInfos.Add(section4);
            }

            while (br.BaseStream.Position < FileSize)
            {
                StreetSection5 section5 = new StreetSection5();

                section5.Section1Index = br.ReadInt16();
                section5.Unknown2 = br.ReadInt16();
                section5.Unknown3 = br.ReadInt16();
                section5.Unknown4 = br.ReadInt16();

                StreetSection5s.Add(section5);
            }

            // TODO: TEMP
            //StreetInfo info1 = result.StreetInfos[1];
            //StreetInfo info70 = result.StreetInfos[70];
            //result.StreetInfos[1] = info70;
            //result.StreetInfos[70] = info1;
            //result.Write(entry);

            return true;
        }

        public bool Write(BundleEntry entry)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(Unknown1);
            long fileSizeOffset = bw.BaseStream.Position;
            bw.Write((int)0);
            bw.Write(Unknown3);
            long section2Offset = bw.BaseStream.Position;
            bw.Write((int)0);
            long streetOffset = bw.BaseStream.Position;
            bw.Write((int)0);
            long roadRuleOffset = bw.BaseStream.Position;
            bw.Write((int)0);
            bw.Write(StreetSection1s.Count);
            bw.Write(StreetSection2s.Count);
            bw.Write(StreetInfos.Count);
            bw.Write(Unknown9);
            bw.Write(Unknown10);
            bw.Write(Unknown11);

            for (int i = 0; i < StreetSection1s.Count; i++)
            {
                StreetSection1 streetSection1 = StreetSection1s[i];

                bw.Write(streetSection1.Unknown1);
                bw.Write(streetSection1.Index);
                bw.Write(streetSection1.Unknown3);
                bw.Write(streetSection1.Unknown4);
                bw.Write(streetSection1.Unknown5);
                bw.Write(streetSection1.Unknown6);
            }

            long newPos = bw.BaseStream.Position;
            bw.BaseStream.Seek(section2Offset, SeekOrigin.Begin);
            bw.Write((int)newPos);

            bw.BaseStream.Seek(newPos, SeekOrigin.Begin);

            for (int i = 0; i < StreetSection2s.Count; i++)
            {
                StreetSection2 section2 = StreetSection2s[i];

                bw.Write(section2.Unknown1);
                bw.Write(section2.ID);
                bw.Write(section2.Unknown3);
                bw.Write(section2.Unknown4);
                bw.Write(section2.Unknown5);
                bw.Write(section2.Unknown6);
                bw.Write(section2.Unknown7);
                bw.Write(section2.Unknown8);
                bw.Write(section2.Unknown9);
                bw.Write(section2.Unknown10);
                bw.Write(section2.Unknown11);
                bw.Write(section2.Unknown12);
            }

            long paddingCount = 16 - (bw.BaseStream.Position % 16);
            bw.BaseStream.Position += paddingCount;

            newPos = bw.BaseStream.Position;
            bw.BaseStream.Seek(streetOffset, SeekOrigin.Begin);
            bw.Write((int)newPos);

            bw.BaseStream.Seek(newPos, SeekOrigin.Begin);

            for (int i = 0; i < StreetInfos.Count; i++)
            {
                StreetInfo section3 = StreetInfos[i];

                bw.Write(section3.Coords);
                bw.Write(section3.Unknown4);

                // TODO: TEMP
                //if (section3.StreetID == 808305)
                //    section3.StreetID = 383595;
                
                bw.Write(section3.StreetID);
                bw.Write(section3.Unknown6);
                bw.Write(section3.Unknown7);
                bw.Write(Encoding.ASCII.GetBytes(section3.StreetNameID).Pad(20));
                bw.Write(section3.Unknown8);
                bw.Write(section3.Unknown9); // PC Only
                bw.Write(section3.Unknown10); // PC Only
            }

            newPos = bw.BaseStream.Position;
            bw.BaseStream.Seek(roadRuleOffset, SeekOrigin.Begin);
            bw.Write((int)newPos);

            bw.BaseStream.Seek(newPos, SeekOrigin.Begin);

            for (int i = 0; i < RoadRuleInfos.Count; i++)
            {
                RoadRuleInfo section4 = RoadRuleInfos[i];

                bw.Write(section4.Unknown1);
                bw.Write(section4.Unknown2);
                bw.Write(section4.Time);
                bw.Write(section4.ShowTime);
                bw.Write(section4.Unknown5);
                bw.Write(section4.Unknown6);
            }

            for (int i = 0; i < StreetSection5s.Count; i++)
            {
                StreetSection5 section5 = StreetSection5s[i];

                bw.Write(section5.Section1Index);
                bw.Write(section5.Unknown2);
                bw.Write(section5.Unknown3);
                bw.Write(section5.Unknown4);
            }

            newPos = bw.BaseStream.Position;
            bw.BaseStream.Seek(fileSizeOffset, SeekOrigin.Begin);
            bw.Write((int)newPos);

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
            return EntryType.StreetData;
        }

        public IEntryEditor GetEditor(BundleEntry entry)
        {
            return null;
        }
    }
}
