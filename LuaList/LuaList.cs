using BundleFormat;
using PluginAPI;
using BundleUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LuaList
{
    public class LuaList : IEntryData
    {
        public IEntryEditor GetEditor(BundleEntry entry)
        {
            LuaListEditor luaListEditor = new LuaListEditor();
            luaListEditor.LuaList = this;
            luaListEditor.EditEvent += () =>
            {
                Write(entry);
            };
            return luaListEditor;
        }

        public EntryType GetEntryType(BundleEntry entry)
        {
            return EntryType.LUAList;
        }

        private void Clear()
        {

        }

        public bool Read(BundleEntry entry, ILoader loader = null)
        {
            Clear();

            MemoryStream ms = entry.MakeStream();
            BinaryReader2 br = new BinaryReader2(ms);

            br.Close();
            ms.Close();

            return true;
        }

        public bool Write(BundleEntry entry)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Close();
            ms.Close();

            return true;
        }
    }
}
