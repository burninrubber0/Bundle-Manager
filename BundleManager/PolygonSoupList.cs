﻿using System;
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
        public uint UnknownProperty;
        public byte[] Indices;
		public byte[] IndicesIndices;

		public PolygonSoupProperty()
        {
            Indices = new byte[4];
			IndicesIndices = new byte[4];
		}

        public static PolygonSoupProperty Read(BinaryReader br)
        {
            PolygonSoupProperty result = new PolygonSoupProperty();

            result.UnknownProperty = br.ReadUInt32();

            for (int i = 0; i < result.Indices.Length; i++)
            {
                result.Indices[i] = br.ReadByte();
			}

			for (int i = 0; i < result.IndicesIndices.Length; i++)
			{
				result.IndicesIndices[i] = br.ReadByte();
			}

            return result;
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(UnknownProperty);

            for (int i = 0; i < Indices.Length; i++)
            {
                bw.Write(Indices[i]);
            }

            for (int i = 0; i < IndicesIndices.Length; i++)
            {
                bw.Write(IndicesIndices[i]);
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
        public byte Unknown8;
        public byte Unknown9;
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
            result.Unknown8 = br.ReadByte();
            result.Unknown9 = br.ReadByte();
            result.PointCount = br.ReadByte();
            result.Unknown10 = br.ReadByte();
            result.Unknown11 = br.ReadInt16();

            br.BaseStream.Position = result.PointListStart;
            for (int i = 0; i < result.PointCount; i++)
            {
                result.PointList.Add(br.ReadVector3S());
            }

            br.BaseStream.Position = result.PropertyListStart;

            int count = (result.Unknown9 >> 1) * 2 +
                        (result.Unknown9 - (result.Unknown9 >> 1) * 2) +
                        (((result.Unknown8 - result.Unknown9) >> 2) * 4) +
                        ((result.Unknown8 - result.Unknown9) - ((result.Unknown8 - result.Unknown9) >> 2) * 4);

            for (int i = 0; i < count; i++)
            {
                result.PropertyList.Add(PolygonSoupProperty.Read(br));
            }

            return result;
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(Position);
            bw.Write(Scale);
            bw.Write(PropertyListStart);
            bw.Write(PointListStart);
            bw.Write(Unknown7);
            bw.Write(Unknown8);
            bw.Write(Unknown9);
            bw.Write(PointCount);
            bw.Write(Unknown10);
            bw.Write(Unknown11);

            bw.BaseStream.Position = PointListStart;
            for (int i = 0; i < PointCount; i++)
            {
                bw.Write(PointList[i]);
            }

            bw.BaseStream.Position = PropertyListStart;

            int count = (Unknown9 >> 1) * 2 +
                        (Unknown9 - (Unknown9 >> 1) * 2) +
                        (((Unknown8 - Unknown9) >> 2) * 4) +
                        ((Unknown8 - Unknown9) - ((Unknown8 - Unknown9) >> 2) * 4);

            for (int i = 0; i < count; i++)
            {
                PropertyList[i].Write(bw);
            }
        }

        public Mesh BuildMesh(Vector3I pos, float scale)
        {
            Mesh mesh = new Mesh();

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
                bw.Write(ChunkPointers[i]);
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

            for (int i = 0; i < ChunkPointers.Count; i++)
            {
                bw.BaseStream.Position = ChunkPointers[i];

                Chunks[i].Write(bw);
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