using PluginAPI;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            foreach (uint key in _lang.Data.Keys)
            {
                string txt = _lang.Data[key];
                dgvMain.Rows.Add(key.ToString("X8"), txt);
            }
        }

        public void RebuildLanguage()
        {
            if (_ignoreChanges)
                return;

            Dictionary<uint, string> data = new Dictionary<uint, string>();

            int index = 0;
            foreach (DataGridViewRow row in dgvMain.Rows)
            {
                if (index >= dgvMain.Rows.Count - 1)
                    break;
                string idString = (string) row.Cells[0].Value;

                if (!uint.TryParse(idString, NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out var id))
                {
                    MessageBox.Show(this, "Failed to parse ID for row " + index, "Warning", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                string value = (string) row.Cells[1].Value;
                if (data.ContainsKey(id))
                {
                    MessageBox.Show(this, "ID Already In Use " + idString, "Warning", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
                data.Add(id, value);

                index++;
            }
            _lang.Data = data;
            Changed?.Invoke();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Import CSV
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Export CSV
        }

        private void hashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string value = InputDialog.ShowInput(this, "Please enter the value to hash.");
            if (value == null)
                return;
            uint result = Language.HashID(value);

            MessageBox.Show(this, "Hashed value is: " + result.ToString("X8"), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string value = InputDialog.ShowInput(this, "Please enter the value to search for.");
            if (value == null)
                return;
            uint result = Language.HashID(value);

            dgvMain.ClearSelection();
            foreach (DataGridViewRow row in dgvMain.Rows)
            {
                string hash = (string)row.Cells[0].Value;
                if (result.ToString("X8") == hash)
                {
                    //MessageBox.Show(this, "FOUND!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dgvMain.CurrentCell = row.Cells[0];
                    row.Selected = true;
                    return;
                }
            }

            MessageBox.Show(this, "Hash not found!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void applyChangesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RebuildLanguage();

            MessageBox.Show(this, "Done!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvMain_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dgvMain.Rows[e.RowIndex].ReadOnly = false;
        }
    }
}
