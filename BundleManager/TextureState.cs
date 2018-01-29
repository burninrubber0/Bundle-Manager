using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BundleFormat;
using BundleUtilities;
using BurnoutImage;

namespace BundleManager
{
    public class TextureState
    {
        private static readonly Dictionary<ulong, Image> _cachedTextures = new Dictionary<ulong, Image>();

        public Image Texture;

        public TextureState()
        {
            
        }

        public static void ResetCache()
        {
            foreach (uint key in _cachedTextures.Keys)
            {
                Image img = _cachedTextures[key];
                img.Dispose();
            }
            _cachedTextures.Clear();
        }

        public static TextureState Read(BundleEntry entry)
        {
            TextureState result = new TextureState();

            List<BundleDependency> dependencies = entry.GetDependencies();
            foreach (BundleDependency dependency in dependencies)
            {
                ulong id = dependency.EntryID;

                if (_cachedTextures.ContainsKey(id))
                {
                    result.Texture = _cachedTextures[id];
                }
                else
                {
                    BundleEntry descEntry1 = entry.Archive.GetEntryByID(id);
                    if (descEntry1 == null)
                    {
                        string file = BundleCache.GetFileByEntryID(id);
                        if (!string.IsNullOrEmpty(file))
                        {
                            BundleArchive archive = BundleArchive.Read(file);
                            if (archive != null)
                                descEntry1 = archive.GetEntryByID(id);
                        }
                    }

                    if (descEntry1 != null && descEntry1.Type == EntryType.RasterResourceType)
                    {
                        if (entry.Console)
                            result.Texture = GameImage.GetImagePS3(descEntry1.Header, descEntry1.Body);
                        else
                            result.Texture = GameImage.GetImagePC(descEntry1.Header, descEntry1.Body);

                        _cachedTextures.Add(id, result.Texture);

                        break;
                    }
                }
            }

            MemoryStream ms = entry.MakeStream();
            BinaryReader2 br = new BinaryReader2(ms);
            br.BigEndian = entry.Console;

            // TODO: Read Texture State

            br.Close();
            ms.Close();

            return result;
        }
    }
}
