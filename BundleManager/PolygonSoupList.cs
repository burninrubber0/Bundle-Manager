using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BundleFormat;
using BundleUtilities;
using DebugHelper;
using MathLib;
using ModelViewer.SceneData;
using OpenTK;
using StandardExtension;

namespace BundleManager
{
    public class PolygonSoupProperty
    {
        public ushort UnknownProperty1;
        public ushort UnknownProperty2;
        public byte[] Indices;
        public uint Unknown1;

        public PolygonSoupProperty()
        {
            Indices = new byte[4];
        }

        public static PolygonSoupProperty Read(BinaryReader br)
        {
            PolygonSoupProperty result = new PolygonSoupProperty();

            result.UnknownProperty1 = br.ReadUInt16();
            result.UnknownProperty2 = br.ReadUInt16();

            for (int i = 0; i < result.Indices.Length; i++)
            {
                result.Indices[i] = br.ReadByte();
            }

            result.Unknown1 = br.ReadUInt32();

            return result;
        }

        public override string ToString()
        {
            return "Prop1: " + UnknownProperty1.ToString("X2") + ", Prop2: " + UnknownProperty2.ToString("X2");
        }
    }

    public class PolygonSoupBoundingBox
    {
        public BoxF Box;
        public int Unknown;

        public override string ToString()
        {
            return Box.ToString() + ", " + Unknown;
        }
    }

    public class PolygonSoupChunk
    {
        public Vector3I Position;
        public float Scale;
        public uint PropertyListStart;
        public uint PointListStart;
        public short Unknown7;
        public byte Unknown8;
        public byte Unknown9;
        public int PointCount;

        public List<Vector3S> PointList;
        public List<PolygonSoupProperty> PropertyList;

        public PolygonSoupChunk()
        {
            PointList = new List<Vector3S>();
            PropertyList = new List<PolygonSoupProperty>();
        }

        public static PolygonSoupChunk Read(BinaryReader br)
        {
            PolygonSoupChunk result = new PolygonSoupChunk();

            result.Position = br.ReadVector3I();
            result.Scale = br.ReadSingle();
            result.PropertyListStart = br.ReadUInt32();
            result.PointListStart = br.ReadUInt32();
            result.Unknown7 = br.ReadInt16();
            result.Unknown8 = br.ReadByte();
            result.Unknown9 = br.ReadByte();
            result.PointCount = br.ReadInt32();

            br.BaseStream.Position = result.PointListStart;
            for (int i = 0; i < result.PointCount; i++)
            {
                result.PointList.Add(br.ReadVector3S());
            }

            br.BaseStream.Position = result.PropertyListStart;

            int count = (result.Unknown9 >> 1) * 2 +
                        (result.Unknown9 - (result.Unknown9 >> 1) * 2) +
                        ((result.Unknown8 - result.Unknown9) >> 2) +
                        ((result.Unknown8 - result.Unknown9) - ((result.Unknown8 - result.Unknown9) >> 2) * 4);

            for (int i = 0; i < count; i++)
            {
                result.PropertyList.Add(PolygonSoupProperty.Read(br));
            }

            return result;
        }

        public void Write(BinaryWriter bw)
        {
            // TODO: Write PolygonSoupChunk
        }

        public Mesh BuildMesh(Vector3I pos, float scale)
        {
            Mesh mesh = new Mesh();

            // TODO: Build Mesh
            
            for (int i = 0; i < PropertyList.Count; i++)
            {
                PolygonSoupProperty property = PropertyList[i];
                mesh.Indices.Add(property.Indices[0]);
                mesh.Indices.Add(property.Indices[1]);
                mesh.Indices.Add(property.Indices[2]);
                if (property.Indices[3] != 0xFF)
                {
                    mesh.Indices.Add(property.Indices[3]);
                    mesh.Indices.Add(property.Indices[2]);
                    mesh.Indices.Add(property.Indices[1]);
                }
                
                //DebugUtil.ShowDebug(mesh);
            }

            List<Vector3S> points = PointList;
            //points.Reverse();

            for (int i = 0; i < points.Count; i++)
            {
                Vector3S vert = points[i];
                //mesh.Vertices.Add(new Vector3(vert.X * scale + pos.X, vert.Y * scale + pos.Y, vert.Z * scale + pos.Z));
                mesh.Vertices.Add(new Vector3((vert.X + pos.X) * scale, (vert.Y + pos.Y) * scale, (vert.Z + pos.Z) * scale));
                //mesh.Vertices.Add(new Vector3((vert.X) * scale, (vert.Y) * scale, (vert.Z) * scale));
                //mesh.Vertices.Add(new Vector3((vert.X) * scale, (vert.Z) * scale, (vert.Y) * scale));
                //mesh.Vertices.Add(new Vector3((vert.X + pos.X) * scale, (vert.Z + pos.Z) * scale, (vert.Y + pos.Y) * scale));
            }

            return mesh;
        }

        public override string ToString()
        {
            return "Pos: " + Position + ", Scale: " + Scale + ", Unk7: " + Unknown7 + ", PointCount: " + PointCount;
        }
    }

    public class PolygonSoupList
    {
        public Vector3 Min;
        public int Unknown4;
        public Vector3 Max;
        public int Unknown8;
        public uint ChunkPointerStart;
        public uint BoxListStart;
        public int ChunkCount;
        public uint FileSize;

        public List<uint> ChunkPointers;
        public List<PolygonSoupBoundingBox> BoundingBoxes;
        public List<PolygonSoupChunk> Chunks;

        public PolygonSoupList()
        {
            ChunkPointers = new List<uint>();
            BoundingBoxes = new List<PolygonSoupBoundingBox>();
            Chunks = new List<PolygonSoupChunk>();
        }

        public static PolygonSoupList Read(BundleEntry entry)
        {
            PolygonSoupList result = new PolygonSoupList();

            MemoryStream ms = entry.MakeStream();
            BinaryReader2 br = new BinaryReader2(ms);
            br.BigEndian = entry.Console;

            result.Min = br.ReadVector3F();
            result.Unknown4 = br.ReadInt32();
            result.Max = br.ReadVector3F();
            result.Unknown8 = br.ReadInt32();
            result.ChunkPointerStart = br.ReadUInt32();
            result.BoxListStart = br.ReadUInt32();
            result.ChunkCount = br.ReadInt32();
            result.FileSize = br.ReadUInt32();

            // No Data
            if (result.ChunkCount == 0)
            {
                br.Close();
                ms.Close();
                return result;
            }

            br.BaseStream.Position = result.ChunkPointerStart;

            for (int i = 0; i < result.ChunkCount; i++)
            {
                result.ChunkPointers.Add(br.ReadUInt32());
            }

            //br.BaseStream.Position += (16 - br.BaseStream.Position % 16);
            //br.BaseStream.Position = result.BoxListStart;

            for (int i = 0; i < result.ChunkCount; i++)
            {
                // Read Vertically

                long pos = result.BoxListStart + 0x70 * (i / 4) + 4 * (i % 4);

                PolygonSoupBoundingBox box = new PolygonSoupBoundingBox();

                BoxF boundingBox = new BoxF();
                br.BaseStream.Position = pos;
                float minX = br.ReadSingle();
                br.BaseStream.Position += 12;
                float minY = br.ReadSingle();
                br.BaseStream.Position += 12;
                float minZ = br.ReadSingle();

                boundingBox.Min = new Vector3(minX, minY, minZ);

                br.BaseStream.Position += 12;
                float maxX = br.ReadSingle();
                br.BaseStream.Position += 12;
                float maxY = br.ReadSingle();
                br.BaseStream.Position += 12;
                float maxZ = br.ReadSingle();

                boundingBox.Max = new Vector3(maxX, maxY, maxZ);

                box.Box = boundingBox;

                br.BaseStream.Position += 12;
                box.Unknown = br.ReadInt32();

                result.BoundingBoxes.Add(box);
            }

            for (int i = 0; i < result.ChunkPointers.Count; i++)
            {
                br.BaseStream.Position = result.ChunkPointers[i];

                result.Chunks.Add(PolygonSoupChunk.Read(br));
            }

            br.Close();
            ms.Close();

            return result;
        }

        public void Write(BundleEntry entry)
        {
            // TODO: Write PolygonSoupList
        }

        public Scene MakeScene(ILoader loader = null)
        {
            Scene scene = new Scene();

            int index = 0;
            foreach (PolygonSoupChunk chunk in Chunks)
            {
                string id = index.ToString();

                Vector3I pos = chunk.Position;
                float scale = chunk.Scale;
                Model model = new Model(chunk.BuildMesh(pos, scale));
                SceneObject sceneObject = new SceneObject(id, model);
                sceneObject.ID = id;
                //sceneObject.Transform = Matrix4.CreateScale(scale) *
                //                        Matrix4.CreateTranslation(new Vector3(pos.X, pos.Y, pos.Z));
                scene.AddObject(sceneObject);
                index++;

                // TODO: TEMP
                //break;
            }

            return scene;
        }
    }
}
