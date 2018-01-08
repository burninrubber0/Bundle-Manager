using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace ModelViewer.SceneData
{
    public class Model
    {
        internal List<Mesh> Meshes;

        public int MeshCount => Meshes.Count;

        public Model(params Mesh[] meshes)
        {
            Meshes = new List<Mesh>();

            Meshes.AddRange(meshes);
        }

        public Model Copy()
        {
            Model result = new Model();

            foreach (Mesh mesh in Meshes)
            {
                result.Meshes.Add(mesh.Copy());
            }

            return result;
        }

        public void AddMesh(Mesh mesh)
        {
            Meshes.Add(mesh);
        }

        public void RemoveMesh(int index)
        {
            Meshes.RemoveAt(index);
        }

        public void RemoveMesh(Mesh mesh)
        {
            Meshes.Remove(mesh);
        }

        public bool ContainsMesh(Mesh mesh)
        {
            return Meshes.Contains(mesh);
        }

        public Model Transformed(Matrix4 transform)
        {
            Model result = new Model();

            foreach (Mesh mesh in Meshes)
            {
                result.Meshes.Add(mesh.Transformed(transform));
            }

            return result;
        }

        public Mesh MergeMeshes()
        {
            Mesh result = new Mesh();
            
            foreach (Mesh mesh in Meshes)
            {
                result = result.Merged(mesh);
            }

            return result;
        }
    }
}
