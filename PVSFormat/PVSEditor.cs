using System.Windows.Forms;
using BundleFormat;

namespace PVSFormat
{
    public partial class PVSEditor : Form
    {
        private PVS _currentPVS;

        public PVSEditor()
        {
            InitializeComponent();
        }

        public void UpdateDisplay()
        {
            dbgMain.SelectedObject = _currentPVS;
        }

        public void Open(BundleEntry entry, bool console)
        {
            _currentPVS = PVS.Read(entry, console);

            UpdateDisplay();
        }
    }
}
