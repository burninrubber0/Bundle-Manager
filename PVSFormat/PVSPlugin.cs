using BundleFormat;
using PluginAPI;

namespace PVSFormat
{
    public class PVSPlugin : Plugin
    {
        public override void Init()
        {
            EntryTypeRegistry.Register(EntryType.ZoneList, new PVS());
        }

        public override string GetID()
        {
            return "pvsplugin";
        }

        public override string GetName()
        {
            return "PVS Resource Handler";
        }
    }
}
