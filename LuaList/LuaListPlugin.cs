using BundleFormat;
using PluginAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuaList
{

    public class LuaListPlugin : Plugin
    {
        public override void Init()
        {
            EntryTypeRegistry.Register(EntryType.LUAList, new LuaList());
        }

        public override string GetID()
        {
            return "lualistplugin";
        }

        public override string GetName()
        {
            return "Lua List Resource Handler";
        }
        
    }
}
