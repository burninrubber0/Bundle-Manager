using System.Windows.Forms;

namespace DebugViewerLib.Util
{
    public static class DebugUtil
    {
        public static void ShowDebug(object obj)
        {
            Form dlg = DebugViewer.BuildDebugDlg(obj);
            dlg.ShowDialog();
        }

        public static void ShowDebug(IWin32Window owner, object obj)
        {
            Form dlg = DebugViewer.BuildDebugDlg(obj);
            dlg.ShowDialog(owner);
        }
    }
}
