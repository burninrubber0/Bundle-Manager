using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BundleFormat
{
    public static class BundleCache
    {
        public static string CurrentPath;
        public static List<string> Paths = new List<string>();
        public static Dictionary<ulong, int> Files = new Dictionary<ulong, int>();
        public static Dictionary<ulong, EntryInfo> EntryInfos = new Dictionary<ulong, EntryInfo>();

        public static string GetFileByEntryID(ulong entryID)
        {
            if (!Files.ContainsKey(entryID))
                return null;
            return Paths[Files[entryID]];
        }

        public static EntryInfo GetInfoByEntryID(ulong entryID)
        {
            if (!EntryInfos.ContainsKey(entryID))
                return null;
            return EntryInfos[entryID];
        }

        public static DebugInfo GetEntryDebugInfoByID(ulong entryID)
        {
            if (!EntryInfos.ContainsKey(entryID))
                return default;
            return EntryInfos[entryID].DebugInfo;
        }

        public static string GetEntryNameByID(ulong entryID)
        {
            return GetEntryDebugInfoByID(entryID).Name;
        }

        public static string GetRelativePath(string path)
        {
            string file = path.Replace('\\', '/').Replace(BundleCache.CurrentPath.Replace('\\', '/'), "");

            if (file.StartsWith("/"))
                file = file.Substring(1);

            return file;
        }
    }
}
