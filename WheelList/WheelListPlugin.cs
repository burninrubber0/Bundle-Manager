using BundleFormat;
using PluginAPI;

namespace WheelList
{
    public class WheelListPlugin : Plugin
    {
        public override void Init()
        {
            EntryTypeRegistry.Register(EntryType.WheelList, new WheelListData());
        }

        public override string GetID()
        {
            return "wheellistplugin";
        }

        public override string GetName()
        {
            return "WheelList Resource Handler";
        }
    }
}
