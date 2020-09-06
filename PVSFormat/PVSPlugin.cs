using BundleFormat;
using PluginAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
