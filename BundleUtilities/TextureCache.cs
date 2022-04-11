using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundleUtilities
{
    public static class TextureCache
    {
        private static readonly Dictionary<ulong, Image> _cachedTextures = new Dictionary<ulong, Image>();

        public static void ResetCache()
        {
            foreach (ulong key in _cachedTextures.Keys)
            {
                Image img = _cachedTextures[key];
                img.Dispose();
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            _cachedTextures.Clear();
        }

        public static void AddToCache(ulong id, Image image)
        {
            _cachedTextures[id] = image;
        }

        public static bool Contains(ulong key)
        {
            return _cachedTextures.ContainsKey(key);
        }

        public static Image GetTexture(ulong key)
        {
            return _cachedTextures[key];
        }
    }
}
