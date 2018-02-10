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

            if (unknownProperty1 > 0x9D64 && unknownProperty1 != 0xFFFF)
            {
                unknownProperty1 = 0x8316; //0x9531; // 0x9007 //0x8511 //0x8316;
            }


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
            result.Unknown7 = br.ReadInt16();
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
            bw.Write(Position);
            bw.Write(Scale);
            long propListStartPtr = bw.BaseStream.Position;
            bw.Write((uint) 0);//PropertyListStart);
            long pointListStartPtr = bw.BaseStream.Position;
            bw.Write((uint) 0);//PointListStart);
            bw.Write(Unknown7);
            bw.Write(PropertyListCount);
            bw.Write(QuadCount);
            bw.Write(PointCount);
            bw.Write(Unknown10);
            bw.Write(Unknown11);

            //bw.BaseStream.Position = PointListStart;
            //bw.BaseStream.Position += (16 - bw.BaseStream.Position % 16);
            long cPos = bw.BaseStream.Position;
            bw.BaseStream.Position = pointListStartPtr;
            bw.Write((uint)cPos);
            bw.BaseStream.Position = cPos;

            for (int i = 0; i < PointCount; i++)
            {
                bw.Write(PointList[i]);
            }

            //bw.BaseStream.Position = PropertyListStart;

            bw.BaseStream.Position = (16 * ((bw.BaseStream.Position + 15) / 16));
            cPos = bw.BaseStream.Position;
            bw.BaseStream.Position = propListStartPtr;
            bw.Write((uint)cPos);
            bw.BaseStream.Position = cPos;

            for (int i = 0; i < PropertyListCount; i++)
            {
                PropertyList[i].Write(bw);
            }
        }

        //public static uint Upper = 0;

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
                if (unknownProperty1 > 0x9D64 && unknownProperty1 != 0xFFFF)
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
                {
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
                }
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
            if (entry.ID == Crc32.HashCrc32B("trk_col_221"))
                ImportObj("E:\\notrains4.obj");

            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(Min);
            bw.Write(Unknown4);
            bw.Write(Max);
            bw.Write(Unknown8);
            bw.Write(ChunkPointerStart);
            bw.Write(BoxListStart);
            bw.Write(ChunkCount);
            bw.Write(FileSize);

            // No Data
            if (ChunkCount == 0)
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
            
            for (int i = 0; i < ChunkCount; i++)
            {
                bw.Write((uint)0); //ChunkPointers[i]);
            }

            //br.BaseStream.Position += (16 - br.BaseStream.Position % 16);
            //br.BaseStream.Position = BoxListStart;

            for (int i = 0; i < ChunkCount; i++)
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

            if (ChunkCount > 0)
            {
                bw.BaseStream.Position = (128 * ((bw.BaseStream.Position + 127) / 128));

                for (int i = 0; i < ChunkCount / 4; i++)
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

            for (int i = 0; i < ChunkPointers.Count; i++)
            {
                bw.BaseStream.Position = (128 * ((bw.BaseStream.Position + 127) / 128));

                //bw.BaseStream.Position = ChunkPointers[i];
                ChunkPointers[i] = (uint)bw.BaseStream.Position;

                Chunks[i].Write(bw);
            }

            bw.BaseStream.Position = (16 * ((bw.BaseStream.Position + 15) / 16)) + 0x5F;
            bw.Write((byte)0);

            bw.BaseStream.Position = ChunkPointerStart;
            for (int i = 0; i < ChunkCount; i++)
            {
                bw.Write(ChunkPointers[i]);
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
                /*List<Mesh> meshes = chunk.BuildMesh(pos, scale);
                for (int i = 0; i < meshes.Count; i++)
                {
                    Mesh mesh = meshes[i];
                    Model model = new Model(mesh);
                    SceneObject sceneObject = new SceneObject(id + "_" + i, model);
                    //sceneObject.ID = id;
                    //sceneObject.Transform = Matrix4.CreateScale(scale) *
                    //                        Matrix4.CreateTranslation(new Vector3(pos.X, pos.Y, pos.Z));
                    scene.AddObject(sceneObject);
                }*/
                Model model = new Model(chunk.BuildMesh(pos, scale));
                SceneObject sceneObject = new SceneObject(id, model);
                //sceneObject.ID = id;
                //sceneObject.Transform = Matrix4.CreateScale(scale) *
                //                        Matrix4.CreateTranslation(new Vector3(pos.X, pos.Y, pos.Z));
                scene.AddObject(sceneObject);
                index++;

                // TODO: TEMP
                //break;
            }

            return scene;
        }

        private class ColMesh
        {
            public int ID;
            public string Name;
            public Dictionary<byte, Vector3S> Points;
            public List<PolygonSoupProperty> Properties;
            //public List<uint> Indices;
        }

        public void ImportObj(string path)
        {
            Stream s = File.Open(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(s);

            List<Vector3> points = new List<Vector3>();
            //List<uint> indices = new List<uint>();

            Dictionary<string, ColMesh> meshes = new Dictionary<string, ColMesh>();

            float scale = 1.0f;
            Vector3I pos = new Vector3I();

            PolygonSoupProperty lastUsedProperty = null;
            bool usedCurrentProperty = false;
            uint globalVertCount = 0;
			int curStartVertex = 0;
			int nextStartVertex = 0;
			int currentProperty = -1;
            string currentMesh = "";
            //PolygonSoupProperty currentProperty;

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

                    if (!float.TryParse(options[1], NumberStyles.Any, CultureInfo.CurrentCulture,
                        out var x))
                    {
                        throw new ReadFailedError("Invalid Coord: " + options[1]);
                    }

                    if (!float.TryParse(options[2], NumberStyles.Any, CultureInfo.CurrentCulture,
                        out var y))
                    {
                        throw new ReadFailedError("Invalid Coord: " + options[2]);
                    }

                    if (!float.TryParse(options[3], NumberStyles.Any, CultureInfo.CurrentCulture,
                        out var z))
                    {
                        throw new ReadFailedError("Invalid Coord: " + options[3]);
                    }

                    globalVertCount++;
                    points.Add(new Vector3(x, y, z));

                } else if (line.StartsWith("f"))
                {
					// add the faces to the list of faces (this happens after)
                    string[] options = line.Split(' ');
                    if (options.Length < 4)
                        throw new ReadFailedError("Invalid Face Line: " + line);

					if (!uint.TryParse(options[1].Split('/')[0], NumberStyles.None, CultureInfo.CurrentCulture,
                        out var f1))
                    {
                        throw new ReadFailedError("Invalid Index: " + options[1]);
                    }

                    if (!uint.TryParse(options[2].Split('/')[0], NumberStyles.None, CultureInfo.CurrentCulture,
                        out var f2))
                    {
                        throw new ReadFailedError("Invalid Index: " + options[2]);
                    }

                    if (!uint.TryParse(options[3].Split('/')[0], NumberStyles.None, CultureInfo.CurrentCulture,
                        out var f3))
                    {
                        throw new ReadFailedError("Invalid Index: " + options[3]);
                    }

                    if (usedCurrentProperty)
                    {
                        currentProperty++;
                        meshes[currentMesh].Properties.Add(lastUsedProperty.Copy());
                    }

					// find the face's points
                    Vector3 v1 = points[(int)f1 - 1];
                    Vector3 v2 = points[(int)f2 - 1];
                    Vector3 v3 = points[(int)f3 - 1];

					// make vectors out of the three points we just read in with their X, Y and Z and take into account the scale/base
                    Vector3S v1S = new Vector3S((short)(Math.Round(v1.X / scale) - pos.X), (short)(Math.Round(v1.Y / scale) - pos.Y), (short)(Math.Round(v1.Z / scale) - pos.Z));
                    Vector3S v2S = new Vector3S((short)(Math.Round(v2.X / scale) - pos.X), (short)(Math.Round(v2.Y / scale) - pos.Y), (short)(Math.Round(v2.Z / scale) - pos.Z));
                    Vector3S v3S = new Vector3S((short)(Math.Round(v3.X / scale) - pos.X), (short)(Math.Round(v3.Y / scale) - pos.Y), (short)(Math.Round(v3.Z / scale) - pos.Z));

					byte localIndex1 = (byte)(f1 - 1 - curStartVertex);
					byte localIndex2 = (byte)(f2 - 1 - curStartVertex);
					byte localIndex3 = (byte)(f3 - 1 - curStartVertex);
					// add the three points we just read in for this mesh to the current property (polygon)
					meshes[currentMesh].Properties[currentProperty].Indices[0] = localIndex1;//(byte)(f1 - 1 - startVertex);
                    meshes[currentMesh].Properties[currentProperty].Indices[1] =
						localIndex2;//1;//(byte)(f2 - 1 - startVertex);
                    meshes[currentMesh].Properties[currentProperty].Indices[2] =
						localIndex3;//2;//(byte)(f3 - 1 - startVertex);

					// then add them to the mesh
					/*
                    meshes[currentMesh][f1] = v1S;
                    meshes[currentMesh][f2] = v2S;
                    meshes[currentMesh][f3] = v3S;
					*/
					if (!meshes[currentMesh].Points.ContainsKey(localIndex1))
					{
						meshes[currentMesh].Points.Add(localIndex1, v1S);
					}
					if (!meshes[currentMesh].Points.ContainsKey(localIndex2))
					{
						meshes[currentMesh].Points.Add(localIndex2, v2S);
					}
					if (!meshes[currentMesh].Points.ContainsKey(localIndex3))
					{
						meshes[currentMesh].Points.Add(localIndex3, v3S);
					}

					// dealing with... quads? in an OBJ file!?
					/*
                    if (options.Length > 4)
                    {
                        if (!uint.TryParse(options[4], NumberStyles.None, CultureInfo.CurrentCulture,
                            out var f4))
                        {
                            throw new ReadFailedError("Invalid Index: " + options[4]);
                        }

                        Vector3 v4 = points[(int)f4 - 1];
                        Vector3S v4S = new Vector3S((short)((v4.X / scale) - pos.X), (short)((v4.Y / scale) - pos.Y), (short)((v4.Z / scale) - pos.Z));

                        meshes[currentMesh].Properties[currentProperty].Indices[3] =
                            (byte) (meshes[currentMesh].Points.Count + 0); //3;//(byte)(f4 - 1 - startVertex);

                        meshes[currentMesh].Points.Add(v4S);
                    }
                    else
                    {*/
                        meshes[currentMesh].Properties[currentProperty].Indices[3] = 0xFF;
                    //}

                    usedCurrentProperty = true;

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

                    if (!ushort.TryParse(property1, NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture,
                        out var prop1))
                    {
                        throw new ReadFailedError("Invalid Property1: " + property1);
                    }

                    if (!ushort.TryParse(property2, NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture,
                        out var prop2))
                    {
                        throw new ReadFailedError("Invalid Property2: " + property1);
                    }

                    if (!byte.TryParse(newByte1, NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture,
                        out var nByte1))
                    {
                        throw new ReadFailedError("Invalid NByte1: " + property1);
                    }

                    if (!byte.TryParse(newByte2, NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture,
                        out var nByte2))
                    {
                        throw new ReadFailedError("Invalid NByte2: " + property1);
                    }

                    if (!byte.TryParse(newByte3, NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture,
                        out var nByte3))
                    {
                        throw new ReadFailedError("Invalid NByte3: " + property1);
                    }

                    if (!byte.TryParse(newByte4, NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture,
                        out var nByte4))
                    {
                        throw new ReadFailedError("Invalid NByte4: " + property1);
                    }

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

                } else if (line.StartsWith("g"))
                {
                    string[] options = line.Split(' ');
                    if (options.Length < 2)
                        throw new ReadFailedError("Invalid Group: <none>");
                    currentMesh = options[1];
					//if (currentMesh == "mesh0")
					//    Debugger.Break();

					curStartVertex = nextStartVertex;
                    nextStartVertex = (int)(globalVertCount - 1);
                    currentProperty = -1;

                    ColMesh colMesh = new ColMesh();

                    // TODO: Only works if mesh already exists in this PolygonSoupList
                    {
                        string meshId = currentMesh.Substring("mesh".Length);
                        if (!int.TryParse(meshId, NumberStyles.None, CultureInfo.CurrentCulture, out var meshIndex))
                        {
                            throw new ReadFailedError("Invalid Group: " + meshId);
                        }

                        scale = Chunks[meshIndex].Scale; // 0.02f
                        pos = Chunks[meshIndex].Position; // get center and use that

                        colMesh.ID = meshIndex;
                    }

                    colMesh.Name = currentMesh;
                    colMesh.Points = new Dictionary<byte, Vector3S>();
                    colMesh.Properties = new List<PolygonSoupProperty>();
                    //colMesh.Indices = new List<uint>();
                    meshes.Add(currentMesh, colMesh);
                }
            }

            sr.Close();
            s.Close();

            foreach (string meshName in meshes.Keys)
            {
                ColMesh mesh = meshes[meshName];

                PolygonSoupChunk chunk = Chunks[mesh.ID];
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
					verts[key] = mesh.Points[key];
				}
				chunk.PointList = verts.ToList();
				chunk.PointCount = (byte)(meshPointMax + 1);

                //int count = chunk.PropertyList.Count;

                chunk.PropertyListCount = (byte)mesh.Properties.Count; //0;
                chunk.QuadCount = 0; // TODO: when quads are supported.
            }
        }
    }
}
