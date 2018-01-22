﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
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
            string fullPath = Path.GetFullPath(path).Replace(Path.GetFileName(path), "");
            string mtlPath = fullPath + Path.GetFileNameWithoutExtension(path) + ".mtl";
            string materialDir = Path.GetFileNameWithoutExtension(path) + "_materials\\";
            string materialDirPath = fullPath + "/" + materialDir;
            if (!Directory.Exists(materialDirPath))
                Directory.CreateDirectory(materialDirPath);

            List<Material> materials = GetAllMaterials();

            Stream s1 = File.Open(mtlPath, FileMode.Create);
            StreamWriter sw1 = new StreamWriter(s1);
            for (int i = 0; i < materials.Count; i++)
            {
                Material material = materials[i];

                string materialFileName =  "material_" + material.Name + ".png";
                string materialRelativePathName = materialDir + materialFileName;
                string materialPathName = materialDirPath + materialFileName;

                sw1.WriteLine("newmtl material_" + material.Name);
                sw1.WriteLine("map_Kd " + materialRelativePathName);

                sw1.WriteLine();

                Image image = material.DiffuseMap;
                //Bitmap bitmap = new Bitmap(image);
                try
                {
                    image?.Save(materialPathName, ImageFormat.Png);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + "\n" + ex.StackTrace);
                }
            }

            sw1.Flush();
            sw1.Close();
            s1.Close();

            Stream s = File.Open(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(s);

            sw.WriteLine("mtllib " + mtlPath);
            
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
                    
                    for (int j = 0; j < mesh.Vertices.Count; j++)
                    {
                        Vector3 vertex = mesh.Vertices[j];
                        sw.WriteLine("v " + vertex.X + " " + vertex.Y + " " + vertex.Z);

                        /*if (j < mesh.UV1.Count)
                        {
                            Vector2 uv1 = mesh.UV1[j];
                            sw.WriteLine("vt " + uv1.X + " " + uv1.Y);
                        }*/
                    }

                    foreach (Vector2 uv1 in mesh.UV1)
                    {
                        sw.WriteLine("vt " + uv1.X + " " + -uv1.Y);
                    }

                    sw.WriteLine();
                    
                    sw.WriteLine("g mesh" + meshIndex);

                    //int materialIndex = materials.IndexOf(mesh.Material);
                    sw.WriteLine("usemtl material_" + mesh.Material.Name);//materialIndex);

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
            foreach (SceneObject obj in SceneObjects.Values)
            {
                if (!obj.InitGraphics())
                    return false;
            }

            return true;
        }

        public void Render()
        {
            foreach (SceneObject obj in SceneObjects.Values)
                obj.Render();
        }

        public void Dispose()
        {
            foreach (SceneObject obj in SceneObjects.Values)
                obj.Dispose();
        }
    }
}
