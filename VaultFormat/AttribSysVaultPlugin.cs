using BundleFormat;
using PluginAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultFormat;

namespace VaultFormat
{
    public class VehicleListPlugin : Plugin
    {
        public override void Init()
        {
            EntryTypeRegistry.Register(EntryType.AttribSysVault, new AttribSys());
        }

        public override string GetID()
        {
            return "attribsysplugin";
        }

        public override string GetName()
        {
            return "Attribsys Resource Handler";
        }
    }
}
