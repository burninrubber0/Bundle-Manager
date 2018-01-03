using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void Open(BND2Entry entry, bool console)
        {
            _currentPVS = PVS.Read(entry, console);

            UpdateDisplay();
        }
    }
}
