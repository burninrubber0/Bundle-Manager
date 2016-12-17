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

namespace BurnoutImage
{
    internal static class ImageUtil
    {
        public static byte[] DecompressImage(byte[] data, int width, int height, DXTCompression compression)
        {
            return CompressionTools.DecompressTexture(data, width, height, compression);
        }

        public static byte[] CompressImage(/*byte[] rgba*/ Image image, /*int width, int height, */DXTCompression compression)
        {
            Bitmap bitmap = new Bitmap(image);

            byte[] pixels = new byte[image.Width * image.Height * 4];

            List<Color> pixelList = new List<Color>();

            for (int j = 0; j < image.Height; j++)
            {
                for (int i = 0; i < image.Width; i++)
                {
                    Color color = bitmap.GetPixel(i, j);
                    pixelList.Add(color);
                }
            }

            int index = 0;
            for (int i = 0; i < pixelList.Count; i++)
            {
                Color color = pixelList[i];
                pixels[index + 0] = color.B;
                pixels[index + 1] = color.G;
                pixels[index + 2] = color.R;
                pixels[index + 3] = color.A;
                index += 4;
            }
            
            byte[] result = CompressionTools.CompressTexture(pixels, image.Width, image.Height, compression);

            return result;
        }
    }

    public enum DXTCompression
    {
        DXT1,
        DXT3,
        DXT5
    }
}
