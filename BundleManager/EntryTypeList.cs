using BaseHandlers;
using BundleFormat;
using LangEditor;
using PluginAPI;
using PVSFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultFormat;
using VehicleList;

namespace BundleManager
{
	public static class EntryTypeList
	{
		public static void InitEntryTypes()
		{
			EntryTypeRegistry.Register(EntryType.PolygonSoupListResourceType, new PolygonSoupList());
			EntryTypeRegistry.Register(EntryType.VehicleListResourceType, new VehicleListData());
			EntryTypeRegistry.Register(EntryType.TriggerResourceType, new TriggerData());
			EntryTypeRegistry.Register(EntryType.StreetDataResourceType, new StreetData());
			EntryTypeRegistry.Register(EntryType.ProgressionResourceType, new ProgressionData());
			EntryTypeRegistry.Register(EntryType.IDList, new IDList());
			EntryTypeRegistry.Register(EntryType.TrafficDataResourceType, new Traffic());
			EntryTypeRegistry.Register(EntryType.FlaptFileResourceType, new FlaptFile());
			//EntryTypeRegistry.Register(EntryType.AptDataHeaderType, new AptData());
			EntryTypeRegistry.Register(EntryType.ZoneListResourceType, new PVS());
			EntryTypeRegistry.Register(EntryType.InstanceListResourceType, new InstanceList());
			EntryTypeRegistry.Register(EntryType.GraphicsSpecResourceType, new GraphicsSpec());
			EntryTypeRegistry.Register(EntryType.RwRenderableResourceType, new Renderable());
			EntryTypeRegistry.Register(EntryType.AttribSysVaultResourceType, new AttribSys());
			EntryTypeRegistry.Register(EntryType.LanguageResourceType, new Language());
		}
	}
}
