using System.Collections.Generic;
using System.IO;
using BundleFormat;
using BundleUtilities;

namespace BundleManager
{
    public struct FlaptSection1Entry
    {
        public byte Unknown00;
        public byte Unknown01;
        public byte Unknown02;
        public byte Unknown03;
        public byte Unknown04;
        public byte Unknown05;
        public byte Unknown06;
        public byte Unknown07;
        public short List1Count;
        public short Unknown0A;
        public int Unknown0C; // 0?

        public uint List1Offset;
        public List<short> List1;
        public uint Unknown14;
        public uint Unknown18;
        public uint Unknown1C;
        public uint Unknown20;
        public uint Unknown24;
        public uint Unknown28;
        public uint Unknown2C;
        public uint Unknown30;
        public uint Unknown34;
        public uint Unknown38;
        public uint Unknown3C;

        public uint NameOffset;
        public string Name;

        public override string ToString() => $"Name: {Name}";
    }

    public struct GeometryVertex
    {
        public float X;
        public float Y;
        public U8Colour Colour;
        public float U;
        public float V;
    }

    // Text or font thing
    public struct TextStyle
    {
        public uint FontNameOffset;
        public string FontName;
        public U8Colour Colour;
        public float TextHeight;

        public override string ToString() => $"Font: {FontName}; Colour: {Colour}; Text height: {TextHeight}";
    }

    public struct FlaptSection5Entry
    {
        public uint Hash;
        public uint CharacterNameOffset;
        public string CharacterName;

        public override string ToString() => $"Hash: 0x{Hash:X}; Character name: {CharacterName}";
    }

    public struct FlaptEventBinding
    {
        public uint CharacterNameOffset;
        public string CharacterName;
        public uint EventNameOffset;
        public string EventName;
        public uint ActionNameOffset;
        public string ActionName;
        public int UnusedInt; // 0?

        public override string ToString() => $"Character: {CharacterName}; Event: {EventName}; Action: {ActionName}";
    }

    public struct StringPointer
    {
        public uint StringOffset;
        public string String;

        public override string ToString() => String;
    }

    public class FlaptFile
    {
        public uint FileSize;
        public float SecondsPerFrame; // 1/30
        public int Section1RecordCount;
        public uint Section1Offset;
        public int ImageIDCount;
        public uint ImageIDListOffset;
        public int GeometryVerticesCount;
        public uint GeometryVerticesOffset;
        public int TextStyleCount;
        public uint TextStyleListOffset;
        public int Section5And6RecordCount;
        public uint Section5Offset;
        public uint Section6Offset;
        public int EventBindsCount;
        public uint EventBindsOffset;
        public int TextStringsCount;
        public uint TextStringsOffset;
        public int DependencyRefStringsCount;
        public uint DependencyRefStringsOffset;
        public int CharacterRefStringsCount;
        public uint CharacterRefStringsOffset;

        public List<FlaptSection1Entry> Section1Entries;
        public List<sbyte> ImageIDs;
        public List<GeometryVertex> GeometryVertices;
        public List<TextStyle> TextStyleEntries;
        public List<FlaptSection5Entry> Section5Entries;
        public List<byte[]> Section6Entries;
        public List<FlaptEventBinding> EventBinds;
        public List<StringPointer> TextStrings;
        public List<StringPointer> DependencyRefStrings;
        public List<StringPointer> CharacterRefStrings;

        public FlaptFile()
        {
            Section1Entries = new List<FlaptSection1Entry>();
            ImageIDs = new List<sbyte>();
            GeometryVertices = new List<GeometryVertex>();
            TextStyleEntries = new List<TextStyle>();
            Section5Entries = new List<FlaptSection5Entry>();
            Section6Entries = new List<byte[]>();
            EventBinds = new List<FlaptEventBinding>();
            TextStrings = new List<StringPointer>();
            DependencyRefStrings = new List<StringPointer>();
            CharacterRefStrings = new List<StringPointer>();
        }

        public static FlaptFile Read(BundleEntry entry)
        {
            FlaptFile result = new FlaptFile();

            MemoryStream ms = entry.MakeStream();
            BinaryReader2 br = new BinaryReader2(ms);
            br.BigEndian = entry.Console;


            br.BaseStream.Position += 4; // magic or something: 0C B0 B0 B0
            result.FileSize = br.ReadUInt32();
            result.SecondsPerFrame = br.ReadSingle();
            result.Section1RecordCount = br.ReadInt32();
            result.Section1Offset = br.ReadUInt32();
            result.ImageIDCount = br.ReadInt32();
            result.ImageIDListOffset = br.ReadUInt32();
            result.GeometryVerticesCount = br.ReadInt32();
            result.GeometryVerticesOffset = br.ReadUInt32();
            result.TextStyleCount = br.ReadInt32();
            result.TextStyleListOffset = br.ReadUInt32();
            result.Section5And6RecordCount = br.ReadInt32();
            result.Section5Offset = br.ReadUInt32();
            result.Section6Offset = br.ReadUInt32();
            result.EventBindsCount = br.ReadInt32();
            result.EventBindsOffset = br.ReadUInt32();
            result.TextStringsCount = br.ReadInt32();
            result.TextStringsOffset = br.ReadUInt32();
            result.DependencyRefStringsCount = br.ReadInt32();
            result.DependencyRefStringsOffset = br.ReadUInt32();
            result.CharacterRefStringsCount = br.ReadInt32();
            result.CharacterRefStringsOffset = br.ReadUInt32();


            br.BaseStream.Position = result.Section1Offset;
            for (int i = 0; i < result.Section1RecordCount; i++)
            {
                FlaptSection1Entry section1Entry = new FlaptSection1Entry();

                section1Entry.Unknown00 = br.ReadByte();
                section1Entry.Unknown01 = br.ReadByte();
                section1Entry.Unknown02 = br.ReadByte();
                section1Entry.Unknown03 = br.ReadByte();
                section1Entry.Unknown04 = br.ReadByte();
                section1Entry.Unknown05 = br.ReadByte();
                section1Entry.Unknown06 = br.ReadByte();
                section1Entry.Unknown07 = br.ReadByte();
                section1Entry.List1Count = br.ReadInt16();
                section1Entry.Unknown0A = br.ReadInt16();
                section1Entry.Unknown0C = br.ReadInt32();

                section1Entry.List1Offset = br.ReadUInt32();
                section1Entry.Unknown14 = br.ReadUInt32();
                section1Entry.Unknown18 = br.ReadUInt32();
                section1Entry.Unknown1C = br.ReadUInt32();
                section1Entry.Unknown20 = br.ReadUInt32();
                section1Entry.Unknown24 = br.ReadUInt32();
                section1Entry.Unknown28 = br.ReadUInt32();
                section1Entry.Unknown2C = br.ReadUInt32();
                section1Entry.Unknown30 = br.ReadUInt32();
                section1Entry.Unknown34 = br.ReadUInt32();
                section1Entry.Unknown38 = br.ReadUInt32();
                section1Entry.Unknown3C = br.ReadUInt32();

                section1Entry.NameOffset = br.ReadUInt32();


                long oldPos = br.BaseStream.Position;

                if (section1Entry.List1Offset != 0)
                {
                    br.BaseStream.Position = section1Entry.List1Offset;
                    section1Entry.List1 = new List<short>(section1Entry.List1Count);
                    for (int j = 0; j < section1Entry.List1Count; j++)
                    {
                        section1Entry.List1.Add(br.ReadInt16());
                    }
                }

                if (section1Entry.NameOffset != 0)
                {
                    br.BaseStream.Position = section1Entry.NameOffset;
                    section1Entry.Name = br.ReadCStr();
                }

                br.BaseStream.Position = oldPos;


                result.Section1Entries.Add(section1Entry);
            }


            br.BaseStream.Position = result.ImageIDListOffset;
            for (int i = 0; i < result.ImageIDCount; i++)
            {
                result.ImageIDs.Add(br.ReadSByte());
                br.BaseStream.Position += 3; // Weird padding?
            }


            br.BaseStream.Position = result.GeometryVerticesOffset;
            for (int i = 0; i < result.GeometryVerticesCount; i++)
            {
                GeometryVertex vertex = new GeometryVertex();

                vertex.X = br.ReadSingle();
                vertex.Y = br.ReadSingle();
                vertex.Colour = U8Colour.FromARGB32(br.ReadUInt32());
                vertex.U = br.ReadSingle();
                vertex.V = br.ReadSingle();

                result.GeometryVertices.Add(vertex);
            }


            br.BaseStream.Position = result.TextStyleListOffset;
            for (int i = 0; i < result.TextStyleCount; i++)
            {
                TextStyle textStyle = new TextStyle();

                textStyle.FontNameOffset = br.ReadUInt32();

                long oldPos = br.BaseStream.Position;
                br.BaseStream.Position = textStyle.FontNameOffset;
                textStyle.FontName = br.ReadCStr();
                br.BaseStream.Position = oldPos;

                textStyle.Colour = U8Colour.FromARGB32(br.ReadUInt32());
                textStyle.TextHeight = br.ReadSingle();

                result.TextStyleEntries.Add(textStyle);
            }


            br.BaseStream.Position = result.Section5Offset;
            for (int i = 0; i < result.Section5And6RecordCount; i++)
            {
                FlaptSection5Entry section5Entry = new FlaptSection5Entry();

                section5Entry.Hash = br.ReadUInt32();
                section5Entry.CharacterNameOffset = br.ReadUInt32();

                long oldPos = br.BaseStream.Position;
                br.BaseStream.Position = section5Entry.CharacterNameOffset;
                section5Entry.CharacterName = br.ReadCStr();
                br.BaseStream.Position = oldPos;

                result.Section5Entries.Add(section5Entry);
            }

            br.BaseStream.Position = result.Section6Offset;
            for (int i = 0; i < result.Section5And6RecordCount; i++)
            {
                byte[] bytes = new byte[33];

                for (int j = 0; j < 33; j++)
                {
                    bytes[j] = br.ReadByte();
                }

                result.Section6Entries.Add(bytes);
            }


            br.BaseStream.Position = result.EventBindsOffset;
            for (int i = 0; i < result.EventBindsCount; i++)
            {
                FlaptEventBinding eventBinding = new FlaptEventBinding();

                eventBinding.CharacterNameOffset = br.ReadUInt32();
                eventBinding.EventNameOffset = br.ReadUInt32();
                eventBinding.ActionNameOffset = br.ReadUInt32();
                eventBinding.UnusedInt = br.ReadInt32();

                long oldPos = br.BaseStream.Position;

                br.BaseStream.Position = eventBinding.CharacterNameOffset;
                eventBinding.CharacterName = br.ReadCStr();

                if (eventBinding.EventNameOffset != 0)
                {
                    br.BaseStream.Position = eventBinding.EventNameOffset;
                    eventBinding.EventName = br.ReadCStr();
                }

                if (eventBinding.ActionNameOffset != 0)
                {
                    br.BaseStream.Position = eventBinding.ActionNameOffset;
                    eventBinding.ActionName = br.ReadCStr();
                }

                br.BaseStream.Position = oldPos;
                
                result.EventBinds.Add(eventBinding);
            }


            br.BaseStream.Position = result.TextStringsOffset;
            for (int i = 0; i < result.TextStringsCount; i++)
            {
                StringPointer textString = new StringPointer();

                textString.StringOffset = br.ReadUInt32();

                long oldPos = br.BaseStream.Position;
                br.BaseStream.Position = textString.StringOffset;
                textString.String = br.ReadCStr();
                br.BaseStream.Position = oldPos;

                result.TextStrings.Add(textString);
            }


            br.BaseStream.Position = result.DependencyRefStringsOffset;
            for (int i = 0; i < result.DependencyRefStringsCount; i++)
            {
                StringPointer dependencyRefString = new StringPointer();

                dependencyRefString.StringOffset = br.ReadUInt32();

                long oldPos = br.BaseStream.Position;
                br.BaseStream.Position = dependencyRefString.StringOffset;
                dependencyRefString.String = br.ReadCStr();
                br.BaseStream.Position = oldPos;

                result.DependencyRefStrings.Add(dependencyRefString);
            }


            br.BaseStream.Position = result.CharacterRefStringsOffset;
            for (int i = 0; i < result.CharacterRefStringsCount; i++)
            {
                StringPointer characterRefString = new StringPointer();

                characterRefString.StringOffset = br.ReadUInt32();

                long oldPos = br.BaseStream.Position;
                br.BaseStream.Position = characterRefString.StringOffset;
                characterRefString.String = br.ReadCStr();
                br.BaseStream.Position = oldPos;

                result.CharacterRefStrings.Add(characterRefString);
            }


            br.Close();
            ms.Close();

            return result;
        }

        public void Write(BundleEntry entry)
        {
            // TODO
        }
    }
}
