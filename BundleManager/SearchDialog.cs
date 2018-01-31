using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BundleUtilities;

namespace BundleManager
{
    public partial class SearchDialog : Form
    {
        public static string LastSearch = "";

        public delegate void OnSearch(ulong id);
        public event OnSearch Search;

        public SearchDialog()
        {
            InitializeComponent();

            txtSearch.Text = LastSearch;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string input = txtSearch.Text;
            if (input.Length == 0)
            {
                MessageBox.Show(this, "Please enter a search query!", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            LastSearch = txtSearch.Text;

            string numInput = input;

            if (numInput.StartsWith("0x"))
                numInput = numInput.Substring(2);

            ulong id;
            if (ulong.TryParse(numInput, NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out id))
            {
                Search?.Invoke(id);
                Close();
                return;
            }

            id = Crc32.HashCrc32B(input);
            Search?.Invoke(id);
            Close();
        }
    }
}
