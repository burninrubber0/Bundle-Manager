using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelViewer.SceneData
{
    public class Material
    {
        public string Name;
        public Image DiffuseMap { get; set; }
        public Image NormalMap { get; set; }
        public Image SpecularMap { get; set; }

        public Material(string name, Image diffuse, Image normal = null, Image specular = null)
        {
            Name = name;
            DiffuseMap = diffuse;
            NormalMap = normal;
            SpecularMap = specular;
        }
    }
}
