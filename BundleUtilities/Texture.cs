using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundleUtilities
{
	public class Texture
	{
		public readonly int Width, Height;

		public byte[] Data;

		public Texture(byte[] data, int width, int height)
		{
			Data = data;
			Width = width;
			Height = height;
		}

		public void Save(string path, ImageFormat format)
		{
			using (Bitmap bitmap = new Bitmap(Width, Height))
			{
				int index = 0;
				for (int y = 0; y < Height; y++)
				{
					for (int x = 0; x < Width; x++)
					{
						byte alpha = Data[index + 3];
						byte red = Data[index + 2];
						byte green = Data[index + 1];
						byte blue = Data[index + 0];
						Color color = Color.FromArgb(alpha, red, green, blue);
						bitmap.SetPixel(x, y, color);
						index += 4;
					}
				}
				bitmap.Save(path, format);
			}
		}
	}
}
