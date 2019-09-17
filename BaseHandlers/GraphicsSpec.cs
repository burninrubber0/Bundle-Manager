using System.Collections.Generic;
using System.IO;
using BundleFormat;
using BundleUtilities;
using MathLib;
using ModelViewer;
using ModelViewer.SceneData;
using OpenTK;
using PluginAPI;

namespace BaseHandlers
{
    public class GraphicsSpec : IEntryData
	{
		private Scene _scene;

		public BundleEntry Entry;

        public int Unknown1;
        public List<ulong> Instances;
        public int Unknown2;
        public int Unknown3;

        public GraphicsSpec()
        {
            Instances = new List<ulong>();
        }

		private void Clear()
		{
			_scene = null;

			Entry = default;

			Unknown1 = default;
			Unknown2 = default;
			Unknown3 = default;

			Instances.Clear();
		}

        public bool Read(BundleEntry entry, ILoader loader)
        {
			Clear();

            Entry = entry;

            // TODO: Process Data

            for (int i = 0; i < entry.GetDependencies().Count; i++)
            {
                Instances.Add(entry.GetDependencies()[i].EntryID);
			}

			_scene = MakeScene(loader);

			return true;
		}

		public bool Write(BundleEntry entry)
		{
			return true;
		}

		public EntryType GetEntryType(BundleEntry entry)
		{
			return EntryType.GraphicsSpecResourceType;
		}

		public IEntryEditor GetEditor(BundleEntry entry)
		{
			ModelViewerForm viewer = new ModelViewerForm();
			viewer.Renderer.Scene = _scene;

			return viewer;
		}

		public Scene MakeScene(ILoader loader)
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
					Renderable renderable = new Renderable();
					renderable.Read(renderableEntry, null); // TODO: Null Loader
                    SceneObject sceneObject = new SceneObject(instance.ToString("X8"), renderable.Model);
                    //sceneObject.Transform = instance.Transform;

                    scene.AddObject(sceneObject);
                }
            }

            return scene;
        }
	}
}
