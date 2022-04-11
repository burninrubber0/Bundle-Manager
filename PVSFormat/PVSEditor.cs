using System.Drawing;
using System.Windows.Forms;
using BundleFormat;
using DebugHelper;
using PluginAPI;

namespace PVSFormat
{
    public partial class PVSEditor : Form, IEntryEditor
    {
        private PVS _currentPVS;

        public Image GameMap
        {
            get => pvsMain.GameMap;
            set => pvsMain.GameMap = value;
        }

        public PVSEditor()
        {
            InitializeComponent();
        }

        public void UpdateDisplay()
        {
            //dbgMain.SelectedObject = _currentPVS;
            pvsMain.PVS = _currentPVS;
        }

        public void Open(PVS pvs)
        {
            _currentPVS = pvs;

            UpdateDisplay();
        }

        private void DebugToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            DebugUtil.ShowDebug(_currentPVS);
        }
    }
}
