﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using OpenTK;

namespace ModelViewer.SceneData
{
    public class Scene
    {
        public Vector3 Center { get; private set; }
        public float HorizSize { get; private set; }
        public float VertSize { get; private set; }

        internal Dictionary<string, SceneObject> SceneObjects;
        
        public int ObjectCount => SceneObjects.Count;

        public Scene()
        {
            SceneObjects = new Dictionary<string, SceneObject>();

            Center = Vector3.Zero;
            HorizSize = 3.0f;
            VertSize = 1.0f;
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

            UpdateSizing();
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

        private void ExportMTL(string mtlPath, string materialDir, string materialDirPath, List<Material> materials)
        {
            Stream s1 = File.Open(mtlPath, FileMode.Create);
            StreamWriter sw1 = new StreamWriter(s1);
            for (int i = 0; i < materials.Count; i++)
            {
                Material material = materials[i];

                string materialFileName = "material_" + material.Name + ".png";
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
            ExportMTL(mtlPath, materialDir, materialDirPath, materials);

            try
            {
                Stream s = File.Open(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(s);

                sw.WriteLine("mtllib " + mtlPath);

                int meshIndex = 0;

                uint usedIndices = 0;
                foreach (SceneObject obj in SceneObjects.Values)
                {
                    string name = obj.Name;
                    sw.WriteLine("# Model " + name);

                    Model model = obj.Model.Transformed(obj.Transform);
                    foreach (Mesh mesh in model.Meshes)
                    {
                        foreach (Vector3 vertex in mesh.Vertices)
                        {
                            sw.WriteLine("v " + vertex.X + " " + vertex.Y + " " + vertex.Z);
                        }

                        foreach (Vector2 uv1 in mesh.UV1)
                        {
                            sw.WriteLine("vt " + uv1.X + " " + -uv1.Y);
                        }

                        sw.WriteLine();

                        sw.WriteLine("g mesh" + meshIndex);

                        sw.WriteLine("usemtl material_" + mesh.Material.Name);

                        sw.WriteLine();

                        for (int j = 0; j < mesh.Indices.Count; j += 3)
                        {
                            uint v0 = mesh.Indices[j + 0] + 1 + usedIndices;
                            uint v1 = mesh.Indices[j + 1] + 1 + usedIndices;
                            uint v2 = mesh.Indices[j + 2] + 1 + usedIndices;
                            sw.WriteLine("f " + v0 + "/" + v0 + " " + v1 + "/" + v1 + " " + v2 + "/" + v2);
                        }

                        usedIndices += (uint) mesh.Vertices.Count;

                        sw.WriteLine();

                        meshIndex++;
                    }
                }

                sw.Flush();
                sw.Close();
                s.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void UpdateSizing()
        {
            float highestAbsHorizSize = 0.0f;
            float highestVertSize = 0.0f;

            float maxX = 0.0f;
            float minX = float.MaxValue;
            float maxY = 0.0f;
            float minY = float.MaxValue;
            float maxZ = 0.0f;
            float minZ = float.MaxValue;

            Vector3 closestToCenter = new Vector3(float.NaN);

            foreach (SceneObject obj in SceneObjects.Values)
            {
                foreach (Mesh mesh in obj.Model.Meshes)
                {
                    for (int i = 0; i < mesh.Vertices.Count; i++)
                    {
                        Vector3 vert = mesh.Vertices[i];

                        float absX = Math.Abs(vert.X);
                        float absZ = Math.Abs(vert.Z);

                        float maxHoriz = Math.Max(absX, absZ);

                        highestAbsHorizSize = Math.Max(maxHoriz, highestAbsHorizSize);
                        highestVertSize = Math.Max(vert.Y, highestVertSize);

                        maxX = Math.Max(vert.X, maxX);
                        minX = Math.Min(vert.X, minX);
                        maxY = Math.Max(vert.Y, maxY);
                        minY = Math.Min(vert.Y, minY);
                        maxZ = Math.Max(vert.Z, maxZ);
                        minZ = Math.Min(vert.Z, minZ);
                    }
                }
            }

            float midX = (minX + maxX) / 2;
            float midY = (minY + maxY) / 2;
            float midZ = (minZ + maxZ) / 2;

            HorizSize = highestAbsHorizSize + 1;
            VertSize = highestVertSize + 1;

            Center = new Vector3(midX, midY, midZ);
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

        public void Update()
        {
            foreach (SceneObject obj in SceneObjects.Values)
                obj.Update();
        }

        public void Render(ICamera camera)
        {
            foreach (SceneObject obj in SceneObjects.Values)
                obj.Render(camera);
        }

        public void Dispose()
        {
            foreach (SceneObject obj in SceneObjects.Values)
                obj.Dispose();
        }
    }
}