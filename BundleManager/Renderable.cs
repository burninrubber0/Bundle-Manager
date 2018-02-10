using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BundleFormat;
using BundleUtilities;
using MathLib;
using ModelViewer.SceneData;
using OpenTK;

namespace BundleManager
{
    public struct VertexData
    {
        public Vector3 Vertex;
        public Vector3 Normal1;
        public Vector3 Normal2;
        public Vector2 UV1;
        public Vector2 UV2;
        public int ID1;
        public int ID2;
    }

    public class RenderableMesh
    {
        public Matrix4 RotationMatrix;
        public int Unknown19;
        public int Unknown20;
        public int IndexOffsetCount;
        public int NumVertices;
        public int VertexOffsetCount;
        public int NumFaces;
        public int MaterialIDInternal;
        public short Unknown21;
        public short Unknown22;
        public int NumIndicesOffset;
        public int VerticesOffsetPtr;
        public int[] VertexDescriptionsInternal;

        public ulong MaterialID;
        public ulong[] VertexDescriptionIDs;
        public VertexDesc[] VertexDescriptions;

        public int IndexOffset => IndexOffsetCount * 2;
        public int IndexCount => NumFaces * 3;

        public MaterialEntry Material;
        public List<uint> Indices;
        public List<VertexData> Vertices;
    }

    public class Renderable
    {
        public static bool LoadMaterials => FileView.LoadMaterials;

        public float Unknown1;
        public float Unknown2;
        public float Unknown3;
        public float Unknown4;
        public short Unknown5;
        public short MeshCount;
        public int StartOffset;
        public int Unknown8;
        public int Unknown9;
        public short Unknown10;
        public short Unknown10_1;
        public int Unknown11;
        public int Unknown12;
        public int Unknown13;
        public List<int> MeshVertexOffsets;
        public int NumIndices;
        //public short Unknown; // Console Only (PS3, X360?)
        public int Unknown15;
        public int Unknown16;
        public int Unknown17;
        public int VertexBlockAddress;
        public int Unknown18;
        public int VertexBlockSize;
        public List<RenderableMesh> Meshes;

        public List<short> Indices;

        public ulong ID;
        public Model Model;

        public Renderable()
        {
            MeshVertexOffsets = new List<int>();
            Meshes = new List<RenderableMesh>();
            Indices = new List<short>();
        }

        public void BuildModel()
        {
            Mesh[] meshes = new Mesh[Meshes.Count];

            for (int i = 0; i < meshes.Length; i++)
            {
                RenderableMesh rMesh = Meshes[i];
                Mesh mesh = new Mesh();

                if (rMesh.Material != null)
                {
                    Color color = rMesh.Material.Color;
                    Image diffuse = rMesh.Material.DiffuseMap;
                    Image normal = rMesh.Material.NormalMap;
                    Image specular = rMesh.Material.SpecularMap;

                    mesh.Material = new Material(rMesh.MaterialID.ToString("X8"), color, diffuse, normal, specular);
                }

                mesh.Indices = rMesh.Indices;

                for (int j = 0; j < rMesh.Vertices.Count; j++)
                {
                    VertexData data = rMesh.Vertices[j];
                    mesh.Vertices.Add(data.Vertex);
                    mesh.Normals.Add(data.Normal1);
                    mesh.UV1.Add(data.UV1);
                }

                meshes[i] = mesh;
            }
            
            Model = new Model(meshes);
        }

        public VertexData ReadVertex(BinaryReader br, int stride)
        {
            switch (stride)
            {
                case 0x0C:
                    return new VertexData()
                    {
                        Vertex = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle())
                    };
                case 0x20:
                    return new VertexData()
                    {
                        Vertex = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
                        Normal1 = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
                        UV1 = new Vector2(br.ReadSingle(), br.ReadSingle())
                    };
                case 0x28:
                    return new VertexData()
                    {
                        Vertex = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
                        Normal1 = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
                        UV1 = new Vector2(br.ReadSingle(), br.ReadSingle()),
                        UV2 = new Vector2(br.ReadSingle(), br.ReadSingle())
                    };
                case 0x2C:
                    return new VertexData()
                    {
                        Vertex = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
                        Normal1 = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
                        Normal2 = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
                        UV2 = new Vector2(br.ReadSingle(), br.ReadSingle())
                    };
                case 0x30:
                    return new VertexData()
                    {
                        Vertex = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
                        Normal1 = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
                        UV1 = new Vector2(br.ReadSingle(), br.ReadSingle()),
                        UV2 = new Vector2(br.ReadSingle(), br.ReadSingle()),
                        ID1 = br.ReadInt32(),
                        ID2 = br.ReadInt32()
                    };
                case 0x34:
                    return new VertexData()
                    {
                        Vertex = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
                        Normal1 = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
                        Normal2 = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
                        UV1 = new Vector2(br.ReadSingle(), br.ReadSingle()),
                        ID1 = br.ReadInt32(),
                        ID2 = br.ReadInt32()
                    };
                case 0x3C:
                    return new VertexData()
                    {
                        Vertex = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
                        Normal1 = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
                        Normal2 = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
                        UV1 = new Vector2(br.ReadSingle(), br.ReadSingle()),
                        UV2 = new Vector2(br.ReadSingle(), br.ReadSingle()),
                        ID1 = br.ReadInt32(),
                        ID2 = br.ReadInt32()
                    };
            }

            return new VertexData();
        }

        public void ReadBody(BinaryReader br)
        {
            for (int i = 0; i < NumIndices; i++)
            {
                Indices.Add(br.ReadInt16());
            }

            for (int i = 0; i < Meshes.Count; i++)
            {
                RenderableMesh mesh = Meshes[i];

                br.BaseStream.Position = mesh.IndexOffset;

                mesh.Indices = new List<uint>();
                for (int j = 0; j < mesh.IndexCount; j++)
                {
                    uint index = br.ReadUInt16() - (uint)mesh.VertexOffsetCount;

                    mesh.Indices.Add(index);
                }

                int stride = 0;
                foreach (VertexDesc desc in mesh.VertexDescriptions)
                {
                    if (desc == null)
                        continue;

                    stride = desc.Stride;
                }

                br.BaseStream.Position = VertexBlockAddress + mesh.VertexOffsetCount * stride;
                mesh.Vertices = new List<VertexData>();

                for (int j = 0; j < mesh.NumVertices; j++)
                {
                    mesh.Vertices.Add(ReadVertex(br, stride));
                }
            }
        }

        public static Renderable Read(BundleEntry entry, ILoader loader)
        {
            List<BundleDependency> dependencies = entry.GetDependencies();

            Renderable result = new Renderable();
            result.ID = entry.ID;

            MemoryStream ms = entry.MakeStream();
            BinaryReader2 br = new BinaryReader2(ms);
            br.BigEndian = entry.Console;

            result.Unknown1 = br.ReadSingle();
            result.Unknown2 = br.ReadSingle();
            result.Unknown3 = br.ReadSingle();
            result.Unknown4 = br.ReadSingle();
            result.Unknown5 = br.ReadInt16();
            result.MeshCount = br.ReadInt16();
            result.StartOffset = br.ReadInt32();
            result.Unknown8 = br.ReadInt32();
            result.Unknown9 = br.ReadInt32();
            result.Unknown10 = br.ReadInt16();
            result.Unknown10_1 = br.ReadInt16();
            result.Unknown11 = br.ReadInt32();
            result.Unknown12 = br.ReadInt32();
            result.Unknown13 = br.ReadInt32();

            br.BaseStream.Position = result.StartOffset;

            for (int i = 0; i < result.MeshCount; i++)
            {
                int offset = br.ReadInt32();
                result.MeshVertexOffsets.Add(offset);
            }

            /*if (entry.Platform == BundlePlatform.PS3)
            {
                br.BaseStream.Position += 16 - (br.BaseStream.Position % 16);
                result.NumIndices = br.ReadInt16();
                result.Unknown = br.ReadInt16();
            }
            else
            {*/
                result.NumIndices = br.ReadInt32();
            //}

            result.Unknown15 = br.ReadInt32();
            result.Unknown16 = br.ReadInt32();
            result.Unknown17 = br.ReadInt32();
            result.VertexBlockAddress = br.ReadInt32();
            result.Unknown18 = br.ReadInt32();
            result.VertexBlockSize = br.ReadInt32();

            // Padding
            br.BaseStream.Position += 16 - br.BaseStream.Position % 16;

            for (int i = 0; i < result.MeshCount; i++)
            {
                RenderableMesh mesh = new RenderableMesh();

                mesh.RotationMatrix = br.ReadMatrix4();
                mesh.Unknown19 = br.ReadInt32();
                mesh.Unknown20 = br.ReadInt32();
                mesh.IndexOffsetCount = br.ReadInt32();
                mesh.NumVertices = br.ReadInt32();
                mesh.VertexOffsetCount = br.ReadInt32();
                mesh.NumFaces = br.ReadInt32();
                int cPos = (int)br.BaseStream.Position;
                mesh.MaterialIDInternal = br.ReadInt32();
                foreach (BundleDependency dependency in dependencies)
                {
                    if (dependency.EntryPointerOffset == cPos)
                    {
                        mesh.MaterialID = dependency.EntryID;
                        break;
                    }
                }
                mesh.Unknown21 = br.ReadInt16();
                mesh.Unknown22 = br.ReadInt16();
                mesh.NumIndicesOffset = br.ReadInt32();
                mesh.VerticesOffsetPtr = br.ReadInt32();

                mesh.VertexDescriptionsInternal = new int[6];
                mesh.VertexDescriptionIDs = new ulong[6];
                for (int j = 0; j < mesh.VertexDescriptionsInternal.Length; j++)
                {
                    int pos = (int)br.BaseStream.Position;
                    foreach (BundleDependency dependency in dependencies)
                    {
                        if (dependency.EntryPointerOffset == pos)
                        {
                            mesh.VertexDescriptionIDs[j] = dependency.EntryID;
                            break;
                        }
                    }

                    mesh.VertexDescriptionsInternal[j] = br.ReadInt32();
                }

                result.Meshes.Add(mesh);
            }

            br.Close();
            ms.Close();
            
            for (int i = 0; i < result.Meshes.Count; i++)
            {
                RenderableMesh mesh = result.Meshes[i];

                if (LoadMaterials)
                {
                    BundleEntry descEntry1 = entry.Archive.GetEntryByID(mesh.MaterialID);
                    if (descEntry1 == null)
                    {
                        string file = BundleCache.GetFileByEntryID(mesh.MaterialID);
                        if (!string.IsNullOrEmpty(file))
                        {
                            BundleArchive archive = BundleArchive.Read(file);
                            descEntry1 = archive.GetEntryByID(mesh.MaterialID);
                        }
                    }

                    if (descEntry1 != null)
                        mesh.Material = MaterialEntry.Read(descEntry1);
                }

                mesh.VertexDescriptions = new VertexDesc[6];
                for (int j = 0; j < mesh.VertexDescriptions.Length; j++)
                {
                    ulong vertexDescID = mesh.VertexDescriptionIDs[j];
                    BundleEntry descEntry = entry.Archive.GetEntryByID(vertexDescID);
                    if (descEntry == null)
                    {
                        string file = BundleCache.GetFileByEntryID(vertexDescID);
                        if (!string.IsNullOrEmpty(file))
                        {
                            BundleArchive archive = BundleArchive.Read(file);
                            descEntry = archive.GetEntryByID(vertexDescID);
                        }
                    }

                    if (descEntry != null)
                        mesh.VertexDescriptions[j] = VertexDesc.Read(descEntry);
                }
            }

            ms = entry.MakeStream(true);
            br = new BinaryReader2(ms);
            br.BigEndian = entry.Console;
            result.ReadBody(br);
            br.Close();
            ms.Close();

            result.BuildModel();

            return result;
        }

        public Scene MakeScene()
        {
            Scene scene = new Scene();
            SceneObject obj = new SceneObject(ID.ToString("X8"), Model);
            scene.AddObject(obj);

            return scene;
        }
    }
}
