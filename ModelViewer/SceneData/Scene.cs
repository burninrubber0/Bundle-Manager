using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace ModelViewer.SceneData
{
    public class Scene
    {
        internal Dictionary<string, SceneObject> SceneObjects;
        
        public int ObjectCount => SceneObjects.Count;

        public Scene()
        {
            SceneObjects = new Dictionary<string, SceneObject>();
        }

        public void ClearObjects()
        {
            SceneObjects.Clear();
        }

        public void AddObject(SceneObject obj)
        {
            while (SceneObjects.ContainsKey(obj.Name))
            {
                obj.Name += "_";
            }
            SceneObjects.Add(obj.Name, obj);
        }

        public void RemoveObject(string name)
        {
            SceneObjects.Remove(name);
        }

        public bool ContainsObject(string name)
        {
            return SceneObjects.ContainsKey(name);
        }

        public bool ContainsObject(SceneObject obj)
        {
            return SceneObjects.ContainsValue(obj);
        }

        public SceneObject MergeSceneObjects()
        {
            Model mergedModel = new Model();
            foreach (SceneObject obj in SceneObjects.Values)
            {
                mergedModel.Meshes.Add(obj.MergeMeshes());
            }

            Model resultModel = new Model(mergedModel.MergeMeshes());
            
            return new SceneObject("Merged", resultModel);
        }

        public List<Material> GetAllMaterials()
        {
            List<Material> result = new List<Material>();

            foreach (SceneObject obj in SceneObjects.Values)
            {
                foreach (Mesh mesh in obj.Model.Meshes)
                {
                    Material material = mesh.Material;

                    if (!result.Contains(material))
                        result.Add(material);
                }
            }

            return result;
        }

        public void ExportCollada14(string path)
        {
            // TODO: Implement Collada Export
        }

        public void ExportWavefrontObj(string path)
        {
            Stream s = File.Open(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(s);

            List<Material> materials = GetAllMaterials();

            int meshIndex = 0;

            int usedIndices = 0;
            foreach (SceneObject obj in SceneObjects.Values)
            {
                string name = obj.Name;
                sw.WriteLine("# Model " + name);

                Model model = obj.Model.Transformed(obj.Transform);
                for (int i = 0; i < model.Meshes.Count; i++)
                {
                    Mesh mesh = model.Meshes[i];

                    foreach (Vector3 vertex in mesh.Vertices)
                    {
                        sw.WriteLine("v " + vertex.X + " " + vertex.Y + " " + vertex.Z);
                    }

                    foreach (Vector2 uv1 in mesh.UV1)
                    {
                        sw.WriteLine("vt " + uv1.X + " " + uv1.Y);
                    }

                    sw.WriteLine();
                    
                    sw.WriteLine("g mesh" + meshIndex);

                    int materialIndex = materials.IndexOf(mesh.Material);
                    sw.WriteLine("usemtl material" + materialIndex);

                    sw.WriteLine();

                    for (int j = 0; j < mesh.Indices.Count; j += 3)
                    {
                        int v0 = mesh.Indices[j + 0] + 1 + usedIndices;
                        int v1 = mesh.Indices[j + 1] + 1 + usedIndices;
                        int v2 = mesh.Indices[j + 2] + 1 + usedIndices;
                        sw.WriteLine("f " + v0 + "/" + v0 + " " + v1 + "/" + v1 + " " + v2 + "/" + v2);
                    }

                    usedIndices += mesh.Vertices.Count;

                    sw.WriteLine();

                    meshIndex++;
                }
            }

            sw.Flush();
            sw.Close();
            s.Close();
        }

        public bool InitGraphics()
        {
            // TODO: Init Graphics

            return true;
        }

        public void Render()
        {
            // TODO: Render Scene
        }
    }
}
