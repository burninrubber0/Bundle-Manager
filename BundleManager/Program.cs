using System;
using System.Windows.Forms;

namespace BundleManager
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm form = new MainForm();
            if (args.Length == 1)
                form.Open(args[0], false);

            Application.Run(form);
        }
    }
}
