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
        public static Dictionary<uint, int> Files = new Dictionary<uint, int>();

        public static string GetFileByEntryID(uint entryID)
        {
            if (!Files.ContainsKey(entryID))
                return null;
            return Paths[Files[entryID]];
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
