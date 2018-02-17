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
    public class ModelInstance
    {
        public ulong ModelEntryID;

        public int ModelEntryPtr;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
        public Matrix4 Transform;

        public Vector3 Translation => Transform.ExtractTranslation();

        public Vector3 Scale => Transform.ExtractScale();

        public Vector4 Rotation => Transform.ExtractRotation().ToAxisAngle();

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

        public byte[] RemainingBytes;

        public InstanceList()
        {
            Instances = new List<ModelInstance>();
        }

        public static InstanceList Read(BundleEntry entry, ILoader loader)
        {
            InstanceList result = new InstanceList();
            result.Entry = entry;

            MemoryStream ms = entry.MakeStream();
            BinaryReader2 br = new BinaryReader2(ms);
            br.BigEndian = entry.Console;

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

            result.RemainingBytes = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position));

            br.Close();
            ms.Close();

            /*foreach (uint key in MultiEntries.Keys)
            {
                int timesUsed = MultiEntries[key];

                Debug.WriteLine("EntryID: 0x" + key.ToString("X8") + " was used " + timesUsed + " times");
            }*/

            /*{
                ModelInstance inst = result.Instances[318];
                Vector4 originalCol = inst.Transform.Column3;
                Matrix4 t = Matrix4.CreateTranslation(inst.Transform.ExtractTranslation() + new Vector3(0, -999, 0));
                Matrix4 r = Matrix4.CreateFromQuaternion(inst.Transform.ExtractRotation());
                Matrix4 s = Matrix4.CreateScale(inst.Transform.ExtractScale());
                inst.Transform = r * s * t;
                inst.Transform.Column3 = originalCol;
            }

            {
                ModelInstance inst = result.Instances[316];
                Vector4 originalCol = inst.Transform.Column3;
                Matrix4 t = Matrix4.CreateTranslation(inst.Transform.ExtractTranslation() + new Vector3(0, -999, 0));
                Matrix4 r = Matrix4.CreateFromQuaternion(inst.Transform.ExtractRotation());
                Matrix4 s = Matrix4.CreateScale(inst.Transform.ExtractScale());
                inst.Transform = r * s * t;
                inst.Transform.Column3 = originalCol;
            }

            {
                ModelInstance inst = result.Instances[148];
                Vector4 originalCol = inst.Transform.Column3;
                Matrix4 t = Matrix4.CreateTranslation(inst.Transform.ExtractTranslation() + new Vector3(0, -999, 0));
                Matrix4 r = Matrix4.CreateFromQuaternion(inst.Transform.ExtractRotation());
                Matrix4 s = Matrix4.CreateScale(inst.Transform.ExtractScale());
                inst.Transform = r * s * t;
                inst.Transform.Column3 = originalCol;
            }

            {
                ModelInstance inst = result.Instances[147];
                Vector4 originalCol = inst.Transform.Column3;
                Matrix4 t = Matrix4.CreateTranslation(inst.Transform.ExtractTranslation() + new Vector3(0, -999, 0));
                Matrix4 r = Matrix4.CreateFromQuaternion(inst.Transform.ExtractRotation());
                Matrix4 s = Matrix4.CreateScale(inst.Transform.ExtractScale());
                inst.Transform = r * s * t;
                inst.Transform.Column3 = originalCol;
            }

            {
                ModelInstance inst = result.Instances[146];
                Vector4 originalCol = inst.Transform.Column3;
                Matrix4 t = Matrix4.CreateTranslation(inst.Transform.ExtractTranslation() + new Vector3(0, -999, 0));
                Matrix4 r = Matrix4.CreateFromQuaternion(inst.Transform.ExtractRotation());
                Matrix4 s = Matrix4.CreateScale(inst.Transform.ExtractScale());
                inst.Transform = r * s * t;
                inst.Transform.Column3 = originalCol;
            }

            {
                ModelInstance inst = result.Instances[145];
                Vector4 originalCol = inst.Transform.Column3;
                Matrix4 t = Matrix4.CreateTranslation(inst.Transform.ExtractTranslation() + new Vector3(0, -999, 0));
                Matrix4 r = Matrix4.CreateFromQuaternion(inst.Transform.ExtractRotation());
                Matrix4 s = Matrix4.CreateScale(inst.Transform.ExtractScale());
                inst.Transform = r * s * t;
                inst.Transform.Column3 = originalCol;
            }

            {
                ModelInstance inst = result.Instances[144];
                Vector4 originalCol = inst.Transform.Column3;
                Matrix4 t = Matrix4.CreateTranslation(inst.Transform.ExtractTranslation() + new Vector3(0, -999, 0));
                Matrix4 r = Matrix4.CreateFromQuaternion(inst.Transform.ExtractRotation());
                Matrix4 s = Matrix4.CreateScale(inst.Transform.ExtractScale());
                inst.Transform = r * s * t;
                inst.Transform.Column3 = originalCol;
            }

            result.Write(entry);*/

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

            bw.Write(RemainingBytes);

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

            Dictionary<ulong, Renderable> models = new Dictionary<ulong, Renderable>();

            int i = 0;
            int index = 1;
            foreach (ModelInstance instance in Instances)
            {
                int progress = (index - 1) * 100 / Instances.Count;
                loader?.SetProgress(progress);

                loader?.SetStatus("Loading(" + progress.ToString("D2") + "%): ModelInstance: " + index + "/" + Instances.Count);
                //DebugTimer t = DebugTimer.Start("ModelInstance[" + index + "/" + Instances.Count + "]");
                index++;

                if (models.ContainsKey(instance.ModelEntryID))
                {
                    Renderable renderable = models[instance.ModelEntryID];
                    SceneObject sceneObject = new SceneObject(instance.ModelEntryID.ToString("X8"), renderable.Model);
                    sceneObject.ID = i.ToString();
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
                            BundleArchive archive = BundleArchive.Read(file);
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
                        sceneObject.ID = i.ToString();
                        sceneObject.Transform = instance.Transform;

                        scene.AddObject(sceneObject);
                    }
                }
                i++;
                //t.StopLog();
            }
            loader?.SetProgress(100);

            //TextureState.ResetCache();

            return scene;
        }
    }
}
