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
using BaseHandlers;
using BundleFormat;
using BundleUtilities;
using BurnoutImage;
using DebugHelper;
using ModelViewer.SceneData;
using PluginAPI;
using Util = BundleFormat.Util;

namespace BundleManager
{
    public partial class MainForm : Form
    {
		#region Variables and Properties

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

		public bool ForceOnlySpecificEntry = false;

		private Thread _openSaveThread;

		#endregion

		public MainForm()
        {
            InitializeComponent();

            //doNew();
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            lstEntries.Items.Clear();

            dumpAllCollisionsToolStripMenuItem.Enabled = false;
            removeWreckSurfacesToolStripMenuItem.Enabled = false;

            if (CurrentArchive == null)
            {
                lstEntries.Enabled = false;
                return;
            }
            else
            {
                lstEntries.Enabled = true;
            }

            lstEntries.BeginUpdate();
            lstEntries.ListViewItemSorter = null; // Disable sorting while adding

            for (int i = 0; i < CurrentArchive.Entries.Count; i++)
            {
                BundleEntry entry = CurrentArchive.Entries[i];
                if (entry.Type == EntryType.PolygonSoupListResourceType)
                {
                    dumpAllCollisionsToolStripMenuItem.Enabled = true;
                    removeWreckSurfacesToolStripMenuItem.Enabled = true;
                }
                Color color = entry.GetColor();
                string[] values = new string[]
                {
                    i.ToString("d3"),
                    entry.DetectName(),
                    "0x" + entry.ID.ToString("X8"),
                    entry.Type.ToString(),
                    entry.EntryBlocks[0].Data.Length.ToString(),
                    entry.EntryBlocks[0].Data.MakePreview(0, 16)
                };

                ListViewItem item = new ListViewItem(values);
                item.BackColor = color;
                lstEntries.Items.Add(item);
            }

            lstEntries.ListViewItemSorter = new EntrySorter(0); // Also calls Sort
            lstEntries.EndUpdate();
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
            if (CurrentArchive.Console)
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

        public void EditSelectedEntry(bool forceHex = false)
        {
            int count = lstEntries.SelectedIndices.Count;
            if (count <= 0)
                return;

            int index;
            if (!int.TryParse(lstEntries.SelectedItems[0].Text, out index))
                return;

			EditEntry(index, forceHex);
        }

		public void EditEntry(int index, bool forceHex = false)
		{
			BundleEntry entry = GetEntry(index);

			if (EntryTypeRegistry.IsRegistered(entry.Type) && !forceHex)
			{
				IEntryData data = EntryTypeRegistry.GetHandler(entry.Type);

				TextureCache.ResetCache();
				LoadingDialog loader = new LoadingDialog();
				loader.Status = "Loading: " + entry.ID.ToString("X8");

				Thread loadInstanceThread = null;
				bool failure = false;

				loader.Done += (cancelled, value) =>
				{
					if (cancelled)
						loadInstanceThread?.Abort();
					else
					{
						if (failure)
						{
							MessageBox.Show(this, "Failed to load Entry", "Error", MessageBoxButtons.OK,
								MessageBoxIcon.Error);
						}
						else
						{
							loader.Hide();

							IEntryEditor editor = data.GetEditor(entry);
							if (editor != null)
								editor.ShowDialog(this);
							else
								DebugUtil.ShowDebug(this, data);
							if (ForceOnlySpecificEntry)
								Environment.Exit(0);
						}
					}
					TextureCache.ResetCache();
				};

				loadInstanceThread = new Thread(() =>
				{
					try
					{
						try
						{
							failure = !data.Read(entry, loader);
						}
						catch (ReadFailedError ex)
						{
							MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
							failure = true;

							throw;
						}
					}
					catch (Exception)
					{
						MessageBox.Show("Failed to load Entry", "Error", MessageBoxButtons.OK,
							MessageBoxIcon.Error);
						failure = true;
					}
					loader.IsDone = true;
				});
				loadInstanceThread.Start();
				loader.ShowDialog(this);
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

		#region Event Handlers

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
			SwitchMode();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lstEntries_ColumnClick(object sender, ColumnClickEventArgs e)
        {
			SortColumn(e.Column);
		}

        private void searchForEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void dumpAllCollisionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
			DumpAllCollisions();
        }

        private void removeWreckSurfacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
			RemoveWreckSurfaces();
		}

		#endregion

		#region Utility

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

		private void SwitchMode()
		{
			if (!CheckSave())
				return;

			CurrentArchive = null;
			CurrentFileName = null;

			Hide();
			Program.folderModeForm.Show();
		}

		private void SortColumn(int column)
		{
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

		#endregion

		#region Extra Tools

		private void DumpAllCollisions()
		{
			if (CurrentArchive == null)
				return;

			FolderBrowserDialog fbd = new FolderBrowserDialog();
			DialogResult result = fbd.ShowDialog(this);
			if (result == DialogResult.OK)
			{
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
					bw.Write(entry.EntryBlocks[0].Data);
					bw.Flush();
					bw.Close();
					outFile.Close();

					BundleEntry polyEntry = CurrentArchive.GetEntryByID(polyID);
					if (polyEntry == null)
						break;
					Stream outFilePoly = File.Open(path + "/" + polyName + ".bin", FileMode.Create, FileAccess.Write);
					BinaryWriter bwPoly = new BinaryWriter(outFilePoly);
					bwPoly.Write(polyEntry.EntryBlocks[0].Data);
					bwPoly.Flush();
					bwPoly.Close();
					outFilePoly.Close();

					PolygonSoupList poly = new PolygonSoupList();
					poly.Read(polyEntry);
					Scene scene = poly.MakeScene();
					scene.ExportWavefrontObj(path + "/" + polyName + ".obj");
				}

				MessageBox.Show(this, "Done!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void RemoveWreckSurfaces()
		{
			for (int i = 0; i < CurrentArchive.Entries.Count; i++)
            {
                BundleEntry entry = CurrentArchive.Entries[i];

                if (entry.Type == EntryType.PolygonSoupListResourceType)
                {
					PolygonSoupList list = new PolygonSoupList();
					list.Read(entry);
                    list.RemoveWreckSurfaces();
                    list.Write(entry);
                }
            }
		}

		public void ConvertImagesFromPS3ToPC_old()
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
					if (entry.EntryBlocks[0].Data.Length == 48 && entry.EntryBlocks[1].Data != null && entry.EntryBlocks[1].Data.Length > 0)
					{
						MemoryStream ms = new MemoryStream(entry.EntryBlocks[0].Data);
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

						entry.EntryBlocks[0].Data = Data;

						entry.Dirty = true;
					}
				}
			}
			else
			{
				MessageBox.Show(this, "This feature only works on PS3 Bundle Files", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
					IDList list = new IDList();
					list.Read(entry);
					list.Write(entry);
				}
				else if (entry.Type == EntryType.PolygonSoupListResourceType)
				{
					PolygonSoupList list = new PolygonSoupList();
					list.Read(entry);
					list.Write(entry);
				}
			}

			//PatchImages();

			CurrentArchive.Platform = BundlePlatform.PC;
		}

		#endregion
	}
}
