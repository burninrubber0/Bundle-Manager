using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using BundleFormat;
using BundleUtilities;
using MathLib;
using ModelViewer.SceneData;
using OpenTK;

namespace BundleManager
{
    public struct ModelInstance
    {
        public uint ModelEntryID;

        public int ModelEntryPtr;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
        public Matrix4 Transform;

        public Vector3 Translation => Transform.ExtractTranslation();

        public Vector3 Scale => Transform.ExtractScale();

        public Vector4 Rotation => Transform.ExtractRotation().ToAxisAngle();
        /*public Vector3D Scale
        {
            get
            {
                float x = new Vector3D(Transform.M11, Transform.M21, Transform.M31).Length();
                float y = new Vector3D(Transform.M12, Transform.M22, Transform.M32).Length();
                float z = new Vector3D(Transform.M13, Transform.M23, Transform.M33).Length();
                return new Vector3D(x, y, z);
            }
        }

        public AxisAngleRotation3D Rotation => new AxisAngleRotation3D(Quaternion.CreateFromRotationMatrix(Transform));*/

        public static ModelInstance Read(BinaryReader br)
        {
            ModelInstance result = new ModelInstance();

            result.ModelEntryPtr = br.ReadInt32();
            result.Unknown2 = br.ReadInt32();
            result.Unknown3 = br.ReadInt32();
            result.Unknown4 = br.ReadInt32();
            result.Transform = br.ReadMatrix4();

            return result;
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(ModelEntryPtr);
            bw.Write(Unknown2);
            bw.Write(Unknown3);
            bw.Write(Unknown4);
            bw.Write(Transform);
        }

        public override string ToString()
        {
            return "Model ID: 0x" + ModelEntryID.ToString("X8") + ", Translation: (" + Translation + "), Scale: (" + Scale + "), Rotation: (" + Rotation + ")";
        }
    }

    public class InstanceList
    {
        public BundleEntry Entry;

        public int Unknown1;
        public List<ModelInstance> Instances;
        public int Unknown2;
        public int Unknown3;

        public InstanceList()
        {
            Instances = new List<ModelInstance>();
        }

        public static InstanceList Read(BundleEntry entry, ILoader loader)
        {
            InstanceList result = new InstanceList();
            result.Entry = entry;

            MemoryStream ms = entry.MakeStream();
            BinaryReader br = new BinaryReader(ms);

            result.Unknown1 = br.ReadInt32();
            int instanceCount = br.ReadInt32();
            result.Unknown2 = br.ReadInt32();
            result.Unknown3 = br.ReadInt32();

            //List<uint> UsedEntries = new List<uint>();
            //Dictionary<uint, int> MultiEntries = new Dictionary<uint, int>();

            for (int i = 0; i < instanceCount; i++)
            {
                ModelInstance instance = ModelInstance.Read(br);

                instance.ModelEntryID = entry.GetDependencies()[i].EntryID;

                /*if (UsedEntries.Contains(instance.ModelEntryID))
                {
                    if (MultiEntries.ContainsKey(instance.ModelEntryID))
                    {
                        MultiEntries[instance.ModelEntryID]++;
                    }
                    else
                    {
                        MultiEntries.Add(instance.ModelEntryID, 1);
                    }
                }
                else
                {
                    UsedEntries.Add(instance.ModelEntryID);
                }*/

                result.Instances.Add(instance);
            }

            br.Close();
            ms.Close();

            /*foreach (uint key in MultiEntries.Keys)
            {
                int timesUsed = MultiEntries[key];

                Debug.WriteLine("EntryID: 0x" + key.ToString("X8") + " was used " + timesUsed + " times");
            }*/

            return result;
        }

        public void Write(BundleEntry entry)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(Unknown1);
            bw.Write(Instances.Count);
            bw.Write(Unknown2);
            bw.Write(Unknown3);

            for (int i = 0; i < Instances.Count; i++)
            {
                ModelInstance instance = Instances[i];

                instance.Write(bw);
            }

            bw.Flush();
            byte[] data = ms.GetBuffer();
            bw.Close();
            ms.Close();

            entry.Header = data;
            entry.Dirty = true;
        }

        public Scene MakeScene(ILoader loader = null)
        {
            //TextureState.ResetCache();

            Scene scene = new Scene();

            Dictionary<uint, Renderable> models = new Dictionary<uint, Renderable>();

            int index = 1;
            foreach (ModelInstance instance in Instances)
            {
                int progress = (index - 1) * 100 / Instances.Count;
                loader?.SetProgress(progress);

                loader?.SetStatus("Loading(" + progress.ToString("D2") + "%): ModelInstance: " + index + "/" + Instances.Count);
                DebugTimer t = DebugTimer.Start("ModelInstance[" + index + "/" + Instances.Count + "]");
                index++;

                if (models.ContainsKey(instance.ModelEntryID))
                {
                    Renderable renderable = models[instance.ModelEntryID];
                    SceneObject sceneObject = new SceneObject(instance.ModelEntryID.ToString("X8"), renderable.Model);
                    sceneObject.Transform = instance.Transform;

                    scene.AddObject(sceneObject);
                }
                else
                {
                    BundleEntry modelEntry = Entry.Archive.GetEntryByID(instance.ModelEntryID);
                    if (modelEntry == null)
                    {
                        string file = BundleCache.GetFileByEntryID(instance.ModelEntryID);
                        if (!string.IsNullOrEmpty(file))
                        {
                            BundleArchive archive = BundleArchive.Read(file, Entry.Console);
                            modelEntry = archive.GetEntryByID(instance.ModelEntryID);
                        }
                    }

                    if (modelEntry != null)
                    {
                        BundleEntry renderableEntry = modelEntry.GetDependencies()[0].Entry;
                        Renderable renderable = Renderable.Read(renderableEntry, null); // TODO: Null Loader
                        models.Add(instance.ModelEntryID, renderable);
                        SceneObject sceneObject =
                            new SceneObject(instance.ModelEntryID.ToString("X8"), renderable.Model);
                        sceneObject.Transform = instance.Transform;

                        scene.AddObject(sceneObject);
                    }
                }
                t.StopLog();
            }
            loader?.SetProgress(100);

            //TextureState.ResetCache();

            return scene;
        }
    }
}
