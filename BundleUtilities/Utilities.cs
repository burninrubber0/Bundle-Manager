using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundleUtilities
{
    public static class Utilities
    {
        public static bool IsValidPath(string path)
        {
            return !string.IsNullOrEmpty(path);
        }

        public static bool FileExists(string path)
        {
            return IsValidPath(path) && File.Exists(path);
        }
    }
}
