using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BundleFormat;
using BundleUtilities;

namespace BundleManager
{
    public partial class FileView : Form
    {
        private static FileView _instance;

        public static bool LoadMaterials => _instance.loadMaterialsToolStripMenuItem.CheckState == CheckState.Checked;

        private bool AlwaysIgnore => ignoreIDConflictsToolStripMenuItem.CheckState == CheckState.Checked;

        private string _currentPath
        {
            get => BundleCache.CurrentPath;
            set => BundleCache.CurrentPath = value;
        }

        private bool HasPath => Utilities.IsValidPath(_currentPath);

        private Thread _updateDisplayThread;

        private bool _console;

        public FileView()
        {
            InitializeComponent();

            _instance = this;

            UpdateRecentFiles();
        }

        private void Clear()
        {
            _currentPath = null;
            BundleCache.Files.Clear();
            BundleCache.Paths.Clear();
            BundleCache.EntryInfos.Clear();
            lstMain.Items.Clear();
        }

        private void UpdateRecentFiles()
        {
            SaveData.Load();

            int index = fileToolStripMenuItem.DropDownItems.IndexOf(recentSeparatorToolStripMenu) + 1;

            while (true)
            {
                if (index >= fileToolStripMenuItem.DropDownItems.Count)
                    break;

                if (fileToolStripMenuItem.DropDownItems[index] is ToolStripSeparator)
                    break;
                        
                fileToolStripMenuItem.DropDownItems.RemoveAt(index);
            }

            if (SaveData.RecentPaths.Count == 0)
            {
                ToolStripItem item = new ToolStripMenuItem("No Recent Items");
                item.Enabled = false;

                fileToolStripMenuItem.DropDownItems.Insert(index, item);
            }
            else
            {
                List<string> paths = SaveData.RecentPaths;
                paths.Reverse();
                for (int i = 0; i < paths.Count; i++)
                {
                    string path = paths[i];

                    ToolStripItem item = new ToolStripMenuItem(path);
                    item.Click += (sender, args) => Open(path);
                            
                    fileToolStripMenuItem.DropDownItems.Insert(index + i, item);
                }
            }
        }

        private void UpdateDisplay()
        {
            Invoke(new Action(UpdateRecentFiles));

            if (!Utilities.IsValidPath(_currentPath))
            {
                BundleCache.Files.Clear();
                BundleCache.Paths.Clear();
                BundleCache.EntryInfos.Clear();
                lstMain.Items.Clear();
            }
            lstMain.Enabled = Utilities.IsValidPath(_currentPath);

            foreach (string path in BundleCache.Paths)
            {
                string fixedPath = path.Replace('\\', '/');
                string fixedCurrentPath = _currentPath.Replace('\\', '/');

                fixedPath = fixedPath.Replace(fixedCurrentPath, "");

                if (fixedPath.StartsWith("/"))
                    fixedPath = fixedPath.Substring(1);

                string[] itemData = new string[]
                {
                    fixedPath
                };

                lstMain.Items.Add(new ListViewItem(itemData));
            }
        }

        private void LoadFileList()
        {
            lstMain.Items.Clear();
            if (HasPath)
            {
                LoadingDialog loader = new LoadingDialog();
                loader.Status = "Loading: " + _currentPath;
                loader.Done += (cancelled, value) =>
                {
                    if (cancelled)
                    {
                        _updateDisplayThread?.Abort();
                        _currentPath = null;
                    }
                    UpdateDisplay();
                };
                _updateDisplayThread = new Thread(() =>
                {
                    DoLoadFileList(loader);

                    loader.Value = new object[] { _currentPath };
                    loader.IsDone = true;
                });
                _updateDisplayThread.Start();
                loader.ShowDialog(this);
            }
        }

        private struct ConflictChoice
        {
            public bool Cancel { get; set; }
            public bool IgnoreAll { get; set; }

            public ConflictChoice(bool cancel, bool ignoreAll)
            {
                Cancel = cancel;
                IgnoreAll = ignoreAll;
            }
        }

        private ConflictChoice ResolveConflict(uint entryID, string file1, string file2)
        {
            if (AlwaysIgnore)
                return new ConflictChoice(false, true);
            bool isSet = false;
            DialogResult result = DialogResult.None;
            Invoke(new Action(() =>
            {
                result = MessageBox.Show(this, "ID Conflict 0x" + entryID.ToString("X8") + " between " +
                                                            file2 + " and " + file1 +
                                                            "\n\nWould you like to ignore this issue? (May cause other problems)\n\nAbort = Cancel\nRetry = Ignore Once\nIgnore = Ignore All",
                    "ID Conflict", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);
                isSet = true;
            }));
            while (!isSet);
            if (result == DialogResult.Retry)
            {
                return new ConflictChoice(false, false);
            } else if (result == DialogResult.Abort)
            {
                return new ConflictChoice(true, false);
            } else if (result == DialogResult.Ignore)
            {
                return new ConflictChoice(false, true);
            }

            return new ConflictChoice(false, false);
        }

        private void DoLoadFileList(ILoader loader)
        {
            BundleCache.Files.Clear();
            BundleCache.Paths.Clear();
            BundleCache.EntryInfos.Clear();

            string[] files = Directory.GetFiles(_currentPath, "*.*", SearchOption.AllDirectories);
            
            Dictionary<string, bool> fileTypes = new Dictionary<string, bool>();

            string cachePath = _currentPath + "/files.bmcache";
            if (File.Exists(cachePath))
            {
                string[] lines = File.ReadAllLines(cachePath);
                foreach (string line in lines)
                {
                    string[] data = line.Split('|');
                    if (data.Length < 2)
                        continue;

                    if (File.Exists(data[0]))
                        fileTypes.Add(data[0], data[1] == "bundle");
                }
            }

            int i = 0;
            int index = 0;
            bool ignoreAllConflicts = false;
            foreach (string file in files)
            {
                index++;

                try
                {
                    if (!fileTypes.ContainsKey(file))
                    {
                        bool isBundle = BundleArchive.IsBundle(file);
                        fileTypes.Add(file, isBundle);

                        if (!isBundle)
                            continue;
                    } else if (!fileTypes[file])
                    {
                        continue;
                    }
                    
                    int progress = index * 100 / files.Length;
                    loader.SetStatus("Loading(" + progress.ToString("D2") + "%): " + Path.GetFileName(file));
                    loader.SetProgress(progress);
                    
                    string fixedPath = file.Replace('\\', '/');
                    string fixedCurrentPath = _currentPath.Replace('\\', '/');

                    fixedPath = fixedPath.Replace(fixedCurrentPath, "");

                    if (fixedPath.StartsWith("/"))
                        fixedPath = fixedPath.Substring(1);

                    string[] itemData = new string[]
                    {
                        fixedPath
                    };

                    bool cancel = false;

                    List<EntryInfo> entryIDs = BundleArchive.GetEntryInfos(file, false);
                    foreach (EntryInfo info in entryIDs)
                    {
                        uint entryID = info.ID;
                        if (BundleCache.Files.ContainsKey(entryID))
                        {
                            if (ignoreAllConflicts)
                                continue;
                            int index1 = BundleCache.Files[entryID];
                            string otherFile = BundleCache.Paths[index1];
                            ConflictChoice choice = ResolveConflict(entryID, file, otherFile);
                            if (choice.Cancel)
                            {
                                cancel = true;
                                break;
                            }
                            if (choice.IgnoreAll)
                                ignoreAllConflicts = true;
                            continue;
                        }
                        if (!BundleCache.EntryInfos.ContainsKey(entryID))
                            BundleCache.EntryInfos.Add(entryID, info);
                        BundleCache.Files.Add(entryID, i);
                    }

                    if (cancel)
                    {
                        BundleCache.Files.Clear();
                        BundleCache.Paths.Clear();
                        BundleCache.EntryInfos.Clear();
                        lstMain.Items.Clear();
                        break;
                    }
                    BundleCache.Paths.Add(file);
                }
                catch (ThreadAbortException)
                {
                    BundleCache.Files.Clear();
                    BundleCache.Paths.Clear();
                    BundleCache.EntryInfos.Clear();

                    _currentPath = null;
                    break;
                }
                i++;
            }

            try
            {
                Stream s = File.Open(cachePath, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(s);

                foreach (string key in fileTypes.Keys)
                    sw.WriteLine(key + "|" + (fileTypes[key] ? "bundle" : "other"));

                sw.Flush();
                sw.Close();
                s.Close();
            }
            catch (IOException ex)
            {
                Invoke(new Action(() => MessageBox.Show(this, "Failed to cache file types:\n" + ex.Message + "\n" + ex.StackTrace)));
            }
        }

        public void Open(string path = "", bool console = false)
        {
            if (string.IsNullOrEmpty(path))
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                SaveData.Load();
                if (SaveData.RecentPaths.Count > 0)
                    fbd.SelectedPath = SaveData.RecentPaths[SaveData.RecentPaths.Count - 1];
                DialogResult result = fbd.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    _currentPath = fbd.SelectedPath;
                    SaveData.AddRecentPath(_currentPath);
                    SaveData.Save();
                    _console = console;
                    LoadFileList();
                }
            }
            else
            {
                _currentPath = path;
                SaveData.AddRecentPath(_currentPath);
                SaveData.Save();
                _console = console;
                LoadFileList();
            }
        }

        private void OpenBundle(int index)
        {
            string file = BundleCache.Paths[index];

            MainForm form = new MainForm();
            form.SubForm = true;
            form.Open(file, _console);

            form.ShowDialog(this);
        }

        private void Exit()
        {
            Application.Exit();
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void tsbOpen_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void tsbSwitchMode_Click(object sender, EventArgs e)
        {
            Clear();

            Hide();
            Program.fileModeForm.Show();
        }

        private void FileView_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void lstMain_DoubleClick(object sender, EventArgs e)
        {
            if (lstMain.SelectedIndices.Count > 0)
            {
                OpenBundle(lstMain.SelectedIndices[0]);
            }
        }

        /*private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Files|*.txt";
            DialogResult result = sfd.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                List<string> referenceList = new List<string>();

                string[] files = Directory.GetFiles(_currentPath, "*.*", SearchOption.AllDirectories);

                int i = 0;
                foreach (string file in files)
                {
                    if (!BundleArchive.IsBundle(file))
                        continue;

                    string fixedPath = file.Replace('\\', '/');
                    string fixedCurrentPath = _currentPath.Replace('\\', '/');

                    fixedPath = fixedPath.Replace(fixedCurrentPath, "");

                    if (fixedPath.StartsWith("/"))
                        fixedPath = fixedPath.Substring(1);

                    string[] itemData = new string[]
                    {
                        fixedPath
                    };

                    BundleArchive archive = BundleArchive.Read(file, _console);
                    referenceList.AddRange(ScanTest(archive));

                    i++;
                }

                Stream s = File.Open(sfd.FileName, FileMode.Create);
                StreamWriter sw = new StreamWriter(s);

                foreach (string reference in referenceList)
                {
                    sw.WriteLine(reference);
                }

                sw.Flush();
                sw.Close();
                s.Close();
            }
        }*/

        private List<string> ScanTest(BundleArchive archive)
        {
            List<string> referenceList = new List<string>();
            foreach (BundleEntry entry in archive.Entries)
            {
                if (entry.Type != EntryType.RwTextureStateResourceType)
                    continue;

                List<Dependency> dependencies = entry.GetDependencies();
                foreach (Dependency dependency in dependencies)
                {
                    uint id = dependency.EntryID;
                    string entryFile = BundleCache.GetFileByEntryID(id);
                    if (entryFile.ToUpper().Contains("WORLDTEX4"))
                        referenceList.Add(Path.GetFileName(archive.Path));
                }
            }
            return referenceList;
        }

        private void ignoreIDConflictsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ignoreIDConflictsToolStripMenuItem.CheckState == CheckState.Checked)
                ignoreIDConflictsToolStripMenuItem.CheckState = CheckState.Unchecked;
            else
                ignoreIDConflictsToolStripMenuItem.CheckState = CheckState.Checked;
        }

        private void loadMaterialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loadMaterialsToolStripMenuItem.CheckState == CheckState.Checked)
                loadMaterialsToolStripMenuItem.CheckState = CheckState.Unchecked;
            else
                loadMaterialsToolStripMenuItem.CheckState = CheckState.Checked;
        }

        private void dumpInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentPath == null)
            {
                MessageBox.Show("Please load a folder first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Files|*.txt|All Files|*.*";
            DialogResult result = sfd.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                string path = sfd.FileName;

                Stream s = File.Open(path, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(s);

                int maxLength = -1;

                Array types = Enum.GetValues(typeof(EntryType));

                foreach (EntryType type in types)
                {
                    int len = type.ToString().Length;

                    maxLength = Math.Max(len, maxLength);
                }

                foreach (uint key in BundleCache.EntryInfos.Keys)
                {
                    EntryInfo info = BundleCache.EntryInfos[key];

                    string thePath = info.Path;
                    string fixedPath = thePath.Replace('\\', '/');
                    string fixedCurrentPath = _currentPath.Replace('\\', '/');

                    fixedPath = fixedPath.Replace(fixedCurrentPath, "");

                    if (fixedPath.StartsWith("/"))
                        fixedPath = fixedPath.Substring(1);

                    string typeName = info.Type.ToString();

                    int len = typeName.Length;

                    int lenDiff = maxLength - len;

                    string typeString = typeName;

                    for (int i = 0; i < lenDiff; i++)
                        typeString += " ";

                    sw.WriteLine("0x" + info.ID.ToString("X8") + "\t\t" + typeString + "\t\t" + fixedPath);
                }

                sw.Flush();
                sw.Close();
                s.Close();
            }
        }
    }
}
