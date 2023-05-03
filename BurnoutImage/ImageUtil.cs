using BCnEncoder.Shared;

namespace BurnoutImage
{
    internal static class ImageUtil
    {
        public static byte[] DecompressImage(byte[] data, int width, int height, CompressionFormat compression)
        {
            return CompressionTools.DecompressTexture(data, width, height, compression);
        }

        public static byte[] CompressImage(string path, CompressionFormat compression)
        {
            return CompressionTools.CompressTexture(path, compression);
        }
    }
}
