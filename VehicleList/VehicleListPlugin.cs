using BundleFormat;
using PluginAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleList
{
    public class VehicleListPlugin : Plugin
    {
        public override void Init()
        {
            EntryTypeRegistry.Register(EntryType.VehicleList, new VehicleListData());
        }

        public override string GetID()
        {
            return "vehiclelistplugin";
        }

        public override string GetName()
        {
            return "VehicleList Resource Handler";
        }
    }
}
