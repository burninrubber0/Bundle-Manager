using BundleFormat;
using PluginAPI;

namespace LangEditor
{
    public class LangPlugin : Plugin
    {
        public override void Init()
        {
            EntryTypeRegistry.Register(EntryType.Language, new Language());
        }

        public override string GetID()
        {
            return "langplugin";
        }

        public override string GetName()
        {
            return "Language Resource Handler";
        }
    }
}
