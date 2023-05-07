namespace ModelViewer
{
    public static class OpenTKCompat
    {
        public static OpenTK.Matrix4 ToOTK3Matrix4(OpenTK.Mathematics.Matrix4 matrix)
        {
            return new OpenTK.Matrix4(
                matrix.M11, matrix.M12, matrix.M13, matrix.M14,
                matrix.M21, matrix.M22, matrix.M23, matrix.M24,
                matrix.M31, matrix.M32, matrix.M33, matrix.M34,
                matrix.M41, matrix.M42, matrix.M43, matrix.M44
                );
        }

        public static OpenTK.Mathematics.Matrix4 ToOTK4Matrix4(OpenTK.Matrix4 matrix)
        {
            return new OpenTK.Mathematics.Matrix4(
                matrix.M11,matrix.M12,matrix.M13,matrix.M14,
                matrix.M21,matrix.M22,matrix.M23,matrix.M24,
                matrix.M31,matrix.M32,matrix.M33,matrix.M34,
                matrix.M41,matrix.M42,matrix.M43,matrix.M44
                );
        }
    }
}
