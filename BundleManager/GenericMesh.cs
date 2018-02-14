using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelViewer.SceneData;
using OpenTK;

namespace BundleManager
{
    public class Face
    {
        public List<uint> Indices;
        public List<uint> UVIndices;
        public Material Material;

        public Face(Material material = null)
        {
            Indices = new List<uint>();
            UVIndices = new List<uint>();
            Material = material;
        }

        public Face(uint[] indices, uint[] uvIndices = null, Material material = null) : this(material)
        {
            Indices.AddRange(indices);
            if (uvIndices != null)
                UVIndices.AddRange(uvIndices);
        }
    }

    public class GenericMesh
    {
        public string Name;
        public Dictionary<uint, Vector3> Vertices;
        public Dictionary<uint, Vector2> UVs;
        public List<Face> Faces;

        public GenericMesh()
        {
            Name = "";
            Vertices = new Dictionary<uint, Vector3>();
            UVs = new Dictionary<uint, Vector2>();
            Faces = new List<Face>();
        }

        public GenericMesh(string name) : this()
        {
            Name = name;
        }
    }
}
