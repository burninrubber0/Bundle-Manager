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
		private static readonly Dictionary<ulong, Texture> _cachedTextures = new Dictionary<ulong, Texture>();

		public static void ResetCache()
		{
			/*foreach (ulong key in _cachedTextures.Keys)
			{
				Texture img = _cachedTextures[key];
				img.Dispose();
			}*/
			_cachedTextures.Clear();

			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
		}

		public static void AddToCache(ulong id, Texture image)
		{
			_cachedTextures[id] = image;
		}

		public static bool Contains(ulong key)
		{
			return _cachedTextures.ContainsKey(key);
		}

		public static Texture GetTexture(ulong key)
		{
			return _cachedTextures[key];
		}
	}
}
