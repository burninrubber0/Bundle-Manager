using BundleFormat;
using PluginAPI;

namespace VaultFormat
{
    public class AttribsSysVaultPlugin : Plugin
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
