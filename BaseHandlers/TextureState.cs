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

namespace BaseHandlers
{
    public class TextureState
    {
        public Texture Texture;

        public TextureState()
        {
            
        }

        public static TextureState Read(BundleEntry entry)
        {
            TextureState result = new TextureState();

            List<BundleDependency> dependencies = entry.GetDependencies();
            foreach (BundleDependency dependency in dependencies)
            {
                ulong id = dependency.EntryID;

                if (TextureCache.Contains(id))
                {
                    result.Texture = TextureCache.GetTexture(id);
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
                            result.Texture = GameImage.GetImagePS3(descEntry1.EntryBlocks[0].Data, descEntry1.EntryBlocks[1].Data);
						else
                            result.Texture = GameImage.GetImage(descEntry1.EntryBlocks[0].Data, descEntry1.EntryBlocks[1].Data);

						if (result.Texture != null)
							TextureCache.AddToCache(id, result.Texture);

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
