using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace ModelViewer.SceneData
{
    public class SceneObject
    {
        public Model Model { get; set; }
        public Matrix4 Transform { get; set; }
        public string Name { get; set; }

        public SceneObject(string name, Model model)
        {
            Name = name;
            Model = model;
            Transform = Matrix4.Identity;
        }

        public SceneObject(string name, Model model, Matrix4 transform)
        {
            Name = name;
            Model = model;
            Transform = transform;
        }

        public Mesh MergeMeshes()
        {
            return Model.MergeMeshes().Transformed(Transform);
        }

        public Model MergeMeshesToModel()
        {
            return new Model(MergeMeshes());
        }
    }
}
