using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using DDSTools;

namespace BurnoutImage
{
    public static class CompressionTools
    {
        public static byte[] CompressTexture(byte[] source, int width, int height, DXTCompression compression)
        {
            return DDS.CompressTexture(source, width, height, (DDSCompression)compression);
        }

        public static byte[] DecompressTexture(byte[] source, int width, int height, DXTCompression compression)
        {
            return DDS.DecompressTexture(source, width, height, (DDSCompression)compression);
        }
    }
}
