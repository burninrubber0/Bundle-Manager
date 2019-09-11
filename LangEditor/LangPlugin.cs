using BundleFormat;
using PluginAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangEditor
{
	public class LangPlugin : Plugin
	{
		public override void Init()
		{
			EntryTypeRegistry.Register(EntryType.LanguageResourceType, new Language());
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
