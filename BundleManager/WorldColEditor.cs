using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BundleUtilities;
using DebugHelper;
using ModelViewer.SceneData;

namespace BundleManager
{
    public partial class WorldColEditor : Form
    {
        public delegate void OnChanged();
        public event OnChanged Changed;

        private PolygonSoupList _poly;
        public PolygonSoupList Poly
        {
            get => _poly;
            set
            {
                _poly = value;
                UpdateDisplay();
            }
        }

        public WorldColEditor()
        {
            InitializeComponent();
        }

        private void UpdateDisplay()
        {
            rndMain.Scene = _poly?.MakeScene();
        }

        private void ImportModel()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Wavefront OBJ|*.obj";
            DialogResult result = ofd.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                string path = ofd.FileName;
                if (string.IsNullOrEmpty(path) || path.Trim().Length == 0)
                    return;

                bool success;
                try
                {
                    _poly.ImportObj(path);
                    success = true;
                }
                catch (ReadFailedError ex)
                {
                    MessageBox.Show(this, ex.Message, "Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    success = false;
                }
                UpdateDisplay();

                if (success)
                    Changed?.Invoke();
            }
        }

        private void ExportModel()
        {
            rndMain.ExportWavefrontObj();
        }

        private void importModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportModel();
        }

        private void exportModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportModel();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void debugInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_poly != null)
                DebugUtil.ShowDebug(this, _poly);
        }

        private void removeWreckSurfacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _poly?.RemoveWreckSurfaces();
            UpdateDisplay();

            Changed?.Invoke();
        }
    }
}
