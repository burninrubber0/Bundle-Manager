using BundleUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelViewer.SceneData
{
    public class Material
    {
        public string Name;
        public Texture DiffuseMap { get; set; }
        public Texture NormalMap { get; set; }
        public Texture SpecularMap { get; set; }

        public Color Color { get; set; }

        public Material(string name, Color color, Texture diffuse = null, Texture normal = null, Texture specular = null)
        {
            Name = name;
            Color = color;
            DiffuseMap = diffuse;
            NormalMap = normal;
            SpecularMap = specular;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Material other))
                return false;

            return other.Name == Name && other.DiffuseMap == DiffuseMap && other.NormalMap == NormalMap &&
                   other.SpecularMap == SpecularMap;
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (DiffuseMap != null ? DiffuseMap.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (NormalMap != null ? NormalMap.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (SpecularMap != null ? SpecularMap.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Color.GetHashCode();
                return hashCode;
            }
        }
    }
}
