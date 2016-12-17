using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComponentTester
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            /*if (!File.Exists("test.bin"))
            {
                MessageBox.Show("Missing test.bin!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }*/
            Application.Run(new Tester());
        }
    }
}
