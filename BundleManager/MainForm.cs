using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BundleFormat;
using BundleUtilities;
using DebugHelper;
using ModelViewer;
using ModelViewer.SceneData;
using PVSFormat;
using VehicleList;
using Util = BundleFormat.Util;

namespace BundleManager
{
    public partial class MainForm : Form
    {
        private bool _subForm;
        public bool SubForm
        {
            get { return _subForm; }
            set
            {
                _subForm = value;

                tsbNew.Visible = !value;
                toolStripSeparator1.Visible = !value;
                tsbOpen.Visible = !value;
                toolStripSeparator2.Visible = !value;
                toolStripSeparator3.Visible = !value;
                tsbSwitchMode.Visible = !value;

                newToolStripMenuItem.Visible = !value;
                toolStripMenuItem3.Visible = !value;
                openToolStripMenuItem.Visible = !value;
                toolStripMenuItem1.Visible = !value;

                exitToolStripMenuItem.Visible = !value;
                closeToolStripMenuItem.Visible = value;
            }
        }

        private BundleArchive _archive;

        private delegate BundleArchive GetArchive();

        private delegate void SetArchive(BundleArchive archive);

        private BundleArchive CurrentArchive
        {
            get
            {
                if (InvokeRequired)
                {
                    GetArchive method = () =>
                    {
                        return _archive;
                    };
                    return (BundleArchive) Invoke(method);
                }
                else
                {
                    return _archive;
                }
            }
            set
            {
                if (InvokeRequired)
                {
                    SetArchive method = (BundleArchive archive) =>
                    {
                        _archive = archive;
                        Task.Run(() => UpdateDisplay());
                    };
                    Invoke(method, value);
                }
                else
                {
                    _archive = value;
                    UpdateDisplay();
                }
            }
        }

        private string _currentFileName;

        private delegate string GetString();

        private delegate void SetString(string archive);

        private string CurrentFileName
        {
            get
            {
                if (InvokeRequired)
                {
                    GetString method = () =>
                    {
                        return _currentFileName;
                    };
                    return (string) Invoke(method);
                }
                else
                {
                    return _currentFileName;
                }
            }
            set
            {
                if (InvokeRequired)
                {
                    SetString method = (string filename) =>
                    {
                        _currentFileName = filename;
                        //Task.Run(() => UpdateDisplay());
                    };
                    Invoke(method, value);
                }
                else
                {
                    _currentFileName = value;
                    UpdateDisplay();
                }
            }
        }

        private delegate bool GetBool();

        private delegate void SetBool(bool value);

        public bool _console => CurrentArchive.Console;

        public MainForm()
        {
            InitializeComponent();

            //doNew();
            UpdateDisplay();
        }

        private void Search()
        {
            if (CurrentArchive == null)
                return;

            SearchDialog search = new SearchDialog();
            search.Search += id =>
            {
                BundleEntry entry = CurrentArchive.GetEntryByID(id);
                if (entry == null)
                {
                    MessageBox.Show(this, "Entry not found!", "Information", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                string indexText = CurrentArchive.Entries.IndexOf(entry).ToString("D3");
                int listIndex = -1;
                for (int i = 0; i < lstEntries.Items.Count; i++)
                {
                    ListViewItem item = lstEntries.Items[i];
                    if (item.Text == indexText)
                    {
                        listIndex = i;
                        break;
                    }
                }

                lstEntries.SelectedIndices.Clear();
                lstEntries.SelectedIndices.Add(listIndex);
                lstEntries.EnsureVisible(listIndex);
            };
            search.ShowDialog(this);
        }

        private void UpdateDisplay()
        {
            lstEntries.Items.Clear();

            if (CurrentArchive == null)
            {
                lstEntries.Enabled = false;
                return;
            }
            else
            {
                lstEntries.Enabled = true;
            }

            for (int i = 0; i < CurrentArchive.Entries.Count; i++)
            {
                BundleEntry entry = CurrentArchive.Entries[i];
                Color color = entry.GetColor();
                string[] values = new string[]
                {
                    i.ToString("d3"),
                    entry.DetectName(),
                    "0x" + entry.ID.ToString("X8"),
                    entry.Type.ToString(),
                    entry.Header.Length.ToString(),
                    entry.Header.MakePreview(0, 16)
                };

                ListViewItem item = new ListViewItem(values);
                item.BackColor = color;
                lstEntries.Items.Add(item);

                lstEntries.ListViewItemSorter = new EntrySorter(0);
                lstEntries.Sort();
            }
        }

        private bool CheckSave()
        {
            if (CurrentArchive == null)
            {
                return true;
            }
            else if (CurrentArchive.Dirty)
            {
                DialogResult result = MessageBox.Show(this, "There are unsaved changes!\nWould you like to save?",
                    "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    return Save();
                }
                else if (result == DialogResult.No)
                {
                    CurrentArchive = null;
                    CurrentFileName = null;
                    UpdateDisplay();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return true;
            }
        }

        private void DoNew()
        {
            if (!CheckSave())
                return;
            //CurrentArchive = new BundleArchive();
            //UpdateDisplay();

            MessageBox.Show(this,
                "This feature is currently not avaliable because we don't know enough about the Bundle format to create them from scratch yet.",
                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private Thread _openSaveThread;
        private void Open()
        {
            if (!CheckSave())
                return;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Supported Files|*.BUNDLE;*.BIN;*.BNDL;*.DAT;*.TEX|All Files|*.*";
            DialogResult result = ofd.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                if (!Utilities.FileExists(ofd.FileName))
                    return;

                Open(ofd.FileName);
            }
        }

        public void Open(string path)
        {
            LoadingDialog loader = new LoadingDialog();
            loader.Status = "Loading: " + path;
            loader.Done += Loader_OpenDone;
            _openSaveThread = new Thread(() => DoOpenBundle(loader, path));
            _openSaveThread.Start();
            loader.ShowDialog(this);
        }

        private void Loader_OpenDone(bool cancelled, object value)
        {
            if (cancelled)
            {
                _openSaveThread?.Abort();
                CurrentArchive = null;
                CurrentFileName = null;
            }
            else
            {
                object[] values = (object[]) value;
                CurrentArchive = (BundleArchive) values[0];

                if (CurrentArchive == null)
                {
                    MessageBox.Show(this, "There was an error opening archive: " + (string) values[1], "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CurrentFileName = null;
                    Text = "Bundle Manager";
                }
                else
                {
                    CurrentFileName = (string) values[1];
                    Text = "Bundle Manager - " + CurrentFileName;
                }
            }
            UpdateDisplay();
        }
        
        public void DoOpenBundle(LoadingDialog loader, string path)
        {
            /*Stream s = null;
            try
            {
                s = File.Open(path, FileMode.Open, FileAccess.Read);
            }
            catch (IOException ex)
            {
                MessageBox.Show(this, "Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (s == null)
                return;

            BinaryReader2 br = new BinaryReader2(s);

            BundleArchive archive = BundleArchive.Read(br);

            loader.Value = new object[] { archive, path };

            br.Close();
            */

            BundleArchive archive = BundleArchive.Read(path);

            loader.Value = new object[] { archive, path };
            loader.IsDone = true;
        }

        public bool Save()
        {
            if (CurrentArchive == null)
            {
                MessageBox.Show(this, "Nothing to save!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            if (string.IsNullOrEmpty(CurrentFileName))
            {
                return SaveAs();
            }

            LoadingDialog loader = new LoadingDialog();
            loader.Status = "Saving: " + CurrentFileName;
            loader.Done += Loader_SaveDone;
            _openSaveThread = new Thread(() => DoSaveBundle(loader, CurrentFileName));
            _openSaveThread.Start();
            loader.ShowDialog(this);
            return true;
        }

        public bool SaveAs()
        {
            if (CurrentArchive == null)
            {
                MessageBox.Show(this, "Nothing to save!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Supported Files|*.BUNDLE;*.BIN;*.BNDL;*.DAT;*.TEX|All Files|*.*";
            DialogResult result = sfd.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                if (!Utilities.IsValidPath(sfd.FileName))
                    return false;

                LoadingDialog loader = new LoadingDialog();
                loader.Status = "Saving: " + sfd.FileName;
                loader.Done += Loader_SaveDone;
                _openSaveThread = new Thread(() => DoSaveBundle(loader, sfd.FileName));
                _openSaveThread.Start();
                loader.ShowDialog(this);

                CurrentFileName = sfd.FileName;
                Text = "Bundle Manager - " + CurrentFileName;

                return true;
            }
            return false;
        }

        private void Loader_SaveDone(bool cancelled, object value)
        {
            if (cancelled)
            {
                _openSaveThread?.Abort();
            } else
            {
                CurrentArchive.Dirty = false;
            }
        }

        public void DoSaveBundle(LoadingDialog loader, string path)
        {
            //if (CurrentArchive.Console)
                ConvertToPC();
            Stream s = File.Open(path, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(s);

            bw.WriteBND2Archive(CurrentArchive);

            bw.Flush();
            bw.Close();

            loader.IsDone = true;
        }

        public void Exit()
        {
            Application.Exit();
        }

        public void PatchImages()
        {
            if (CurrentArchive == null)
            {
                MessageBox.Show(this, "No Archive Open!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (CurrentArchive.Console)
            {
                for (int i = 0; i < CurrentArchive.Entries.Count; i++)
                {
                    BundleEntry entry = CurrentArchive.Entries[i];
                    if (entry.Header.Length == 48 && entry.Body != null && entry.BodySize > 0)
                    {
                        MemoryStream ms = new MemoryStream(entry.Header);
                        BinaryReader2 br = new BinaryReader2(ms);
                        br.BigEndian = entry.Console;

                        byte compression = br.ReadByte();
                        byte[] unknown1 = br.ReadBytes(3);
                        byte[] type = Encoding.ASCII.GetBytes("DXT1");
                        if (compression == 0x85)
                        {
                            type = new byte[] { 0x15, 0x00, 0x00, 0x00 };
                        }
                        else if (compression == 0x86)
                        {
                            type = Encoding.ASCII.GetBytes("DXT1");
                        }
                        else if (compression == 0x88)
                        {
                            type = Encoding.ASCII.GetBytes("DXT5");
                        }
                        int unknown2 = Util.ReverseBytes(br.ReadInt32());
                        int width = Util.ReverseBytes(br.ReadInt16());
                        int height = Util.ReverseBytes(br.ReadInt16());
                        br.Close();

                        MemoryStream msx = new MemoryStream();
                        BinaryWriter bw = new BinaryWriter(msx);

                        bw.Write((int)0);
                        bw.Write((int)0);
                        bw.Write((int)0);
                        bw.Write((int)1);

                        bw.Write(type);
                        bw.Write((short)width);
                        bw.Write((short)height);
                        bw.Write((int)0x15);
                        bw.Write((int)0);

                        bw.Flush();

                        byte[] Data = msx.ToArray();

                        bw.Close();

                        entry.Header = Data;

                        entry.Dirty = true;
                    }
                }
            } else
            {
                MessageBox.Show(this, "This feature only works on PS3 Bundle Files", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private delegate BundleEntry GetEntryDelegate(int index);
        public BundleEntry GetEntry(int index)
        {
            if (InvokeRequired)
            {
                GetEntryDelegate del = GetEntry;
                return (BundleEntry)Invoke(del, index);
            } else
            {
                return CurrentArchive.Entries[index];
            }
        }

        public void EditSelectedEntry(bool forceHex)
        {
            int count = lstEntries.SelectedIndices.Count;
            if (count <= 0)
                return;

            int index;
            if (!int.TryParse(lstEntries.SelectedItems[0].Text, out index))
                return;

            //int index = lstEntries.SelectedIndices[0];

            BundleEntry entry = GetEntry(index);
            if (entry.Type == EntryType.VehicleListResourceType && !forceHex)
            {
                VehicleListForm vehicleList = new VehicleListForm();
                vehicleList.Edit += () =>
                {
                    entry = vehicleList.Write(entry.Console);
                    CurrentArchive.Entries[index] = entry;
                    CurrentArchive.Entries[index].Dirty = true;
                };
                vehicleList.Open(entry);
                vehicleList.ShowDialog(this);
            }
            else if (entry.Type == EntryType.ZoneListResourceType && !forceHex)
            {
                PVSEditor pvsForm = new PVSEditor();
                pvsForm.Open(entry);
                pvsForm.ShowDialog(this);
            }
            else if (entry.Type == EntryType.InstanceListResourceType && !forceHex)
            {
                TextureState.ResetCache();
                LoadingDialog loader = new LoadingDialog();
                loader.Status = "Loading: " + entry.ID.ToString("X8");

                Thread loadInstanceThread = null;
                InstanceList instanceList = null;
                Scene scene = null;
                loader.Done += (cancelled, value) =>
                {
                    if (cancelled)
                        loadInstanceThread?.Abort();
                    else
                    {
                        if (instanceList == null)
                        {
                            MessageBox.Show(this, "Failed to load Entry", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                        else
                        {
                            loader.Hide();
                            ModelViewerForm.ShowModelViewer(this, scene);
                        }
                    }
                    TextureState.ResetCache();
                };

                loadInstanceThread = new Thread(() =>
                {
                    instanceList = InstanceList.Read(entry, loader);
                    scene = instanceList.MakeScene(loader);
                    loader.IsDone = true;
                });
                loadInstanceThread.Start();
                loader.ShowDialog(this);
                //DebugUtil.ShowDebug(this, InstanceList.Read(entry, null));
            }
            else if (entry.Type == EntryType.GraphicsSpecResourceType && !forceHex)
            {
                TextureState.ResetCache();
                LoadingDialog loader = new LoadingDialog();
                loader.Status = "Loading: " + entry.ID.ToString("X8");

                Thread loadInstanceThread = null;
                GraphicsSpec instanceList = null;
                Scene scene = null;
                loader.Done += (cancelled, value) =>
                {
                    if (cancelled)
                        loadInstanceThread?.Abort();
                    else
                    {
                        if (instanceList == null)
                        {
                            MessageBox.Show(this, "Failed to load Entry", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                        else
                        {
                            loader.Hide();
                            ModelViewerForm.ShowModelViewer(this, scene);
                        }
                    }

                    TextureState.ResetCache();
                };

                loadInstanceThread = new Thread(() =>
                {
                    instanceList = GraphicsSpec.Read(entry, loader);
                    scene = instanceList.MakeScene();
                    loader.IsDone = true;
                });
                loadInstanceThread.Start();
                loader.ShowDialog(this);
                //DebugUtil.ShowDebug(this, instanceList);
            }
            else if (entry.Type == EntryType.RwRenderableResourceType && !forceHex)
            {
                TextureState.ResetCache();
                LoadingDialog loader = new LoadingDialog();
                loader.Status = "Loading: " + entry.ID.ToString("X8");

                Thread loadInstanceThread = null;
                Renderable renderable = null;
                loader.Done += (cancelled, value) =>
                {
                    if (cancelled)
                        loadInstanceThread?.Abort();
                    else
                    {
                        if (renderable == null)
                        {
                            MessageBox.Show(this, "Failed to load Entry", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                        else
                        {
                            loader.Hide();
                            Scene scene = renderable.MakeScene();
                            ModelViewerForm.ShowModelViewer(this, scene);
                        }
                    }
                    TextureState.ResetCache();
                };
                
                loadInstanceThread = new Thread(() =>
                {
                    renderable = Renderable.Read(entry, loader);
                    loader.IsDone = true;
                });
                loadInstanceThread.Start();
                loader.ShowDialog(this);
            }
            else if (entry.Type == EntryType.TriggerResourceType && !forceHex)
            {
                TriggerData triggers = TriggerData.Read(entry);
                DebugUtil.ShowDebug(this, triggers);
            }
            else if (entry.Type == EntryType.StreetDataResourceType && !forceHex)
            {
                StreetData streets = StreetData.Read(entry);
                DebugUtil.ShowDebug(this, streets);
            }
            else if (entry.Type == EntryType.AptDataHeaderType && !forceHex)
            {
                // TODO
                AptData data = AptData.Read(entry);
                //AptDataAlt data = AptDataAlt.Read(entry);
                DebugUtil.ShowDebug(this, data);
            }
            else if (entry.Type == EntryType.ProgressionResourceType && !forceHex)
            {
                ProgressionData progression = ProgressionData.Read(entry);
                DebugUtil.ShowDebug(this, progression);
            } else if (entry.Type == EntryType.PolygonSoupListResourceType && !forceHex)
            {
                PolygonSoupList list = PolygonSoupList.Read(entry);
				// UNCOMMENT ME TO DEBUG MODEL VIEWER!!
                Scene scene = list.MakeScene();
                ModelViewerForm.ShowModelViewer(this, scene);
                //DebugUtil.ShowDebug(this, list);
            }
            else if (entry.Type == EntryType.IDList && !forceHex)
            {
                IDList list = IDList.Read(entry);
                DebugUtil.ShowDebug(this, list);
            }
            else if (entry.Type == EntryType.AttribSysVaultResourceType && !forceHex)
            {
                try
                {
                    AttribSys at = AttribSys.Read(entry);
                    DebugUtil.ShowDebug(this, at);
                }
                catch (ReadFailedError ex)
                {
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (entry.Type == EntryType.LanguageResourceType && !forceHex)
            {
                Language language = Language.Read(entry);
                //DebugUtil.ShowDebug(this, language);
                LangEdit edit = new LangEdit();
                edit.Lang = language;
                edit.Changed += () =>
                {
                    edit.Lang.Write(entry);
                };
                edit.ShowDialog(this);
            }
            /* else if (entry.Type == EntryType.ModelResourceType && !forceHex)
            {
                for (int i = 0; i < CurrentArchive.Entries.Count; i++)
                {
                    BundleEntry entry2 = CurrentArchive.Entries[i];
                    if (entry2.Type == EntryType.ModelResourceType)
                    {
                        List<Dependency> dependencies = entry2.GetDependencies();
                        foreach (Dependency dep in dependencies)
                        {
                            uint id = dep.EntryID;
                            if (id == 0x172E1475 || id == 0x6D5CEDB6 || id == 0xF2DCDF89)
                            {
                                MessageBox.Show(this, "Found: 0x" + id.ToString("X8") + " in 0x" + entry2.ID.ToString("X8"));
                            }
                        }
                    }
                }
            }*/
            else
            {
                EntryEditor editor = new EntryEditor();
                editor.ForceHex = forceHex;
                Task.Run(() => openEditor(editor, index));
                editor.ShowDialog(this);
            }
        }

        public void openEditor(EntryEditor editor, int index)
        {
            editor.Entry = GetEntry(index);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoNew();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            DoNew();
        }

        private void tsbOpen_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void lstEntries_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            EditSelectedEntry(false);
        }

        private void previewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditSelectedEntry(false);
        }

        private void viewDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditSelectedEntry(true);
        }

        private void patchImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PatchImages();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CheckSave())
            {
                e.Cancel = true;
            }

            if (!SubForm)
                Application.Exit();
        }

        private void tsbSwitchMode_Click(object sender, EventArgs e)
        {
            if (!CheckSave())
                return;

            CurrentArchive = null;
            CurrentFileName = null;

            Hide();
            Program.folderModeForm.Show();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /*private void modelCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int modelCount = 0;
            foreach (BundleEntry entry in CurrentArchive.Entries)
            {
                if (entry.Type == EntryType.Model)
                    modelCount++;
            }

            MessageBox.Show(this, "Model Count: " + modelCount, "DEBUG", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }*/

        private void lstEntries_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int column = e.Column;
			EntrySorter sorter;

			if (lstEntries.ListViewItemSorter is EntrySorter)
			{
				// every other time
				sorter = lstEntries.ListViewItemSorter as EntrySorter;
				sorter.Column = column;
			}
			else
			{
				// first time
				sorter = new EntrySorter(column);
				lstEntries.ListViewItemSorter = sorter;
			}
			// if you're clicking the same column already being sorted
            //if (sorter.Column == column)
            //{
				// change the direction state from true to false or vice-versa
                sorter.Swap();
				// bop-it
				lstEntries.Sort();
			//}
		}

        private class EntrySorter : IComparer
        {
            public int Column;
            public bool Direction;

            public EntrySorter(int column)
            {
                this.Column = column;
                this.Direction = false;
            }

            public int Compare(object x, object y)
            {
                ListViewItem itemX = (ListViewItem)x;
                ListViewItem itemY = (ListViewItem)y;

                if (Column > itemX?.SubItems.Count || Column > itemY?.SubItems.Count)
                {
                    if (this.Direction)
                        return -1;
                    return 1;
                }

                string iX = itemX?.SubItems[Column].Text;
                string iY = itemY?.SubItems[Column].Text;

				/*
                if (int.TryParse(iX, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out var iXint))
                {
                    if (int.TryParse(iY, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out var iYint))
                    {
                        int val2 = iXint.CompareTo(iYint);
                        if (this.Direction)
                            return val2 * -1;
                        return val2;
                    }
                }
				*/

                int val = Math.Sign(string.Compare(iX, iY, StringComparison.CurrentCultureIgnoreCase));
                if (this.Direction)
                    return val * -1;
                return val;

            }

            public void Swap()
            {
                this.Direction = !this.Direction;
            }
        }

        private void searchForEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void dumpAllCollisionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentArchive == null)
                return;

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                //PolygonSoupChunk.Upper = 0;
                string path = fbd.SelectedPath;

                for (int i = 0; ; i++)
                {
                    string idListName = "trk_clil" + i;
                    string polyName = "trk_col_" + i;

                    ulong idListID = Crc32.HashCrc32B(idListName);
                    ulong polyID = Crc32.HashCrc32B(polyName);

                    BundleEntry entry = CurrentArchive.GetEntryByID(idListID);
                    if (entry == null)
                        break;
                    Stream outFile = File.Open(path + "/" + idListName + ".bin", FileMode.Create, FileAccess.Write);
                    BinaryWriter bw = new BinaryWriter(outFile);
                    bw.Write(entry.Header);
                    bw.Flush();
                    bw.Close();
                    outFile.Close();

                    BundleEntry polyEntry = CurrentArchive.GetEntryByID(polyID);
                    if (polyEntry == null)
                        break;
                    Stream outFilePoly = File.Open(path + "/" + polyName + ".bin", FileMode.Create, FileAccess.Write);
                    BinaryWriter bwPoly = new BinaryWriter(outFilePoly);
                    bwPoly.Write(polyEntry.Header);
                    bwPoly.Flush();
                    bwPoly.Close();
                    outFilePoly.Close();

                    PolygonSoupList poly = PolygonSoupList.Read(polyEntry);
                    Scene scene = poly.MakeScene();
                    scene.ExportWavefrontObj(path + "/" + polyName + ".obj");
                }

                //MessageBox.Show(this, PolygonSoupChunk.Upper.ToString("X4"), "DEBUG", MessageBoxButtons.OK, MessageBoxIcon.Error);

                MessageBox.Show(this, "Done!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void ConvertToPC()
        {
            // TODO: Support everything

            for (int i = 0; i < CurrentArchive.Entries.Count; i++)
            {
                BundleEntry entry = CurrentArchive.Entries[i];

                if (entry.Type == EntryType.IDList)
                {
                    IDList list = IDList.Read(entry);
                    list.Write(entry);
                }
                else if (entry.Type == EntryType.PolygonSoupListResourceType)
                {
                    PolygonSoupList list = PolygonSoupList.Read(entry);
                    list.Write(entry);
                }
            }

            CurrentArchive.Platform = BundlePlatform.PC;
        }
    }
}
