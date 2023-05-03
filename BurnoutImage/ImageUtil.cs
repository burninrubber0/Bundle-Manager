using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Forms;
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
