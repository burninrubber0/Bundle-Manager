using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BundleFormat;
using System.Threading;

namespace BND2Master
{
    public partial class MainForm : Form
    {
        private BND2Archive _archive;

        private delegate BND2Archive GetArchive();

        private delegate void SetArchive(BND2Archive archive);

        private BND2Archive CurrentArchive
        {
            get
            {
                if (InvokeRequired)
                {
                    GetArchive method = () =>
                    {
                        return _archive;
                    };
                    return (BND2Archive) Invoke(method);
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
                    SetArchive method = (BND2Archive archive) =>
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

        private string currentFileName
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
                BND2Entry entry = CurrentArchive.Entries[i];
                string[] values = new string[]
                {
                    i.ToString("d3"),
                    entry.Type.ToString(),
                    entry.DataCompressed ? "Yes" : "No",
                    entry.ExtraData == null ? "No" : "Yes",
                    entry.ExtraDataCompressed ? "Yes" : "No",
                    entry.Data.MakePreview(0, 16)
                };

                ListViewItem item = new ListViewItem(values);
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
                    return save();
                }
                else if (result == DialogResult.No)
                {
                    CurrentArchive = null;
                    currentFileName = null;
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

        private void doNew()
        {
            if (!CheckSave())
                return;
            //CurrentArchive = new BND2Archive();
            //UpdateDisplay();

            MessageBox.Show(this,
                "This feature is currently not avaliable because we don't know enough about the Bundle format to create them from scratch yet.",
                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void open(bool console)
        {
            if (!CheckSave())
                return;
            _console = console;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Supported Files|*.BUNDLE;*.BIN;*.BNDL;*.DAT;*.TEX|All Files|*.*";
            ofd.FileOk += Ofd_FileOk;
            ofd.ShowDialog(this);
        }

        private Thread OpenSaveThread;

        private void Ofd_FileOk(object sender, CancelEventArgs e)
        {
            OpenFileDialog ofd = (OpenFileDialog) sender;

            if (ofd.FileName == null || ofd.FileName.Length <= 0 || !File.Exists(ofd.FileName))
                return;

            LoadingDialog loader = new LoadingDialog();
            loader.Status = "Loading: " + ofd.FileName;
            loader.Done += Loader_Done;
            OpenSaveThread = new Thread(() => DoOpenBundle(loader, ofd.FileName));
            OpenSaveThread.Start();
            loader.ShowDialog(this);
        }

        private void Loader_Done(bool cancelled, object value)
        {
            if (cancelled)
            {
                if (OpenSaveThread != null)
                    OpenSaveThread.Abort();
                CurrentArchive = null;
                currentFileName = null;
            }
            else
            {
                object[] values = (object[]) value;
                CurrentArchive = (BND2Archive) values[0];

                if (CurrentArchive == null)
                {
                    MessageBox.Show(this, "There was an error opening archive: " + (string) values[1], "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    currentFileName = null;
                    Text = "Bundle Manager";
                }
                else
                {
                    currentFileName = (string) values[1];
                    Text = "Bundle Manager - " + currentFileName;
                }
            }
            UpdateDisplay();
        }

        //private delegate void VoidMethod();
        public void DoOpenBundle(LoadingDialog loader, string path)
        {
            Stream s = null;
            try
            {
                s = File.Open(path, FileMode.Open);
            }
            catch (IOException ex)
            {
                MessageBox.Show(this, "Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (s == null)
                return;

            BinaryReader br = new BinaryReader(s);
            
            BND2Archive archive = br.ReadBND2Archive(_console);

            loader.Value = new object[] { archive, path };

            br.Close();

            loader.IsDone = true;
        }

        public bool save()
        {
            if (CurrentArchive == null)
            {
                MessageBox.Show(this, "Nothing to save!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            if (currentFileName == null || currentFileName == "")
            {
                return saveAs();
            } else
            {
                LoadingDialog loader = new LoadingDialog();
                loader.Status = "Saving: " + currentFileName;
                loader.Done += Loader_Done1;
                OpenSaveThread = new Thread(() => DoSaveBundle(loader, currentFileName));
                OpenSaveThread.Start();
                loader.ShowDialog(this);
                return true;
            }
        }

        public bool saveAs()
        {
            if (CurrentArchive == null)
            {
                MessageBox.Show(this, "Nothing to save!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Supported Files|*.BUNDLE;*.BIN;*.BNDL;*.DAT;*.TEX|All Files|*.*";
            sfd.FileOk += Sfd_FileOk;
            return sfd.ShowDialog(this) != DialogResult.Cancel;
        }

        private void Sfd_FileOk(object sender, CancelEventArgs e)
        {
            SaveFileDialog sfd = (SaveFileDialog)sender;
            if (sfd.FileName == null || sfd.FileName.Length <= 0)
                return;

            LoadingDialog loader = new LoadingDialog();
            loader.Status = "Saving: " + sfd.FileName;
            loader.Done += Loader_Done1;
            OpenSaveThread = new Thread(() => DoSaveBundle(loader, sfd.FileName));
            OpenSaveThread.Start();
            loader.ShowDialog(this);

            currentFileName = sfd.FileName;
            Text = "Bundle Manager - " + currentFileName;
        }

        private void Loader_Done1(bool cancelled, object value)
        {
            if (cancelled)
            {
                if (OpenSaveThread != null)
                    OpenSaveThread.Abort();
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

        public void exit()
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
                    BND2Entry entry = CurrentArchive.Entries[i];
                    if (entry.Data.Length == 48 && entry.ExtraData != null && entry.ExtraSize > 0)
                    {
                        MemoryStream ms = new MemoryStream(entry.Data);
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

                        entry.Data = Data;

                        entry.Dirty = true;
                    }
                }
            } else
            {
                MessageBox.Show(this, "This feature only works on PS3 Bundle Files", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private delegate BND2Entry GetEntryDelegate(int index);
        public BND2Entry GetEntry(int index)
        {
            if (InvokeRequired)
            {
                GetEntryDelegate del = GetEntry;
                return (BND2Entry)Invoke(del, index);
            } else
            {
                return CurrentArchive.Entries[index];
            }
        }

        public void editSelectedEntry(bool forceHex)
        {
            int count = lstEntries.SelectedIndices.Count;
            if (count <= 0)
                return;

            int index = lstEntries.SelectedIndices[0];

            EntryEditor editor = new EntryEditor();
            editor.ForceHex = forceHex;
            Task.Run(()=>openEditor(editor, index));
            editor.ShowDialog(this);
        }

        public void openEditor(EntryEditor editor, int index)
        {
            editor.Entry = GetEntry(index);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            doNew();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open(false);
        }

        private void openConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open(true);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAs();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exit();
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            doNew();
        }

        private void tsbOpen_Click(object sender, EventArgs e)
        {
            open(false);
        }

        private void tsbOpenConsole_Click(object sender, EventArgs e)
        {
            open(true);
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            save();
        }

        private void lstEntries_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            editSelectedEntry(false);
        }

        private void previewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editSelectedEntry( false);
        }

        private void viewDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editSelectedEntry(true);
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
        }
    }
}
