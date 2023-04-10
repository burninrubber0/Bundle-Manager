using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BundleFormat;
using BundleUtilities;
using PluginAPI;

namespace LangEditor
{
    public class Language : IEntryData
    {
        public uint LanguageID;
        public uint Size;
        public ulong Entries;
        public Dictionary<uint, string> Data;

        public Language()
        {
            Data = new Dictionary<uint, string>();
        }

        private void Clear()
        {
            LanguageID = default;
            Size = default;
            Entries = default;

            Data.Clear();
        }

        public bool Read(BundleEntry entry, ILoader loader = null)
        {
            Clear();

            MemoryStream ms = entry.MakeStream();
            BinaryReader2 br = new BinaryReader2(ms);
            br.BigEndian = entry.Console;

            // Read header
            LanguageID = br.ReadUInt32();
            Size = br.ReadUInt32() - 1; // Exclude the padding string
            Entries = br.ReadUInt64();

            // Read entries and data
            for (int i = 0; i < Size; i++)
            {
                uint id = br.ReadUInt32();
                br.SkipUniquePadding(4);
                string txt = br.ReadCStringPtr();
                Data.Add(id, txt);
            }

            br.Close();
            ms.Close();

            //result.Write(entry);

            return true;
        }

        public bool Write(BundleEntry entry)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            // Write header
            bw.Write(LanguageID);
            bw.Write(Data.Count + 1);
            bw.Write(Entries);
            
            // Write entries
            foreach (uint id in Data.Keys)
            {
                bw.Write(id);
                bw.Write(0); // Padding
                bw.Write((ulong)0); // Offset
            }

            // Write padding entry
            bw.Write(0);
            bw.Write(0); // Padding
            bw.Write((ulong)0); // Offset

            // Write data
            int index = 0;
            foreach (uint id in Data.Keys)
            {
                // Write offset
                long stringStartPos = bw.BaseStream.Position;
                bw.BaseStream.Position = (int)Entries + (index * 0x10) + 8;
                bw.Write(stringStartPos);

                // Write string
                bw.BaseStream.Position = stringStartPos;
                if (Data[id] == null)
                    bw.WriteCStr("");
                else
                    bw.WriteCStr(Data[id]);

                index++;
            }

            // Write padding offset
            long paddingStartPos = bw.BaseStream.Position;
            bw.BaseStream.Position = (int)Entries + (index * 0x10) + 8;
            bw.Write(paddingStartPos);

            // Write data
            bw.BaseStream.Position = paddingStartPos;
            int paddingCount = (int)(entry.EntryBlocks[0].Data.Length - bw.BaseStream.Position);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < paddingCount; i++)
                sb.Append("A");
            bw.WriteCStr(sb.ToString());

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
            return EntryType.Language;
        }

        public IEntryEditor GetEditor(BundleEntry entry)
        {
            LangEdit edit = new LangEdit();
            edit.Lang = this;
            edit.Changed += () =>
            {
                edit.Lang.Write(entry);
            };

            return edit;
        }

        public static uint HashID(string id)
        {
            /*uint result = 0xFFFFFFFF;
            for (int i = 0; i < id.Length; i++)
            {
                byte b = (byte)id[i];
                //if (b == 0)
                //    break;
                byte newByte = (byte)((b - 0x41) & 0xFF);
                if (newByte <= 0x19)
                    b += 0x20;
                byte lowByte = (byte)(result & 0xFF);
                lowByte = (byte)((lowByte ^ b) & 0xFF);
                result = (result >> 8) & 0x00FFFFFF;
                result = (result ^ Crc32.Crc32BTable[lowByte]) & 0xFFFFFFFF;
            }
            result = (~result) & 0xFFFFFFFF;

            return result;*/

            byte[] message = Encoding.ASCII.GetBytes(id);
            UInt32 hash = UInt32.MaxValue;
            for (UInt32 i = 0; i < message.Length; i++)
            {
                UInt32 index = message[i];
                index ^= (hash & 0x000000FF);
                hash = Crc32.Crc32BTable[index] ^ (hash >> 8);
            }

            return hash;
        }
    }
}
