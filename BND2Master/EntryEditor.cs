using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BundleFormat;
using System.IO;
using System.Reflection;
using BurnoutImage;
using System.Drawing.Imaging;

namespace BND2Master
{
    public partial class EntryEditor : Form
    {
        private delegate BND2Entry GetEntry();
        private delegate void SetEntry(BND2Entry entry);

        private BND2Entry _entry;
        public BND2Entry Entry
        {
            get
            {
                if (InvokeRequired)
                {
                    GetEntry method = () =>
                    {
                        return _entry;
                    };
                    return (BND2Entry)Invoke(method);
                }
                else
                {
                    return _entry;
                }
            }
            set
            {
                if (InvokeRequired)
                {
                    SetEntry method = (BND2Entry entry) =>
                    {
                        _entry = entry;
                        Task.Run(()=>UpdateDisplay());
                    };
                    Invoke(method, value);
                }
                else
                {
                    _entry = value;
                    UpdateDisplay();
                }
            }
        }

        /*public bool ForceHex
        {
            get;
            set;
        }*/

        public EntryEditor()
        {
            InitializeComponent();

            ImageMenuVisible = false;
            BinaryMenuVisible = false;
            ImageVisible = false;
            TabsVisible = false;

            //txtData.MaxLength = int.MaxValue;
            //txtExtraData.MaxLength = int.MaxValue;
        }

        private void FocusTab()
        {
            if (tabList.SelectedTab != null)
            {
                if (tabList.SelectedTab.Controls.Count > 0)
                {
                    tabList.SelectedTab.Controls[0].Focus();
                }
            }
        }

        private delegate byte[] GetDataHex();
        private delegate void SetDataHex(byte[] hex);
        private byte[] DataHex
        {
            get
            {
                if (hexData.InvokeRequired)
                {
                    GetDataHex method = () =>
                    {
                        return hexData.HexData;
                    };
                    return (byte[])Invoke(method);
                }
                else
                {
                    return hexData.HexData;
                }
            }
            set
            {
                if (hexData.InvokeRequired)
                {
                    SetDataHex method = (byte[] hex) =>
                    {
                        hexData.HexData = hex;
                    };
                    Invoke(method, value);
                }
                else
                {
                    hexData.HexData = value;
                }
            }
        }

        private byte[] ExtraDataHex
        {
            get
            {
                if (hexExtraData.InvokeRequired)
                {
                    GetDataHex method = () =>
                    {
                        return hexExtraData.HexData;
                    };
                    return (byte[])Invoke(method);
                }
                else
                {
                    return hexExtraData.HexData;
                }
            }
            set
            {
                if (hexExtraData.InvokeRequired)
                {
                    SetDataHex method = (byte[] hex) =>
                    {
                        hexExtraData.HexData = hex;
                    };
                    Invoke(method, value);
                }
                else
                {
                    hexExtraData.HexData = value;
                }
            }
        }

        private delegate Image GetImage();
        private delegate void SetImage(Image img);
        private Image Image
        {
            get
            {
                if (pboImage.InvokeRequired)
                {
                    GetImage method = () =>
                    {
                        return pboImage.Image;
                    };
                    return (Image)Invoke(method);
                }
                else
                {
                    return pboImage.Image;
                }
            }
            set
            {
                if (pboImage.InvokeRequired)
                {
                    SetImage method = (Image img) =>
                    {
                        pboImage.Image = img;
                    };
                    Invoke(method, value);
                }
                else
                {
                    pboImage.Image = value;
                }
            }
        }

        private delegate bool GetBool();
        private delegate void SetBool(bool visible);
        private bool ImageVisible
        {
            get
            {
                if (tabList.InvokeRequired)
                {
                    GetBool method = () =>
                    {
                        return tabList.TabPages.Contains(tabImage);
                    };
                    return (bool)Invoke(method);
                }
                else
                {
                    return tabList.TabPages.Contains(tabImage);
                }
            }
            set
            {
                if (tabList.InvokeRequired)
                {
                    SetBool method = (bool visible) =>
                    {
                        if (visible && !tabList.TabPages.Contains(tabImage))
                        {
                            tabList.TabPages.Add(tabImage);
                        } else if (!visible && tabList.TabPages.Contains(tabImage))
                        {
                            tabList.TabPages.Remove(tabImage);
                        }
                    };
                    Invoke(method, value);
                }
                else
                {
                    if (value && !tabList.TabPages.Contains(tabImage))
                    {
                        tabList.TabPages.Add(tabImage);
                    }
                    else if (!value && tabList.TabPages.Contains(tabImage))
                    {
                        tabList.TabPages.Remove(tabImage);
                    }
                }
            }
        }

        private delegate void SetTitleText(string val);
        private string Title
        {
            set
            {
                if (InvokeRequired)
                {
                    SetTitleText method = (string val) =>
                    {
                        Text = val;
                    };
                    Invoke(method, value);
                }
                else
                {
                    Text = value;
                }
            }
        }
        
        private bool TabsEnabled
        {
            get
            {
                if (tabList.InvokeRequired)
                {
                    GetBool method = () =>
                    {
                        return tabList.Enabled;
                    };
                    return (bool)Invoke(method);
                }
                else
                {
                    return tabList.Enabled;
                }
            }
            set
            {
                if (tabList.InvokeRequired)
                {
                    SetBool method = (bool enabled) =>
                    {
                        tabList.Enabled = enabled;
                    };
                    Invoke(method, value);
                }
                else
                {
                    tabList.Enabled = value;
                }
            }
        }

        private bool TabsVisible
        {
            get
            {
                if (tabList.InvokeRequired)
                {
                    GetBool method = () =>
                    {
                        return tabList.Visible;
                    };
                    return (bool)Invoke(method);
                }
                else
                {
                    return tabList.Visible;
                }
            }
            set
            {
                if (tabList.InvokeRequired)
                {
                    SetBool method = (bool visible) =>
                    {
                        tabList.Visible = visible;
                    };
                    Invoke(method, value);
                }
                else
                {
                    tabList.Visible = value;
                }
            }
        }

        private bool ImageMenuVisible
        {
            get
            {
                if (mnuBinary.InvokeRequired)
                {
                    GetBool method = () =>
                    {
                        return exportToolStripMenuItem.Enabled;
                    };
                    return (bool)Invoke(method);
                }
                else
                {
                    return exportToolStripMenuItem.Enabled;
                }
            }
            set
            {
                if (mnuBinary.InvokeRequired)
                {
                    SetBool method = (bool enabled) =>
                    {
                        exportToolStripMenuItem.Enabled = enabled;
                    };
                    Invoke(method, value);
                }
                else
                {
                    exportToolStripMenuItem.Enabled = value;
                }
            }
        }

        private bool BinaryMenuVisible
        {
            get
            {
                if (mnuBinary.InvokeRequired)
                {
                    GetBool method = () =>
                    {
                        return mnuBinary.Visible;
                    };
                    return (bool)Invoke(method);
                }
                else
                {
                    return mnuBinary.Visible;
                }
            }
            set
            {
                if (mnuBinary.InvokeRequired)
                {
                    SetBool method = (bool visible) =>
                    {
                        mnuBinary.Visible = visible;
                    };
                    Invoke(method, value);
                }
                else
                {
                    mnuBinary.Visible = value;
                }
            }
        }

        private void UpdateDisplay()
        {
            Title = "Edit Entry: " + _entry.Index.ToString("d3");
            
            /*if (ForceHex)
            {
                ImageVisible = false;
                TabsVisible = true;
            }
            else
            {*/
                if (_entry.Console)
                    Image = GameImage.GetImagePS3(_entry.Data, _entry.ExtraData);
                else
                    Image = GameImage.GetImagePC(_entry.Data, _entry.ExtraData);

                ImageVisible = Image != null;

                TabsVisible = true;//!ImageVisible;
            //}
            ImageMenuVisible = ImageVisible;
            BinaryMenuVisible = TabsVisible;

            if (TabsVisible)
            {
                DataHex = _entry.Data;//.AsString();
                ExtraDataHex = _entry.ExtraData;//.AsString();
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PNG Images|*.png";
            sfd.FileOk += Sfd_FileOk;
            sfd.ShowDialog(this);
        }

        private void Sfd_FileOk(object sender, CancelEventArgs e)
        {
            SaveFileDialog sfd = (SaveFileDialog)sender;

            if (sfd.FileName == null || sfd.FileName.Length <= 0)
                return;

            string path = sfd.FileName;

            Image.Save(path, ImageFormat.Png);
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PNG Images|*.png";
            ofd.FileOk += Ofd_FileOk;
            ofd.ShowDialog(this);
        }

        private void Ofd_FileOk(object sender, CancelEventArgs e)
        {
            OpenFileDialog ofd = (OpenFileDialog)sender;

            if (ofd.FileName == null || ofd.FileName.Length <= 0 || !File.Exists(ofd.FileName))
                return;

            string path = ofd.FileName;
            Task.Run(() => ImportImage(path));
        }

        private void ImportImage(string path)
        {
            _entry.Dirty = true;
            Image newImage = Image.FromFile(path);

            ImageVisible = false;
            TabsVisible = false;
            ImageMenuVisible = false;
            BinaryMenuVisible = false;

            MemoryStream ms = new MemoryStream(_entry.Data);
            BinaryReader br = new BinaryReader(ms);
            int unknown1 = br.ReadInt32();
            int unknown2 = br.ReadInt32();
            int unknown3 = br.ReadInt32();
            int unknown4 = br.ReadInt32();
            //br.BaseStream.Seek(0x10, SeekOrigin.Begin);
            GameImage.CompressionType type = GameImage.CompressionType.UNKNOWN;
            byte[] compression = br.ReadBytes(4);
            string compressionString = Encoding.ASCII.GetString(compression);
            if (compression.Matches(new byte[] { 0x15, 0x00, 0x00, 0x00 }))
            {
                type = GameImage.CompressionType.BGRA;
            }
            else if (compressionString.StartsWith("DXT"))
            {
                switch (compressionString[3])
                {
                    case '1':
                        type = GameImage.CompressionType.DXT1;
                        break;
                    case '3':
                        type = GameImage.CompressionType.DXT3;
                        break;
                    case '5':
                        type = GameImage.CompressionType.DXT5;
                        break;
                }
            }
            int w = br.ReadInt16();
            int h = br.ReadInt16();
            int unknown5 = br.ReadInt32();
            int unknown6 = br.ReadInt32();
            br.Close();

            // Temp
            //type = CompressionType.DXT5;
            //type = CompressionType.DXT1;
            //type = CompressionType.DXT3;

            if (type == GameImage.CompressionType.BGRA)
            {
                Bitmap image = new Bitmap(newImage);
                int width = image.Width;
                int height = image.Height;
                //byte[] pixels = new byte[width * height * 4];
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

                _entry.ExtraData = mspixels.ToArray();

                MemoryStream msx = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(msx);

                bw.Write(unknown1);
                bw.Write(unknown2);
                bw.Write(unknown3);
                bw.Write(unknown4);

                //bw.Write((int)0);
                //bw.Write((int)0);
                //bw.Write((int)0);
                //bw.Write((int)1);

                bw.Write((int)0x15);
                bw.Write((short)width);
                bw.Write((short)height);

                bw.Write(unknown5);
                bw.Write(unknown6);
                
                //bw.Write((int)0x15);
                //bw.Write((int)0);

                bw.Flush();

                _entry.Data = msx.ToArray();

                bw.Close();
            }
            else
            {
                DXTCompression dxt = DXTCompression.DXT1;
                if (type == GameImage.CompressionType.DXT3)
                    dxt = DXTCompression.DXT1;
                else if (type == GameImage.CompressionType.DXT5)
                    dxt = DXTCompression.DXT5;
                GameImage.ImageInfo info = GameImage.SetImage(newImage, dxt);
                //Entry.Data = info.Data;

                MemoryStream msx = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(msx);

                bw.Write(unknown1);
                bw.Write(unknown2);
                bw.Write(unknown3);
                bw.Write(unknown4);

                //bw.Write((int)0);
                //bw.Write((int)0);
                //bw.Write((int)0);
                //bw.Write((int)1);

                bw.Write(compression);
                bw.Write((short)newImage.Width);
                bw.Write((short)newImage.Height);

                bw.Write(unknown5);
                bw.Write(unknown6);

                //bw.Write((int)0x15);
                //bw.Write((int)0);

                bw.Flush();

                Entry.Data = msx.ToArray();

                //bw.Close();
                Entry.ExtraData = info.ExtraData;
            }

            ImageVisible = false;
            TabsVisible = false;
            ImageMenuVisible = false;
            BinaryMenuVisible = false;
            Task.Run(() => UpdateDisplay());
        }

        private void importDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Binary Files|*.bin";
            ofd.FileOk += Ofd_FileOk1;
            ofd.ShowDialog(this);
        }

        private void Ofd_FileOk1(object sender, CancelEventArgs e)
        {
            OpenFileDialog ofd = (OpenFileDialog)sender;

            if (ofd.FileName == null || ofd.FileName.Length <= 0 || !File.Exists(ofd.FileName))
                return;

            Stream s = ofd.OpenFile();
            MemoryStream ms = new MemoryStream();
            s.CopyTo(ms);
            s.Close();
            byte[] data = ms.ToArray();
            ms.Close();

            Entry.Data = data;
            Entry.Dirty = true;
            
            Task.Run(() => UpdateDisplay());
        }

        private void exportDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Binary Files|*.bin";
            sfd.FileOk += Sfd_FileOk1;
            sfd.ShowDialog(this);
        }

        private void Sfd_FileOk1(object sender, CancelEventArgs e)
        {
            SaveFileDialog sfd = (SaveFileDialog)sender;

            if (sfd.FileName == null || sfd.FileName.Length <= 0)
                return;

            Stream s = sfd.OpenFile();
            MemoryStream ms = new MemoryStream(Entry.Data);
            ms.CopyTo(s);
            s.Flush();
            s.Close();
            ms.Close();
        }

        private void importExtraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Binary Files|*.bin";
            ofd.FileOk += Ofd_FileOk2;
            ofd.ShowDialog(this);
        }

        private void Ofd_FileOk2(object sender, CancelEventArgs e)
        {
            OpenFileDialog ofd = (OpenFileDialog)sender;

            if (ofd.FileName == null || ofd.FileName.Length <= 0 || !File.Exists(ofd.FileName))
                return;

            Stream s = ofd.OpenFile();
            MemoryStream ms = new MemoryStream();
            s.CopyTo(ms);
            s.Close();
            byte[] data = ms.ToArray();
            ms.Close();

            Entry.ExtraData = data;
            Entry.Dirty = true;

            Task.Run(() => UpdateDisplay());
        }

        private void exportExtraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Binary Files|*.bin";
            sfd.FileOk += Sfd_FileOk2;
            sfd.ShowDialog(this);
        }

        private void Sfd_FileOk2(object sender, CancelEventArgs e)
        {
            SaveFileDialog sfd = (SaveFileDialog)sender;

            if (sfd.FileName == null || sfd.FileName.Length <= 0)
                return;

            Stream s = sfd.OpenFile();
            MemoryStream ms = new MemoryStream(Entry.ExtraData);
            ms.CopyTo(s);
            s.Flush();
            s.Close();
            ms.Close();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            FocusTab();
        }

        private void tabList_SelectedIndexChanged(object sender, EventArgs e)
        {
            FocusTab();
        }

        private void EntryEditor_Shown(object sender, EventArgs e)
        {
            FocusTab();
        }
    }
}
