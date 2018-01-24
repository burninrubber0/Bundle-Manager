using System;
using System.ComponentModel;
using System.Drawing;
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
                tsbOpenConsole.Visible = !value;
                toolStripSeparator2.Visible = !value;
                toolStripSeparator3.Visible = !value;
                tsbSwitchMode.Visible = !value;

                newToolStripMenuItem.Visible = !value;
                toolStripMenuItem3.Visible = !value;
                openToolStripMenuItem.Visible = !value;
                openConsoleToolStripMenuItem.Visible = !value;
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

        private bool __console;

        private delegate bool GetBool();

        private delegate void SetBool(bool value);

        public bool _console
        {
            get
            {
                if (InvokeRequired)
                {
                    GetBool method = () =>
                    {
                        return __console;
                    };
                    return (bool) Invoke(method);
                }
                else
                {
                    return __console;
                }
            }
            set
            {
                if (InvokeRequired)
                {
                    SetBool method = (bool val) =>
                    {
                        __console = val;
                        //Task.Run(() => UpdateDisplay());
                    };
                    Invoke(method, value);
                }
                else
                {
                    __console = value;
                    UpdateDisplay();
                }
            }
        }

        public MainForm()
        {
            InitializeComponent();

            //doNew();
            UpdateDisplay();
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
                    "0x" + entry.ID.ToString("X8"),
                    entry.Type.ToString(),
                    entry.Header.MakePreview(0, 16)
                };

                ListViewItem item = new ListViewItem(values);
                item.BackColor = color;
                lstEntries.Items.Add(item);
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
        private void Open(bool console = false)
        {
            if (!CheckSave())
                return;
            _console = console;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Supported Files|*.BUNDLE;*.BIN;*.BNDL;*.DAT;*.TEX|All Files|*.*";
            DialogResult result = ofd.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                if (!Utilities.FileExists(ofd.FileName))
                    return;

                Open(ofd.FileName, console);
            }
        }

        public void Open(string path, bool console = false)
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
            Stream s = null;
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

            BinaryReader br = new BinaryReader(s);
            
            BundleArchive archive = br.ReadBND2Archive(_console);

            loader.Value = new object[] { archive, path };

            br.Close();

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
                        BinaryReader br = new BinaryReader(ms);
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

            int index = lstEntries.SelectedIndices[0];

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
                vehicleList.Open(entry, entry.Console);
                vehicleList.ShowDialog(this);
            }
            else if (entry.Type == EntryType.ZoneListResourceType && !forceHex)
            {
                PVSEditor pvsForm = new PVSEditor();
                pvsForm.Open(entry, entry.Console);
                pvsForm.ShowDialog(this);
            }
            else if (entry.Type == EntryType.InstanceListResourceType && !forceHex)
            {
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
                };

                loadInstanceThread = new Thread(() =>
                {
                    instanceList = InstanceList.Read(entry, loader);
                    scene = instanceList.MakeScene();
                    loader.IsDone = true;
                });
                loadInstanceThread.Start();
                loader.ShowDialog(this);
                //DebugUtil.ShowDebug(this, instanceList);
            }
            else if (entry.Type == EntryType.GraphicsSpecResourceType && !forceHex)
            {
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
            }
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
            Open(false);
        }

        private void openConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open(true);
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
            Open(false);
        }

        private void tsbOpenConsole_Click(object sender, EventArgs e)
        {
            Open(true);
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
    }
}
