using PluginAPI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace LangEditor
{
    public partial class LangEdit : Form, IEntryEditor
    {
        public delegate void OnChanged();
        public event OnChanged Changed;

        private bool _ignoreChanges;

        private Dictionary<uint, string> dict;
        private Language _lang;
        public Language Lang
        {
            get => _lang;
            set
            {
                _ignoreChanges = true;
                _lang = value;
                UpdateDisplay();
                _ignoreChanges = false;
            }
        }

        public LangEdit()
        {
            _ignoreChanges = true;
            InitializeComponent();
            GenerateDictionary();
            _ignoreChanges = false;
        }

        public void GenerateDictionary()
        {
            dict = new Dictionary<uint, string>();
            StreamReader keys = new StreamReader("keys/keys.csv", Encoding.UTF8);
            string key;
            while ((key = keys.ReadLine()) != null)
            {
                key = key.Substring(1, key.Length - 2);
                if (key.Contains('<'))
                {
                    int subKeyFilePos = key.IndexOf("<") + 1;
                    string subKeyFile = key.Substring(subKeyFilePos, key.IndexOf(">") - subKeyFilePos);
                    StreamReader subKeys = new StreamReader("keys/" + subKeyFile + ".csv", Encoding.UTF8);
                    string subKey;
                    while ((subKey = subKeys.ReadLine()) != null)
                    {
                        string fullSubKey = key.Replace(subKeyFile, subKey); // Replace file name with subkey
                        fullSubKey = fullSubKey.Remove(subKeyFilePos - 1, 1); // Remove <
                        fullSubKey = fullSubKey.Remove(subKeyFilePos - 1 + subKey.Length, 1); // Remove >
                        dict.Add(Language.HashID(fullSubKey), fullSubKey);
                    }

                    subKeys.Close();

                    continue;
                }

                dict.Add(Language.HashID(key), key);

                keys.Close();
            }
        }

        public void UpdateDisplay()
        {
            foreach (KeyValuePair<uint, string> entry in _lang.mpEntries)
            {
                if (dict.ContainsKey(entry.Key))
                    dgvMain.Rows.Add(dict[entry.Key], entry.Value);
                else
                    dgvMain.Rows.Add("0x" + entry.Key.ToString("X8"), entry.Value);
            }
        }

        public void RebuildLanguage()
        {
            if (_ignoreChanges)
                return;

            Dictionary<uint, string> data = new Dictionary<uint, string>();

            for (int i = 0; i < dgvMain.Rows.Count - 1; ++i)
            {
                string idString = (string)dgvMain.Rows[i].Cells[0].Value;

                uint id;
                if (idString.Length == 8 && idString.StartsWith("0x")) // Hash
                {
                    if (!uint.TryParse(idString[2..], NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out id))
                    {
                        MessageBox.Show(this, "Failed to parse ID \"" + idString + "\"", "Warning",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else // String
                {
                    id = Language.HashID(idString);
                }

                string value = (string)dgvMain.Rows[i].Cells[1].Value;
                if (data.ContainsKey(id))
                {
                    MessageBox.Show(this, "ID Already In Use " + idString, "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                data.Add(id, value);
            }

            _lang.mpEntries = data;

            Changed?.Invoke();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Column-separated values (*.csv)|*.csv|All files(*.*)|*.*";
            if (openDialog.ShowDialog() == DialogResult.Cancel)
                return;

            Dictionary<uint, string> data = new Dictionary<uint, string>();
            StreamReader file = new StreamReader(openDialog.FileName, Encoding.UTF8);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string[] pair = line.Split(',', 2);
                pair[0] = pair[0].Substring(3, 8); // Key
                if (pair[1].Length != 0) // Non-empty value
                {
                    pair[1] = pair[1].Substring(1, pair[1].Length - 2); // Remove enclosing double quotes
                    pair[1] = pair[1].Replace("\"\"", "\""); // Convert double quotes
                    pair[1] = pair[1].Replace("\\r", "\xD").Replace("\\n", "\xA"); // Convert newlines
                }
                if (!uint.TryParse(pair[0], NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out var key))
                {
                    MessageBox.Show("Could not parse ID " + pair[0] + ". Import cancelled.", "Error", MessageBoxButtons.OK);
                    return;
                }
                data.Add(key, pair[1]);
            }

            _lang.mpEntries = data;

            dgvMain.Rows.Clear();
            UpdateDisplay();

            MessageBox.Show("Import successful.", "Information", MessageBoxButtons.OK);
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Note when language is dirty, not just bundle
            if (MessageBox.Show(this, "Changes must be applied before exporting. Save now?", "Information",
                MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Column-separated values (*.csv)|*.csv";
            if (saveDialog.ShowDialog() == DialogResult.Cancel)
                return;

            RebuildLanguage();

            // CSV content as a single contiguous string.
            // Keys are prepended with 0x and padded to 8 characters.
            // In values, incompatible characters are escaped.
            // Both keys and values are enclosed in double quotes.
            string languageContent = "";
            foreach (KeyValuePair<uint, string> entry in _lang.mpEntries)
            {
                languageContent += "\"0x" + entry.Key.ToString("X8") + "\","; // Key
                string tempValue = entry.Value;
                tempValue = tempValue.Replace("\xD", "\\r").Replace("\xA", "\\n"); // Convert newlines
                tempValue = tempValue.Replace("\"", "\"\""); // Convert double quotes
                if (tempValue.Length != 0) // Enclose non-empty strings in double quotes
                {
                    tempValue = "\"" + tempValue;
                    tempValue += "\"";
                }
                languageContent += tempValue + "\r\n";
            }

            FileStream file = (FileStream)saveDialog.OpenFile();
            file.Write(Encoding.UTF8.GetBytes(languageContent));
            file.Flush();
            file.Close();

            MessageBox.Show("Export successful.", "Information", MessageBoxButtons.OK);
        }

        private void applyChangesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RebuildLanguage();

            MessageBox.Show(this, "Done!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string value = InputDialog.ShowInput(this, "Please enter the value to search for.");
            if (value == null)
                return;
            uint result = Language.HashID(value);

            string hash;
            foreach (DataGridViewRow row in dgvMain.Rows)
            {
                hash = (string)row.Cells[0].Value;
                if (result.ToString("X8") == hash)
                {
                    dgvMain.ClearSelection();
                    dgvMain.CurrentCell = row.Cells[0];
                    row.Selected = true;
                    return;
                }
            }

            MessageBox.Show(this, "Hash not found!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void hashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string value = InputDialog.ShowInput(this, "Please enter the value to hash.");
            if (value == null)
                return;
            uint result = Language.HashID(value);

            MessageBox.Show(this, "Hashed value is: " + result.ToString("X8"), "Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvMain_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dgvMain.Rows[e.RowIndex].ReadOnly = false;
        }
    }
}
