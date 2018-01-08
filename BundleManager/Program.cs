using System;
using System.IO;
using System.Windows.Forms;

namespace BundleManager
{
    public static class Program
    {
        public static MainForm fileModeForm;
        public static FileView folderModeForm;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            fileModeForm = new MainForm();
            folderModeForm = new FileView();

            if (args.Length == 0)
            {
                /*DialogResult result =
                    MessageBox.Show(
                        "This program has 2 modes, Folder mode and File mode. Would you like to use folder mode?",
                        "Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Application.Run(new FileView());
                } else if (result == DialogResult.No)
                {
                    Application.Run(new MainForm());
                }*/

                Application.Run(folderModeForm);
            }
            else
            {
                if (args.Length == 1)
                {
                    string path = args[0];
                    if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
                    {
                        folderModeForm.Open(path, false);
                        Application.Run(folderModeForm);
                    }
                    else
                    {
                        fileModeForm.Open(path, false);
                        Application.Run(fileModeForm);
                    }
                }
            }

        }
    }
}
