using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace BurnoutImage
{
	public class DirectBitmap : IDisposable
	{
		public Bitmap Bitmap { get; private set; }
		//public byte[] Bits { get; private set; }
		public int[] Bits { get; private set; }
		public bool Disposed { get; private set; }
		public int Height { get; private set; }
		public int Width { get; private set; }

		protected GCHandle BitsHandle { get; private set; }

		public DirectBitmap(int width, int height)
		{
			Width = width;
			Height = height;
			//Bits = new byte[width * height * 4];
			Bits = new int[width * height];
			BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
			Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
		}

		public void SetPixel(int x, int y, Color colour)
		{
			int index = x + (y * Width);

			/*Bits[index + 0] = colour.A;
			Bits[index + 1] = colour.R;
			Bits[index + 2] = colour.G;
			Bits[index + 3] = colour.B;*/

			int col = colour.ToArgb();

			Bits[index] = col;
		}

		public Color GetPixel(int x, int y)
		{
			int index = x + (y * Width);

			/*byte alpha = Bits[index + 0];
			byte red = Bits[index + 1];
			byte green = Bits[index + 2];
			byte blue = Bits[index + 3];
			Color result = Color.FromArgb(alpha, red, green, blue);*/

			int col = Bits[index];
			Color result = Color.FromArgb(col);

			return result;
		}

		public void Dispose()
		{
			if (Disposed) return;
			Disposed = true;
			Bitmap.Dispose();
			BitsHandle.Free();
		}
	}
}
