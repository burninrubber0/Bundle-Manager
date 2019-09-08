using System.Collections.Generic;
using System.IO;
using BundleFormat;
using BundleUtilities;
using MathLib;
using ModelViewer.SceneData;
using OpenTK;

namespace BundleManager
{
    public class GraphicsSpec
    {
        public BundleEntry Entry;

        public int Unknown1;
        public List<ulong> Instances;
        public int Unknown2;
        public int Unknown3;

        public GraphicsSpec()
        {
            Instances = new List<ulong>();
        }

        public static GraphicsSpec Read(BundleEntry entry, ILoader loader)
        {
            GraphicsSpec result = new GraphicsSpec();
            result.Entry = entry;

            // TODO: Process Data

            for (int i = 0; i < entry.GetDependencies().Count; i++)
            {
                result.Instances.Add(entry.GetDependencies()[i].EntryID);
            }

            return result;
        }

        public Scene MakeScene()
        {
            Scene scene = new Scene();

            foreach (uint instance in Instances)
            {
                BundleEntry modelEntry = Entry.Archive.GetEntryByID(instance);
                if (modelEntry == null)
                {
                    string file = BundleCache.GetFileByEntryID(instance);
                    if (!string.IsNullOrEmpty(file))
                    {
                        BundleArchive archive = BundleArchive.Read(file);
                        modelEntry = archive.GetEntryByID(instance);
                    }
                }

                if (modelEntry != null)
                {
                    BundleEntry renderableEntry = modelEntry.GetDependencies()[0].Entry;
                    Renderable renderable = Renderable.Read(renderableEntry, null); // TODO: Null Loader
                    SceneObject sceneObject = new SceneObject(instance.ToString("X8"), renderable.Model);
                    //sceneObject.Transform = instance.Transform;

                    scene.AddObject(sceneObject);
                }
            }

            return scene;
        }
    }
}
