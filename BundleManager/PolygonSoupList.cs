using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
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
        public uint UnknownProperty;
        public byte[] Indices;
		public byte[] UnknownBytes;

		public PolygonSoupProperty()
        {
            Indices = new byte[4];
			UnknownBytes = new byte[4];
        }

        public PolygonSoupProperty Copy()
        {
            PolygonSoupProperty result = new PolygonSoupProperty();

            result.UnknownProperty = UnknownProperty;
            for (int i = 0; i < Indices.Length; i++)
            {
                result.Indices[i] = Indices[i];
            }
            for (int i = 0; i < UnknownBytes.Length; i++)
            {
                result.UnknownBytes[i] = UnknownBytes[i];
            }

            return result;
        }

        public static PolygonSoupProperty Read(BinaryReader br)
        {
            PolygonSoupProperty result = new PolygonSoupProperty();

            result.UnknownProperty = br.ReadUInt32();

            for (int i = 0; i < result.Indices.Length; i++)
            {
                result.Indices[i] = br.ReadByte();
			}

			for (int i = 0; i < result.UnknownBytes.Length; i++)
			{
				result.UnknownBytes[i] = br.ReadByte();
			}

            return result;
        }

        public void Write(BinaryWriter bw)
        {
            // Cap to 0x9D64 for PC
            ushort unknownProperty1 = (ushort)(UnknownProperty & 0xFFFF);
            /*byte unk1 = (byte) (unknownProperty1 & 0xFF);
            byte unk2 = (byte) (unknownProperty1 >> 8);

            if (unk2 > 0x9D && unk2 != 0xFF)
                unk2 = 0x9D;

            if (unk1 > 0x64 && unk1 != 0xFF)
                unk1 = 0x64;*/

            //unknownProperty1 = (ushort)((unk2 << 8) | unk1);

            // Patch values that are too high (from console) with Schembri Pass
            if (unknownProperty1 > 0x9D64 && unknownProperty1 != 0xFFFF)
            {
                // 0x8316, 0x9531,  0x9007, 0x8511, 0x8316, 0xFFFF
                unknownProperty1 = 0x8316;
            }

            // Patch values that are too high(from console) with 0xFFFF
            //if (unknownProperty1 > 0x9D64)
            //    unknownProperty1 = 0xFFFF;

            ushort unknownProperty2 = (ushort) ((UnknownProperty >> 16) & 0xFFFF);

            // Remove wreck surfaces - TODO: tmp
            //unknownProperty2 &= unchecked((ushort)~0x4000);

            uint unknownProperty = (uint)((unknownProperty2 << 16) | unknownProperty1);

            bw.Write(unknownProperty);

            for (int i = 0; i < Indices.Length; i++)
            {
                bw.Write(Indices[i]);
            }

            for (int i = 0; i < UnknownBytes.Length; i++)
            {
                bw.Write(UnknownBytes[i]);
            }
        }

        public override string ToString()
        {
            return "Prop: " + UnknownProperty.ToString("X8");
        }
    }

    public class PolygonSoupBoundingBox
    {
        public BoxF Box;
        public int Unknown;

		public PolygonSoupBoundingBox(BoxF box, int unknown)
		{
			Box = box;
			Unknown = unknown;
		}

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
        public byte PropertyListCount;
        public byte QuadCount;
        public byte PointCount;
        public byte Unknown10;
        public short Unknown11;

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
            br.ReadInt16(); // Length
            result.PropertyListCount = br.ReadByte();
            result.QuadCount = br.ReadByte();
            result.PointCount = br.ReadByte();
            result.Unknown10 = br.ReadByte();
            result.Unknown11 = br.ReadInt16();

            br.BaseStream.Position = result.PointListStart;
            for (int i = 0; i < result.PointCount; i++)
            {
                result.PointList.Add(br.ReadVector3S());
            }

            br.BaseStream.Position = result.PropertyListStart;

            for (int i = 0; i < result.PropertyListCount; i++)
            {
                result.PropertyList.Add(PolygonSoupProperty.Read(br));
            }

            return result;
        }

        public void Write(BinaryWriter bw)
		{
			long chunkStartPtr = bw.BaseStream.Position;
			bw.Write(Position);
            bw.Write(Scale);
            long propListStartPtr = bw.BaseStream.Position;
            bw.Write((uint) 0);
            long pointListStartPtr = bw.BaseStream.Position;
            bw.Write((uint) 0);
			long lengthPtr = bw.BaseStream.Position;
            bw.Write((ushort) 0);
            bw.Write(PropertyListCount);
            bw.Write(QuadCount);
            bw.Write(PointCount);
            bw.Write(Unknown10);
            bw.Write(Unknown11);
            
            long cPos = bw.BaseStream.Position;
            bw.BaseStream.Position = pointListStartPtr;
            bw.Write((uint)cPos);
            bw.BaseStream.Position = cPos;

            for (int i = 0; i < PointCount; i++)
            {
                bw.Write(PointList[i]);
            }

            bw.BaseStream.Position = (16 * ((bw.BaseStream.Position + 15) / 16));
            cPos = bw.BaseStream.Position;
            bw.BaseStream.Position = propListStartPtr;
            bw.Write((uint)cPos);
            bw.BaseStream.Position = cPos;

            for (int i = 0; i < PropertyListCount; i++)
            {
                PropertyList[i].Write(bw);
            }
			cPos = bw.BaseStream.Position;
			bw.BaseStream.Position = lengthPtr;
			bw.Write((short)(bw.BaseStream.Length - chunkStartPtr));
			bw.BaseStream.Position = cPos;

		}

        public Mesh BuildMesh(Vector3I pos, float scale)
        {
            Mesh mesh = new Mesh();
            mesh.Materials = new Dictionary<uint, Material>();

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

                ushort unknownProperty1 = (ushort)(property.UnknownProperty & 0xFFFF);
                ushort unknownProperty2 = (ushort)((property.UnknownProperty >> 16));// & 0xFFFF);

                //ushort banana = (ushort)(unknownProperty1 & 0xFF);
                //ushort id = (ushort) (unknownProperty1 & 0x7FFF);

                //if (unknownProperty1 != 0xFFFF)
                // Upper = Math.Max(unknownProperty1, Upper);

                // To Reverse
                //uint unknownProperty = (uint)((unknownProperty2 << 16) | unknownProperty1);

                /*if (unknownProperty1 == 0x9DA2)
                {
                    Material mat = new Material(unknownProperty1.ToString("X4"), Color.Orange);
                    mesh.Materials[property.Indices[0]] = mat;
                    mesh.Materials[property.Indices[1]] = mat;
                    mesh.Materials[property.Indices[2]] = mat;
                    if (property.Indices[3] != 0xFF)
                        mesh.Materials[property.Indices[3]] = mat;
                } else if (unknownProperty1 > 0x9D64 && unknownProperty1 != 0xFFFF)
                {
                    Material mat = new Material(unknownProperty1.ToString("X4"), Color.Red);
                    mesh.Materials[property.Indices[0]] = mat;
                    mesh.Materials[property.Indices[1]] = mat;
                    mesh.Materials[property.Indices[2]] = mat;
                    if (property.Indices[3] != 0xFF)
                        mesh.Materials[property.Indices[3]] = mat;
                }
                else
                {
                    Material mat = new Material(unknownProperty1.ToString("X4"), Color.White);
                    mesh.Materials[property.Indices[0]] = mat;
                    mesh.Materials[property.Indices[1]] = mat;
                    mesh.Materials[property.Indices[2]] = mat;
                    if (property.Indices[3] != 0xFF)
                        mesh.Materials[property.Indices[3]] = mat;
                }*/

                /*int red = banana * 10 % 255;
                int green = banana * 5 % 255;
                int blue = banana * 12 % 255;

                Color color = Color.FromArgb(red, green, blue);
                Material mat = new Material(banana.ToString("X2"), color);
                    //Color.FromArgb(banana & 0xFF, 0, (banana >> 8) & 0xFF));
                mesh.Materials[property.Indices[0]] = mat;
                mesh.Materials[property.Indices[1]] = mat;
                mesh.Materials[property.Indices[2]] = mat;
                if (property.Indices[3] != 0xFF)
                    mesh.Materials[property.Indices[3]] = mat;*/

                //if (unknownProperty1 > 0x9D64 && unknownProperty1 != 0xFFFF)
                /*if (unknownProperty1 > 0x9D64 && unknownProperty1 != 0xFFFF)
                {
                    string bla = property.UnknownBytes[0].ToString("X2") + "_" +
                                 property.UnknownBytes[1].ToString("X2") + "_" +
                                 property.UnknownBytes[2].ToString("X2") + "_" +
                                 property.UnknownBytes[3].ToString("X2");
                    //Material mat = new Material(unknownProperty1.ToString("X4"),
                    Material mat = new Material(unknownProperty1.ToString("X4") + "_" + unknownProperty2.ToString("X4") + "_" + bla,
                        Color.FromArgb(unknownProperty1 & 0xFF, 0, (unknownProperty1 >> 8) & 0xFF));
                    mesh.Materials[property.Indices[0]] = mat;
                    mesh.Materials[property.Indices[1]] = mat;
                    mesh.Materials[property.Indices[2]] = mat;
                    if (property.Indices[3] != 0xFF)
                        mesh.Materials[property.Indices[3]] = mat;
                }
                else
                {*/
                    string bla = property.UnknownBytes[0].ToString("X2") + "_" +
                                 property.UnknownBytes[1].ToString("X2") + "_" +
                                 property.UnknownBytes[2].ToString("X2") + "_" +
                                 property.UnknownBytes[3].ToString("X2");
                    //Material mat = new Material(unknownProperty1.ToString("X4"),
                    Material mat = new Material(unknownProperty1.ToString("X4") + "_" + unknownProperty2.ToString("X4") + "_" + bla, Color.White);
                    mesh.Materials[property.Indices[0]] = mat;
                    mesh.Materials[property.Indices[1]] = mat;
                    mesh.Materials[property.Indices[2]] = mat;
                    if (property.Indices[3] != 0xFF)
                        mesh.Materials[property.Indices[3]] = mat;
                //}
            }
			

			List<Vector3S> points = PointList;

            for (int i = 0; i < points.Count; i++)
            {
                Vector3S vert = points[i];
                mesh.Vertices.Add(new Vector3((vert.X + pos.X) * scale, (vert.Y + pos.Y) * scale, (vert.Z + pos.Z) * scale));
            }

            return mesh;
        }

        public override string ToString()
        {
            return "Pos: " + Position + ", Scale: " + Scale + ", PointCount: " + PointCount;
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

        //public List<uint> ChunkPointers;
        public List<PolygonSoupBoundingBox> BoundingBoxes;
        public List<PolygonSoupChunk> Chunks;

        public PolygonSoupList()
        {
            //ChunkPointers = new List<uint>();
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

            List<uint> chunkPointers = new List<uint>();

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
                chunkPointers.Add(br.ReadUInt32());
            }

            for (int i = 0; i < result.ChunkCount; i++)
            {
                // Read Vertically

                long pos = result.BoxListStart + 0x70 * (i / 4) + 4 * (i % 4);

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
				br.BaseStream.Position += 12;

				boundingBox.Max = new Vector3(maxX, maxY, maxZ);

				PolygonSoupBoundingBox box = new PolygonSoupBoundingBox(boundingBox, br.ReadInt32());

				result.BoundingBoxes.Add(box);
            }

            for (int i = 0; i < chunkPointers.Count; i++)
            {
                br.BaseStream.Position = chunkPointers[i];

                result.Chunks.Add(PolygonSoupChunk.Read(br));
            }

            br.Close();
            ms.Close();

            return result;
        }

        public void Write(BundleEntry entry)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(Min);
            bw.Write(Unknown4);
            bw.Write(Max);
            bw.Write(Unknown8);
            bw.Write(ChunkPointerStart);
            bw.Write(BoxListStart);
            bw.Write(Chunks.Count);
            bw.Write(FileSize);
            
            uint[] chunkPointers = new uint[Chunks.Count];

            // No Data
            if (Chunks.Count == 0)
            {
                bw.Flush();
                byte[] data2 = ms.ToArray();
                bw.Close();
                ms.Close();

                entry.Header = data2;
                entry.Dirty = true;
                return;
            }

            bw.BaseStream.Position = ChunkPointerStart;
            
            for (int i = 0; i < Chunks.Count; i++)
            {
                bw.Write((uint)0);
            }

            for (int i = 0; i < Chunks.Count; i++)
            {
                // Write Vertically

                long pos = BoxListStart + 0x70 * (i / 4) + 4 * (i % 4);

                PolygonSoupBoundingBox box = BoundingBoxes[i];

                bw.BaseStream.Position = pos;
                bw.Write(box.Box.Min.X);
                bw.BaseStream.Position += 12;
                bw.Write(box.Box.Min.Y);
                bw.BaseStream.Position += 12;
                bw.Write(box.Box.Min.Z);

                bw.BaseStream.Position += 12;
                bw.Write(box.Box.Max.X);
                bw.BaseStream.Position += 12;
                bw.Write(box.Box.Max.Y);
                bw.BaseStream.Position += 12;
                bw.Write(box.Box.Max.Z);

                bw.BaseStream.Position += 12;
                bw.Write(box.Unknown);
            }

            bw.BaseStream.Position = (16 * ((bw.BaseStream.Position + 15) / 16));

            if (Chunks.Count > 0)
            {
                bw.BaseStream.Position = (128 * ((bw.BaseStream.Position + 127) / 128));

                for (int i = 0; i < Chunks.Count / 4; i++)
                {
                    if ((i % 8) % 3 == 0)
                    {
                        bw.BaseStream.Position += 256;
                    }
                    else
                    {
                        bw.BaseStream.Position += 384;
                    }
                }
            }

            for (int i = 0; i < chunkPointers.Length; i++)
            {
                bw.BaseStream.Position = (128 * ((bw.BaseStream.Position + 127) / 128));

                chunkPointers[i] = (uint)bw.BaseStream.Position;

                Chunks[i].Write(bw);
            }

            bw.BaseStream.Position = (16 * ((bw.BaseStream.Position + 15) / 16)) + 0x5F;
            bw.Write((byte)0);

            bw.BaseStream.Position = ChunkPointerStart;
            for (int i = 0; i < Chunks.Count; i++)
            {
                bw.Write(chunkPointers[i]);
            }

            bw.Flush();
            byte[] data = ms.ToArray();
            bw.Close();
            ms.Close();

            entry.Header = data;
            entry.Dirty = true;
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
                scene.AddObject(sceneObject);
                index++;
            }

            return scene;
        }

        private class ColMesh
        {
			public Dictionary<byte, Vector3> Points;
			public float Scale;
			public Vector3 Position;
            public Vector3 Min;
			public Vector3 Max;
			public List<PolygonSoupProperty> Properties;
        }

        public void ImportObj(string path)
        {
            Stream s = File.Open(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(s);

            List<Vector3> points = new List<Vector3>();

            Dictionary<string, ColMesh> meshes = new Dictionary<string, ColMesh>();

            PolygonSoupProperty lastUsedProperty = null;
            bool usedCurrentProperty = false;
            uint globalVertCount = 0;
			int curStartVertex = 0;
			int nextStartVertex = 0;
			int currentProperty = -1;
            string currentMesh = "";

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (line == null || line.Trim().StartsWith("#"))
                    continue;

                line = line.Trim();

                if (line.StartsWith("v"))
                {
					// read in this vertex and add it to the points (this happens first)
                    string[] options = line.Split(' ');
                    if (options.Length < 4)
                        throw new ReadFailedError("Invalid Vertex Line: " + line);
                    
                    Utilities.Parse(options[1], false, out float x);
                    Utilities.Parse(options[2], false, out float y);
                    Utilities.Parse(options[3], false, out float z);

                    globalVertCount++;
                    points.Add(new Vector3(x, y, z));

                } else if (line.StartsWith("f"))
                {
					// add the faces to the list of faces (this happens after)
                    string[] options = line.Split(' ');
                    if (options.Length < 4)
                        throw new ReadFailedError("Invalid Face Line: " + line);

                    try
                    {
                        Utilities.Parse(options[1].Split('/')[0], false, out uint f1);
                        Utilities.Parse(options[2].Split('/')[0], false, out uint f2);
                        Utilities.Parse(options[3].Split('/')[0], false, out uint f3);

                        if (usedCurrentProperty)
                        {
                            currentProperty++;
                            meshes[currentMesh].Properties.Add(lastUsedProperty?.Copy());
                        }

                        // find the face's points
                        Vector3 v1 = points[(int) f1 - 1];
                        Vector3 v2 = points[(int) f2 - 1];
                        Vector3 v3 = points[(int) f3 - 1];
                        meshes[currentMesh].Min = MathUtils.MinBounds(v1, v2, v3, meshes[currentMesh].Min);
                        meshes[currentMesh].Max = MathUtils.MaxBounds(v1, v2, v3, meshes[currentMesh].Max);
                        meshes[currentMesh].Position = meshes[currentMesh].Min;

                        uint vert1 = (uint)(f1 - 1 - curStartVertex);
                        uint vert2 = (uint)(f2 - 1 - curStartVertex);
                        uint vert3 = (uint)(f3 - 1 - curStartVertex);

                        if (vert1 > 255 || vert2 > 255 || vert3 > 255)
                            throw new ReadFailedError("PolygonSoupLists require that each mesh has less than 255 vertices.\nSplit up your mesh and try again.\n\nThis error occurred while importing: " + currentMesh);

                        byte localIndex1 = (byte) vert1;
                        byte localIndex2 = (byte) vert2;
                        byte localIndex3 = (byte) vert3;

                        // add the three points we just read in for this mesh to the current property (polygon)
                        meshes[currentMesh].Properties[currentProperty].Indices[0] = localIndex1;
                        meshes[currentMesh].Properties[currentProperty].Indices[1] = localIndex2;
                        meshes[currentMesh].Properties[currentProperty].Indices[2] = localIndex3;

                        if (!meshes[currentMesh].Points.ContainsKey(localIndex1))
                        {
                            meshes[currentMesh].Points.Add(localIndex1, v1);
                        }
                        if (!meshes[currentMesh].Points.ContainsKey(localIndex2))
                        {
                            meshes[currentMesh].Points.Add(localIndex2, v2);
                        }
                        if (!meshes[currentMesh].Points.ContainsKey(localIndex3))
                        {
                            meshes[currentMesh].Points.Add(localIndex3, v3);
                        }
                        meshes[currentMesh].Properties[currentProperty].Indices[3] = 0xFF;

                        usedCurrentProperty = true;
                        
                        /* Doesn't work - TODO: FIX
                        
                        // If there are more than 240 points then put them into another mesh
                        // 255 is the limit but let's use 240 to be safe
                        byte meshPointMax = 0;
                        foreach (byte key in meshes[currentMesh].Points.Keys)
                        {
                            if (meshPointMax < key)
                            {
                                meshPointMax = key;
                            }
                        }
                        if (meshPointMax >= 240)
                        {
                            currentMesh = currentMesh + "_";

                            curStartVertex = nextStartVertex;
                            nextStartVertex = (int)(globalVertCount - 1);
                            currentProperty = -1;

                            ColMesh colMesh = new ColMesh
                            {
                                Points = new Dictionary<byte, Vector3>(),
                                Properties = new List<PolygonSoupProperty>(),
                                Scale = 0.015f,
                                Position = new Vector3(float.MaxValue),
                                Min = new Vector3(float.MaxValue),
                                Max = new Vector3(float.MinValue)
                            };
                            meshes.Add(currentMesh, colMesh);
                        }*/
                    }
                    catch (NotSupportedException)
                    {
                        throw new ReadFailedError("Invalid Face Line: " + line);
                    }

                } else if (line.StartsWith("usemtl"))
                {
                    string[] options = line.Split(' ');
                    if (options.Length < 2)
                        throw new ReadFailedError("Invalid Material Line: " + line);
                    string material = options[1];
                    string[] matStrings = material.Split('_');
                    if (matStrings.Length < 7)
                        throw new ReadFailedError("Invalid Material: " + material);
                    string property1 = matStrings[1];
                    string property2 = matStrings[2];
                    string newByte1 = matStrings[3];
                    string newByte2 = matStrings[4];
                    string newByte3 = matStrings[5];
                    string newByte4 = matStrings[6];

                    try
                    {
                        Utilities.Parse(property1, true, out ushort prop1);
                        Utilities.Parse(property2, true, out ushort prop2);
                        Utilities.Parse(newByte1, true, out byte nByte1);
                        Utilities.Parse(newByte2, true, out byte nByte2);
                        Utilities.Parse(newByte3, true, out byte nByte3);
                        Utilities.Parse(newByte4, true, out byte nByte4);

                        usedCurrentProperty = false;

                        PolygonSoupProperty property = new PolygonSoupProperty();
                        property.UnknownProperty = (uint) ((prop2 << 16) | prop1);
                        //property.Indices is done above
                        property.UnknownBytes[0] = nByte1;
                        property.UnknownBytes[1] = nByte2;
                        property.UnknownBytes[2] = nByte3;
                        property.UnknownBytes[3] = nByte4;

                        lastUsedProperty = property;

                        currentProperty++;
                        meshes[currentMesh].Properties.Add(property);
                    }
                    catch (NotSupportedException)
                    {
                        throw new ReadFailedError("Invalid Material: " + material);
                    }

                } else if (line.StartsWith("g"))
                {
                    string[] options = line.Split(' ');
                    if (options.Length < 2)
                        throw new ReadFailedError("Invalid Group: <none>");
                    currentMesh = options[1];

                    // the next 2 lines were flipped before. How did that even work?!
                    nextStartVertex = (int)(globalVertCount - 1);
					curStartVertex = nextStartVertex;
                    currentProperty = -1;

                    ColMesh colMesh = new ColMesh
                    {
                        Points = new Dictionary<byte, Vector3>(),
                        Properties = new List<PolygonSoupProperty>(),
                        Scale = 0.015f,
                        Position = new Vector3(float.MaxValue),
                        Min = new Vector3(float.MaxValue),
                        Max = new Vector3(float.MinValue)
                    };
                    meshes.Add(currentMesh, colMesh);
                }
            }

            sr.Close();
            s.Close();

			Chunks.Clear();
			BoundingBoxes.Clear();

            // Convert ColMeshes to PolygonSoupChunks and generate bounding boxes
            foreach (string meshName in meshes.Keys)
            {
                ColMesh mesh = meshes[meshName];

				// at this point we have the mesh, we need to process its points
				// and we need to subtract the position and take scale into account here!
				// make vectors out of the three points we just read in with their X, Y and Z and take into account the scale/base

				PolygonSoupChunk chunk = new PolygonSoupChunk();
                chunk.PropertyList = mesh.Properties;
				byte meshPointMax = 0;
				foreach (byte key in mesh.Points.Keys)
				{
					if (meshPointMax < key)
					{
						meshPointMax = key;
					}
				}
				Vector3S[] verts = new Vector3S[meshPointMax + 1];
				foreach (byte key in mesh.Points.Keys)
				{
                    Vector3S pointProcessed = new Vector3S((short)Math.Round((mesh.Points[key].X - mesh.Position.X) / mesh.Scale), (short)Math.Round((mesh.Points[key].Y - mesh.Position.Y) / mesh.Scale), (short)Math.Round((mesh.Points[key].Z - mesh.Position.Z) / mesh.Scale));
                    verts[key] = pointProcessed;
				}
				chunk.PointList = verts.ToList();
				chunk.PointCount = (byte)(meshPointMax + 1);

                chunk.PropertyListCount = (byte)mesh.Properties.Count;
                chunk.QuadCount = 0; // TODO: when quads are supported.
				chunk.Scale = mesh.Scale;
				chunk.Position = new Vector3I((int)Math.Round(mesh.Position.X / mesh.Scale), (int)Math.Round(mesh.Position.Y / mesh.Scale), (int)Math.Round(mesh.Position.Z / mesh.Scale));
				int polygonSoupBoundingBoxUnknown = -1;
				BoundingBoxes.Add(new PolygonSoupBoundingBox(new BoxF(mesh.Min, mesh.Max), polygonSoupBoundingBoxUnknown));
				Chunks.Add(chunk);
            }
        }
    }
}
