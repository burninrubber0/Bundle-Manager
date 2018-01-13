using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BundleFormat;
using BurnoutImage;

namespace BundleManager
{
    public class TextureState
    {
        public Image Texture;

        public TextureState()
        {
            
        }

        public static TextureState Read(BundleEntry entry)
        {
            TextureState result = new TextureState();

            List<Dependency> dependencies = entry.GetDependencies();
            foreach (Dependency dependency in dependencies)
            {
                uint id = dependency.EntryID;

                BundleEntry descEntry1 = entry.Archive.GetEntryByID(id);
                if (descEntry1 == null)
                {
                    string file = BundleCache.GetFileByEntryID(id);
                    if (!string.IsNullOrEmpty(file))
                    {
                        BundleArchive archive = BundleArchive.Read(file, entry.Console);
                        descEntry1 = archive.GetEntryByID(id);
                    }
                }

                if (descEntry1 != null && descEntry1.Type == EntryType.RasterResourceType)
                {
                    if (entry.Console)
                        result.Texture = GameImage.GetImagePS3(descEntry1.Header, descEntry1.Body);
                    else
                        result.Texture = GameImage.GetImagePC(descEntry1.Header, descEntry1.Body);

                    break;
                }
            }

            MemoryStream ms = entry.MakeStream();
            BinaryReader br = new BinaryReader(ms);

            // TODO: Read Texture State

            br.Close();
            ms.Close();

            return result;
        }
    }
}
