using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BundleFormat;
using BundleUtilities;
using BurnoutImage;

namespace BundleManager
{
    public partial class EntryEditor : Form
    {
        private delegate BundleEntry GetEntry();
        private delegate void SetEntry(BundleEntry entry);

        private BundleEntry _entry;
        public BundleEntry Entry
        {
            get
            {
                if (InvokeRequired)
                {
                    GetEntry method = () =>
                    {
                        return _entry;
                    };
                    return (BundleEntry)Invoke(method);
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
                    SetEntry method = (BundleEntry entry) =>
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

        public bool ForceHex
        {
            get;
            set;
        }

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
        private delegate string GetString();
        private delegate void SetString(string text);
        private delegate TabControl.TabPageCollection GetTabPages();
        private delegate TabControl GetTabControl();
        private delegate TabPage GetTabPage();
        private bool ImageVisible
        {
            get
            {
                if (pboImage.InvokeRequired)
                {
                    GetBool method = () =>
                    {
                        return pboImage.Visible;
                    };
                    return (bool)Invoke(method);
                }
                else
                {
                    return pboImage.Visible;
                }
            }
            set
            {
                if (pboImage.InvokeRequired)
                {
                    SetBool method = (bool visible) =>
                    {
                        pboImage.Visible = visible;
                    };
                    Invoke(method, value);
                }
                else
                {
                    pboImage.Visible = value;
                }
            }
        }

        /*private bool ImageVisible
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
        }*/

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

        /*private bool ModelMenuVisible
        {
            get
            {
                if (mnuBar.InvokeRequired)
                {
                    GetBool method = () =>
                    {
                        return modelToolStripMenuItem.Visible;
                    };
                    return (bool)Invoke(method);
                }
                else
                {
                    return modelToolStripMenuItem.Visible;
                }
            }
            set
            {
                if (mnuBar.InvokeRequired)
                {
                    SetBool method = (bool enabled) =>
                    {
                        modelToolStripMenuItem.Visible = enabled;
                    };
                    Invoke(method, value);
                }
                else
                {
                    modelToolStripMenuItem.Visible = value;
                }
            }
        }*/

        private bool ImageMenuVisible
        {
            get
            {
                if (mnuBar.InvokeRequired)
                {
                    GetBool method = () =>
                    {
                        return imageToolStripMenuItem.Visible;
                    };
                    return (bool)Invoke(method);
                }
                else
                {
                    return imageToolStripMenuItem.Visible;
                }
            }
            set
            {
                if (mnuBar.InvokeRequired)
                {
                    SetBool method = (bool enabled) =>
                    {
                        imageToolStripMenuItem.Visible = enabled;
                    };
                    Invoke(method, value);
                }
                else
                {
                    imageToolStripMenuItem.Visible = value;
                }
            }
        }

        private bool BinaryMenuVisible
        {
            get
            {
                if (mnuBar.InvokeRequired)
                {
                    GetBool method = () =>
                    {
                        return binaryToolStripMenuItem.Visible;
                    };
                    return (bool)Invoke(method);
                }
                else
                {
                    return mnuBar.Visible;
                }
            }
            set
            {
                if (mnuBar.InvokeRequired)
                {
                    SetBool method = (bool visible) =>
                    {
                        binaryToolStripMenuItem.Visible = visible;
                    };
                    Invoke(method, value);
                }
                else
                {
                    mnuBar.Visible = value;
                }
            }
        }

        private bool MenuVisible
        {
            get
            {
                if (mnuBar.InvokeRequired)
                {
                    GetBool method = () =>
                    {
                        return mnuBar.Visible;
                    };
                    return (bool)Invoke(method);
                }
                else
                {
                    return mnuBar.Visible;
                }
            }
            set
            {
                if (mnuBar.InvokeRequired)
                {
                    SetBool method = (bool visible) =>
                    {
                        mnuBar.Visible = visible;
                    };
                    Invoke(method, value);
                }
                else
                {
                    mnuBar.Visible = value;
                }
            }
        }

        /*private bool BodyTabVisible
        {
            get
            {
                if (tabBody.InvokeRequired)
                {
                    GetBool method = () =>
                    {
                        return tabBody.Enabled;
                    };
                    return (bool)Invoke(method);
                }
                else
                {
                    return tabBody.Enabled;
                }
            }
            set
            {
                if (tabBody.InvokeRequired)
                {
                    SetBool method = (bool visible) =>
                    {
                        tabBody.Enabled = visible;
                    };
                    Invoke(method, value);
                }
                else
                {
                    tabBody.Enabled = value;
                }
            }
        }

        private bool HeaderTabVisible
        {
            get
            {
                if (tabHeader.InvokeRequired)
                {
                    GetBool method = () =>
                    {
                        return tabHeader.Enabled;
                    };
                    return (bool)Invoke(method);
                }
                else
                {
                    return tabHeader.Enabled;
                }
            }
            set
            {
                if (tabHeader.InvokeRequired)
                {
                    SetBool method = (bool visible) =>
                    {
                        tabHeader.Enabled = visible;
                    };
                    Invoke(method, value);
                }
                else
                {
                    tabHeader.Enabled = value;
                }
            }
        }*/

        /*private TabControl TabList
        {
            get
            {
                if (tabList.InvokeRequired)
                {
                    GetTabControl method = () =>
                    {
                        return tabList;
                    };
                    return (TabControl)tabList.Invoke(method);
                }
                else
                {
                    return tabList;
                }
            }
        }

        private TabPage TabBody
        {
            get
            {
                if (tabBody.InvokeRequired)
                {
                    GetTabPage method = () =>
                    {
                        return tabBody;
                    };
                    return (TabPage)tabBody.Invoke(method);
                }
                else
                {
                    return tabBody;
                }
            }
        }*/

        private string InfoText
        {
            get
            {
                if (mnuBar.InvokeRequired)
                {
                    GetString method = () =>
                    {
                        return txtInfo.Text;
                    };
                    return (string)Invoke(method);
                }
                else
                {
                    return txtInfo.Text;
                }
            }
            set
            {
                if (mnuBar.InvokeRequired)
                {
                    SetString method = (string text) =>
                    {
                        txtInfo.Text = text;
                    };
                    Invoke(method, value);
                }
                else
                {
                    txtInfo.Text = value;
                }
            }
        }

        private string GetInfo()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("ID: 0x" + _entry.ID.ToString("X8"));
            sb.AppendLine("Type: " + _entry.Type);
            sb.AppendLine("Has Header: " + _entry.HasHeader);
            sb.AppendLine("Has Body: " + _entry.HasBody);
            List<BundleDependency> dependencies = _entry.GetDependencies();
            if (dependencies.Count > 0)
            {
                sb.AppendLine("Dependencies[" + dependencies.Count + "] = {");
                for (int i = 0; i < dependencies.Count; i++)
                {
                    BundleDependency bundleDependency = dependencies[i];
                    sb.AppendLine("    " + bundleDependency.ToString());
                }
                sb.AppendLine("}");
            }
            else
            {
                sb.AppendLine("Dependencies[" + dependencies.Count + "];");
            }

            return sb.ToString();
        }

        private void UpdateDisplay()
        {
            Title = "Edit Entry: " + _entry.Index.ToString("d3");
            
            if (ForceHex)
            {
                ImageVisible = false;
            }
            else if (_entry.Type == EntryType.RasterResourceType)
            {
				Texture texture;
                if (_entry.Console)
					texture = GameImage.GetImagePS3(_entry.EntryBlocks[0].Data, _entry.EntryBlocks[1].Data);
				else
					texture = GameImage.GetImage(_entry.EntryBlocks[0].Data, _entry.EntryBlocks[1].Data);

                ImageVisible = Image != null;

            }
            TabsVisible = !ImageVisible;
            //}
            ImageMenuVisible = ImageVisible;
            BinaryMenuVisible = TabsVisible;

            //ModelMenuVisible = _entry.Type == EntryType.RwRenderableResourceType;

            if (TabsVisible)
            {
				DataHex = _entry.EntryBlocks[0].Data;//.AsString();
                ExtraDataHex = _entry.EntryBlocks[1].Data;//.AsString();
            }

            MenuVisible = true;
            
            /*try
            {
                if (_entry.HasBody && !TabList.TabPages.Contains(TabBody))
                    TabList.TabPages.Add(TabBody);
                else if (!_entry.HasBody && TabList.TabPages.Contains(TabBody))
                    TabList.TabPages.Remove(TabBody);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
            }*/

            InfoText = GetInfo();
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

			ImageHeader header = GameImage.GetImageHeader(_entry.EntryBlocks[0].Data);

            ImageInfo info = GameImage.SetImage(newImage, header.CompressionType);

			Entry.EntryBlocks[0].Data = info.Header;
            Entry.EntryBlocks[1].Data = info.Data;

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

            Entry.EntryBlocks[0].Data = data;
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
            MemoryStream ms = new MemoryStream(Entry.EntryBlocks[0].Data);
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

            Entry.EntryBlocks[1].Data = data;
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
            MemoryStream ms = new MemoryStream(Entry.EntryBlocks[1].Data);
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

        private struct Vertex
        {
            public float X, Y, Z;
        }

        /*private int GetVertexSize()
        {
            List<int> vertexSizes = new List<int>();
            foreach (BundleEntry entry in Entry.Archive.Entries)
            {
                if (entry.Type != EntryType.RwVertexDescResourceType)
                    continue;

                byte[] entryData = entry.EntryBlocks[0].Data;

                MemoryStream ems = new MemoryStream(entryData);
                BinaryReader2 ebr = new BinaryReader2(ems);
                ebr.BigEndian = entry.Console;

                ebr.BaseStream.Position += 17;
                int vSize = ebr.ReadByte();

                ebr.Close();
                ems.Close();

                if (!vertexSizes.Contains(vSize))
                    vertexSizes.Add(vSize);
            }

            vertexSizes.Reverse();

            int vertexSize;

            if (vertexSizes.Count > 1)
            {
                VertexSizePicker picker = new VertexSizePicker();
                picker.VertexSizeList = vertexSizes;
                picker.ShowDialog(this);
                vertexSize = picker.VertexSize;
            }
            else if (vertexSizes.Count == 1)
            {
                vertexSize = vertexSizes[0];
            }
            else
            {
                vertexSize = -1;
            }

            return vertexSize;
        }*/

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Entry.Type != EntryType.RwRenderableResourceType)
                return;

            string modelInfo = "";

            byte[] header = Entry.EntryBlocks[0].Data;

            MemoryStream ms = new MemoryStream(header);
            BinaryReader2 br = new BinaryReader2(ms);
            br.BigEndian = Entry.Console;

            br.BaseStream.Position = 0x24;
            int offset = br.ReadInt32();
            br.BaseStream.Position = offset;

            int vertexBlockOff = br.ReadInt32();
            int dummy = br.ReadInt32();
            int vertexBlockSize = br.ReadInt32();

            long paddingCount = 16 - (br.BaseStream.Position % 16);
            br.BaseStream.Position += paddingCount;

            long blablaOff = br.BaseStream.Position;

            br.BaseStream.Position = blablaOff + 0x4C;
            int vertexCount = br.ReadInt32();
            br.BaseStream.Position = blablaOff + 0x54;
            int polyCount = br.ReadInt32();
            int indexCount = polyCount * 3;

            br.Close();
            ms.Close();

            modelInfo = "Model Info:\n";
            modelInfo += "Vertex Block Offset: " + vertexBlockOff.ToString("X8") + "\n";
            modelInfo += "Vertex Block Size: " + vertexBlockSize.ToString("X8") + "\n";
            modelInfo += "Vertex Count: " + vertexCount.ToString() + "\n";
            modelInfo += "Polygon Count: " + polyCount.ToString() + "\n";
            modelInfo += "Index Count: " + indexCount.ToString() + "\n";

            MessageBox.Show(this, modelInfo, "DEBUG", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /*private void exportObjToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Entry.Type != EntryType.RwRenderableResourceType)
                return;

            int vertexSize = GetVertexSize();
            if (vertexSize == -1)
            {
                MessageBox.Show(this, "No VertexDesc was found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            byte[] header = Entry.EntryBlocks[0].Data;

            MemoryStream ms = new MemoryStream(header);
            BinaryReader2 br = new BinaryReader2(ms);
            br.BigEndian = Entry.Console;

            br.BaseStream.Position = 0x24;
            int offset = br.ReadInt32();
            br.BaseStream.Position = offset;

            int vertexBlockOff = br.ReadInt32();
            int dummy = br.ReadInt32();
            int vertexBlockSize = br.ReadInt32();

            long paddingCount = 16 - (br.BaseStream.Position % 16);
            br.BaseStream.Position += paddingCount;

            long blablaOff = br.BaseStream.Position;

            br.BaseStream.Position = blablaOff + 0x4C;
            int vertexCount = br.ReadInt32();
            br.BaseStream.Position = blablaOff + 0x54;
            int polyCount = br.ReadInt32();
            int indexCount = polyCount * 3;

            br.Close();
            ms.Close();

            try
            {

                byte[] data = Entry.EntryBlocks[1].Data;
                ms = new MemoryStream(data);
                br = new BinaryReader2(ms);
                br.BigEndian = Entry.Console;

                List<short> indices = new List<short>();

                for (int i = 0; i < indexCount; i++)
                {
                    indices.Add((short)(br.ReadInt16() + 1));
                }

                br.BaseStream.Position = vertexBlockOff;

                List<Vertex> vertices = new List<Vertex>();

                for (int i = 0; i < vertexCount; i++)
                {
                    br.BaseStream.Position = vertexBlockOff + vertexSize * i;
                    Vertex vertex = new Vertex();
                    vertex.X = br.ReadSingle();
                    vertex.Y = br.ReadSingle();
                    vertex.Z = br.ReadSingle();
                    vertices.Add(vertex);
                }

                br.Close();
                ms.Close();

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Wavefront OBJ|*.obj|All Files|*.*";
                DialogResult result = sfd.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    Stream s = File.Open(sfd.FileName, FileMode.Create);
                    StreamWriter sw = new StreamWriter(s);

                    for (int i = 0; i < vertices.Count; i++)
                    {
                        Vertex v = vertices[i];
                        sw.WriteLine("v " + v.X + " " + v.Y + " " + v.Z);
                    }

                    sw.WriteLine();
                    sw.WriteLine("g submesh_0");

                    for (int i = 0; i < indices.Count; i += 3)
                    {
                        int index1 = indices[i + 0];
                        int index2 = indices[i + 1];
                        int index3 = indices[i + 2];
                        sw.WriteLine("f " + index1 + " " + index2 + " " + index3);
                    }

                    sw.WriteLine();

                    sw.Flush();
                    sw.Close();
                    sw.Close();
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(this, "Error: " + ex.Message + "\n\n" + ex.StackTrace, "DEBUG", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/
    }
}
