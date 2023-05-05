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
            _ignoreChanges = false;
        }

        public void UpdateDisplay()
        {
            foreach (uint key in _lang.mpEntries.Keys)
                dgvMain.Rows.Add(key.ToString("X8"), _lang.mpEntries[key]);
        }

        public void RebuildLanguage()
        {
            if (_ignoreChanges)
                return;

            Dictionary<uint, string> data = new Dictionary<uint, string>();

            for (int i = 0; i < dgvMain.Rows.Count - 1; ++i)
            {
                string idString = (string)dgvMain.Rows[i].Cells[0].Value;

                if (!uint.TryParse(idString, NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out var id))
                {
                    MessageBox.Show(this, "Failed to parse ID \"" + idString + "\"", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
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
            // TODO: Import CSV
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
                tempValue = tempValue.Replace("\xD", "\\r").Replace("\xA", "\\n"); // Newlines
                tempValue = tempValue.Replace("\"", "\"\""); // Quotation marks
                if (tempValue != "") // Enclose non-empty strings in double quotes
                {
                    tempValue = "\"" + tempValue;
                    tempValue += "\"";
                }
                languageContent += tempValue + "\r\n";
            }

            FileStream file = (FileStream)saveDialog.OpenFile();
            file.Write(Encoding.ASCII.GetBytes(languageContent));
            file.Flush();
            file.Close();
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
