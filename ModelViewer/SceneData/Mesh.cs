using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ModelViewer.SceneData
{
    public class Mesh
    {
        private int _vertexArray;
        private int _indexArray;

        public Material Material { get; set; }

        public List<Vector3> Vertices { get; set; }
        public List<Vector3> Normals { get; set; }
        public List<Vector3> Normals2 { get; set; }
        public List<int> Indices { get; set; }
        public List<Vector2> UV1 { get; set; }
        public List<Vector2> UV2 { get; set; }
        public int ID1 { get; set; }
        public int ID2 { get; set; }

        public Mesh()
        {
            Vertices = new List<Vector3>();
            Normals = new List<Vector3>();
            Normals2 = new List<Vector3>();
            Indices = new List<int>();
            UV1 = new List<Vector2>();
            UV2 = new List<Vector2>();
            ID1 = 0;
            ID2 = 0;
        }

        public Mesh Copy()
        {
            Mesh result = new Mesh();

            result.Material = Material;

            result.Vertices = new List<Vector3>(Vertices);
            result.Normals = new List<Vector3>(Normals);
            result.Normals2 = new List<Vector3>(Normals2);
            result.Indices = new List<int>(Indices);
            result.UV1 = new List<Vector2>(UV1);
            result.UV2 = new List<Vector2>(UV2);
            result.ID1 = ID1;
            result.ID2 = ID2;

            return result;
        }

        public Mesh Merged(Mesh mesh)
        {
            Mesh result = Copy();

            for (int i = 0; i < mesh.Indices.Count; i++)
            {
                // TODO: Verify
                result.Indices.Add(result.Vertices.Count + mesh.Indices[i]);
            }

            result.Vertices.AddRange(mesh.Vertices);

            // TODO: Merge Normals, and UVs correclty for indices
            // TODO: Merge Materials somehow (make texture atlas?)

            return result;
        }

        public Mesh Transformed(Matrix4 transform)
        {
            Mesh result = Copy();

            result.Vertices.Clear();
            
            for (int i = 0; i < Vertices.Count; i++)
            {
                //transform.ClearProjection();
                /*Vector4 vertex = new Vector4(Vertices[i], 1);
                Vector4 mult = transform * vertex;
                Vector3 v = new Vector3(mult.X / mult.W, mult.Y / mult.W , mult.Z / mult.W);*/

                //Vector3 v = Vector3.TransformPerspective(Vertices[i], transform);
                //Vector3 v = new Vector3(mult.X, mult.Y, mult.Z);
                Vector3 v = Vertices[i];
                v *= transform.ExtractScale();
                v += transform.ExtractTranslation();
                v = Vector3.Transform(v, transform.ExtractRotation());
                result.Vertices.Add(v);
            }

            return result;
        }

        public bool InitGraphics()
        {
            // Vertices
            _vertexArray = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexArray);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * Vertices.Count, Vertices.ToArray(), BufferUsageHint.StaticDraw);

            // Indices

            _indexArray = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexArray);
            GL.BufferData(BufferTarget.ElementArrayBuffer, 4 * Indices.Count, Indices.ToArray(), BufferUsageHint.StaticDraw);

            return true;
        }

        public void Render(Matrix4 transform)
        {
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.IndexArray);
            
            GL.VertexAttrib3(0, transform.ExtractTranslation());
            GL.VertexPointer(3, VertexPointerType.Float, Vector3.SizeInBytes, 0);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexArray);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexArray);
            GL.DrawElements(PrimitiveType.Triangles, Indices.Count, DrawElementsType.UnsignedInt, 0);

            GL.DisableClientState(ArrayCap.IndexArray);
            GL.DisableClientState(ArrayCap.VertexArray);
        }

        public void Dispose()
        {
            GL.DeleteVertexArrays(1, ref _vertexArray);
        }
    }
}
