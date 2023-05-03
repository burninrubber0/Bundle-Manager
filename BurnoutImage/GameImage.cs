using System;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BCnEncoder.Shared;

namespace BurnoutImage
{
    public struct ImageInfo
    {
        public readonly byte[] Header;
        public readonly byte[] Data;

        public ImageInfo(byte[] header, byte[] data)
        {
            this.Header = header;
            this.Data = data;
        }
    }

    public class ImageHeader
    {
        public readonly CompressionType CompressionType;
        public readonly int Width, Height;

        public ImageHeader(CompressionType compression, int width, int height)
        {
            CompressionType = compression;
            Width = width;
            Height = height;
        }
    }

    public static class GameImage
    {
        public static ImageInfo SetImage(string path, int width, int height, CompressionType compression)
        {
            byte[] data;

            if (compression == CompressionType.BGRA)
            {
                Bitmap image = new Bitmap(Image.FromFile(path));
                MemoryStream mspixels = new MemoryStream();

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Color pixel = image.GetPixel(j, i);
                        mspixels.WriteByte(pixel.B);
                        mspixels.WriteByte(pixel.G);
                        mspixels.WriteByte(pixel.R);
                        mspixels.WriteByte(pixel.A);
                    }
                }

                data = mspixels.ToArray();
            }
            else
            {
                CompressionFormat dxt = CompressionFormat.Unknown;
                if (compression == CompressionType.DXT1)
                    dxt = CompressionFormat.Bc1;
                else if (compression == CompressionType.DXT5)
                    dxt = CompressionFormat.Bc3;
                data = ImageUtil.CompressImage(path, dxt);
            }

            MemoryStream msx = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(msx);

            // Original game header: https://burnout.wiki/wiki/Texture/PC
            // TODO: Implement as separate option
            //bw.Write(0); // Data pointer
            //bw.Write(0); // Texture interface pointer
            //bw.Write(0); // Padding
            //bw.Write((short)1); // Pool
            //bw.Write((byte)0); // ?
            //bw.Write((byte)0); // ?
            //if (compression == CompressionType.DXT1 || compression == CompressionType.DXT5) // Format
            //    bw.Write(Encoding.ASCII.GetBytes(compression.ToString()));
            //else
            //    bw.Write(0x15); // A8R8G8B8
            //bw.Write((short)width); // Width
            //bw.Write((short)height); // Height
            //bw.Write((byte)1); // Depth
            //bw.Write(1); // MipLevels (TODO: Support mipmapping)
            //bw.Write((byte)0); // Texture type
            //bw.Write((byte)0); // Flags

            // Remastered Texture header: https://burnout.wiki/wiki/Texture/Remastered
            bw.Write(0); // Texture interface pointer
            bw.Write(0); // Usage
            bw.Write(7); // Dimension
            bw.Write(0); // Pixel data pointer
            bw.Write(0); // Shader resource view interface pointer 1
            bw.Write(0); // Shader resource view interface pointer 2
            bw.Write(0); // ?
            if (compression == CompressionType.ARGB) // Format
                bw.Write(0x1C); // R8G8B8A8_UNORM
            else if (compression == CompressionType.DXT1)
                bw.Write(0x47); // BC1_UNORM
            else if (compression == CompressionType.DXT5)
                bw.Write(0x4D); // BC3_UNORM
            bw.Write(0); // Flags
            bw.Write((short)width); // Width
            bw.Write((short)height); // Height
            bw.Write((short)1); // Depth
            bw.Write((short)1); // Array size
            bw.Write((byte)0); // Most detailed mip
            bw.Write((byte)1); // Mip levels (TODO: Support mipmapping)
            bw.Write((short)0); // ?
            bw.Write(0); // ? pointer
            bw.Write(0); // Array index (unused)
            bw.Write(0); // Contents size (unused)
            bw.Write(0); // Texture data (unused)

            bw.Flush();
            bw.Close();

            return new ImageInfo(msx.ToArray(), data);
        }

        public static ImageHeader GetImageHeader(byte[] data)
        {
            try
            {
                MemoryStream ms = new MemoryStream(data);
                BinaryReader br = new BinaryReader(ms);
                if (data.Length == 0x40 || data.Length == 0x30)
                {
                    // Remaster
                    br.BaseStream.Seek(8, SeekOrigin.Begin);
                    uint unk1 = br.ReadUInt32();
                    uint unk2 = br.ReadUInt32();

                    br.BaseStream.Seek(0x1C, SeekOrigin.Begin);

                    CompressionType type = CompressionType.UNKNOWN;
                    byte[] compression = br.ReadBytes(4);
                    string compressionString = Encoding.ASCII.GetString(compression);
                    if (compression.Matches(new byte[] { 0x15, 0x00, 0x00, 0x00 }))
                    {
                        type = CompressionType.BGRA;
                    }
                    else if (compression.Matches(new byte[] { 0x1C, 0x00, 0x00, 0x00 }))
                    {
                        type = CompressionType.RGBA;
                    }
                    else if (compression.Matches(new byte[] { 0xFF, 0x00, 0x00, 0x00 }))
                    {
                        type = CompressionType.ARGB;
                    }
                    else if (compression.Matches(new byte[] { 0x47, 0x00, 0x00, 0x00 }))
                    {
                        type = CompressionType.DXT1;
                    }
                    else if (compression.Matches(new byte[] { 0x4D, 0x00, 0x00, 0x00 }))
                    {
                        type = CompressionType.DXT5;
                    }

                    uint unk3 = br.ReadUInt32();

                    int width = br.ReadInt16();
                    int height = br.ReadInt16();
                    br.Close();

                    return new ImageHeader(type, width, height);
                }
                else
                {
                    // OLD PC
                    br.BaseStream.Seek(0x10, SeekOrigin.Begin);
                    CompressionType type = CompressionType.UNKNOWN;
                    byte[] compression = br.ReadBytes(4);
                    string compressionString = Encoding.ASCII.GetString(compression);
                    if (compression.Matches(new byte[] { 0x15, 0x00, 0x00, 0x00 }))
                    {
                        type = CompressionType.BGRA;
                    }
                    else if (compression.Matches(new byte[] { 0xFF, 0x00, 0x00, 0x00 }))
                    {
                        type = CompressionType.ARGB;
                    }
                    else if (compressionString.StartsWith("DXT"))
                    {
                        switch (compressionString[3])
                        {
                            case '1':
                                type = CompressionType.DXT1;
                                break;
                            case '5':
                                type = CompressionType.DXT5;
                                break;
                        }
                    }

                    int width = br.ReadInt16();
                    int height = br.ReadInt16();
                    br.Close();

                    return new ImageHeader(type, width, height);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
            }

            return null;
        }

        public static Image GetImage(byte[] data, byte[] extraData)
        {
            try
            {
                ImageHeader header = GetImageHeader(data);
                byte[] pixels = extraData;

                if (header.CompressionType == CompressionType.DXT1)
                {
                    pixels = ImageUtil.DecompressImage(pixels, header.Width, header.Height, CompressionFormat.Bc1);
                }
                else if (header.CompressionType == CompressionType.DXT5)
                {
                    pixels = ImageUtil.DecompressImage(pixels, header.Width, header.Height, CompressionFormat.Bc3);
                }

                DirectBitmap bitmap = new DirectBitmap(header.Width, header.Height);

                int index = 0;
                for (int y = 0; y < header.Height; y++)
                {
                    for (int x = 0; x < header.Width; x++)
                    {
                        byte red;
                        byte green;
                        byte blue;
                        byte alpha;
                        if (header.CompressionType == CompressionType.BGRA)
                        {
                            blue = pixels[index + 0];
                            green = pixels[index + 1];
                            red = pixels[index + 2];
                            alpha = pixels[index + 3];
                        }
                        else
                        {
                            red = pixels[index + 0];
                            green = pixels[index + 1];
                            blue = pixels[index + 2];
                            alpha = pixels[index + 3];
                        }

                        Color color = Color.FromArgb(alpha, red, green, blue);
                        bitmap.Bits[x + y * header.Width] = color.ToArgb();
                        index += 4;
                    }
                }

                return bitmap.Bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
                return null;
            }
        }

        public static Image GetImagePS3(byte[] data, byte[] extraData)
        {
            if (extraData != null && data.Length == 48)
            {
                try
                {
                    MemoryStream ms = new MemoryStream(data);
                    BinaryReader br = new BinaryReader(ms);

                    byte compression = br.ReadByte();
                    byte[] unknown1 = br.ReadBytes(3);
                    CompressionType type = CompressionType.UNKNOWN;
                    if (compression == 0x85)
                    {
                        type = CompressionType.ARGB;
                    }
                    else if (compression == 0x86)
                    {
                        type = CompressionType.DXT1;
                    }
                    else if (compression == 0x88)
                    {
                        type = CompressionType.DXT5;
                    }
                    int unknown2 = Util.ReverseBytes(br.ReadInt32());
                    int width = Util.ReverseBytes(br.ReadInt16());
                    int height = Util.ReverseBytes(br.ReadInt16());
                    
                    br.Close();

                    byte[] pixels = extraData;

                    if (type == CompressionType.DXT1)
                    {
                        pixels = ImageUtil.DecompressImage(pixels, width, height, CompressionFormat.Bc1);
                    }
                    else if (type == CompressionType.DXT5)
                    {
                        pixels = ImageUtil.DecompressImage(pixels, width, height, CompressionFormat.Bc3);
                    }

                    DirectBitmap bitmap = new DirectBitmap(width, height);

                    int index = 0;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            byte red;
                            byte green;
                            byte blue;
                            byte alpha;
                            if (type == CompressionType.BGRA)
                            {
                                blue = pixels[index + 0];
                                green = pixels[index + 1];
                                red = pixels[index + 2];
                                alpha = pixels[index + 3];
                            }
                            else
                            {

                                alpha = pixels[index + 0];
                                red = pixels[index + 1];
                                green = pixels[index + 2];
                                blue = pixels[index + 3];
                            }

                            Color color = Color.FromArgb(alpha, red, green, blue);
                            bitmap.Bits[x + y * width] = color.ToArgb();
                            bitmap.SetPixel(x, y, color);
                            index += 4;
                        }
                    }
                    
                    return bitmap.Bitmap;

                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }

    public enum CompressionType
    {
        UNKNOWN,
        RGBA,
        ARGB,
        BGRA,
        DXT1,
        DXT5
    }
}
