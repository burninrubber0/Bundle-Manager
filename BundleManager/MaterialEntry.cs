using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BundleFormat;

namespace BundleManager
{
    public class MaterialEntry
    {
        public List<TextureState> TextureStates;

        public Image DiffuseMap { get; set; }
        public Image NormalMap { get; set; }
        public Image SpecularMap { get; set; }

        public MaterialEntry()
        {
            TextureStates = new List<TextureState>();
        }

        public static MaterialEntry Read(BundleEntry entry)
        {
            MaterialEntry result = new MaterialEntry();
            
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

                if (descEntry1 != null && descEntry1.Type == EntryType.RwTextureStateResourceType)
                {
                    TextureState state = TextureState.Read(descEntry1);

                    result.TextureStates.Add(state);
                }
            }

            MemoryStream ms = entry.MakeStream();
            BinaryReader br = new BinaryReader(ms);

            // TODO: Read Material

            br.Close();
            ms.Close();

            if (result.TextureStates.Count > 0)
                result.DiffuseMap = result.TextureStates[0].Texture;
            if (result.TextureStates.Count > 1)
                result.NormalMap = result.TextureStates[1].Texture;
            if (result.TextureStates.Count > 2)
                result.SpecularMap = result.TextureStates[2].Texture;

            return result;
        }
    }
}
