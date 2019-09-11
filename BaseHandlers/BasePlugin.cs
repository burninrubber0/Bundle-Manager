using BundleFormat;
using PluginAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseHandlers
{
	public class BasePlugin : Plugin
	{
		public override void Init()
		{
			EntryTypeRegistry.Register(EntryType.PolygonSoupListResourceType, new PolygonSoupList());
			EntryTypeRegistry.Register(EntryType.TriggerResourceType, new TriggerData());
			EntryTypeRegistry.Register(EntryType.StreetDataResourceType, new StreetData());
			EntryTypeRegistry.Register(EntryType.ProgressionResourceType, new ProgressionData());
			EntryTypeRegistry.Register(EntryType.IDList, new IDList());
			EntryTypeRegistry.Register(EntryType.TrafficDataResourceType, new Traffic());
			EntryTypeRegistry.Register(EntryType.FlaptFileResourceType, new FlaptFile());
			//EntryTypeRegistry.Register(EntryType.AptDataHeaderType, new AptData());
			EntryTypeRegistry.Register(EntryType.InstanceListResourceType, new InstanceList());
			EntryTypeRegistry.Register(EntryType.GraphicsSpecResourceType, new GraphicsSpec());
			EntryTypeRegistry.Register(EntryType.RwRenderableResourceType, new Renderable());
		}

		public override string GetID()
		{
			return "baseplugin";
		}

		public override string GetName()
		{
			return "Base Resource Handlers";
		}
	}
}
